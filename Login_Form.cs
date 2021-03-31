﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LibraryApp
{
    public partial class Login_Form : Form
    {
        public Login_Form()
        {
            InitializeComponent();
        }

        public string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\ТУ Варна\Семестър 6\ТСП - проект\LibraryApp\LibraryDB.mdf;Integrated Security=True";
        public SqlConnection myConnection = default(SqlConnection);
        public SqlCommand myCommand = default(SqlCommand);

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
        }

        private void label_termsClick(object sender, EventArgs e)
        {
            Terms_Form terms = new Terms_Form();
            terms.Show();
        }

        private void label_forgotClick(object sender, EventArgs e)
        {
            Forgot_Form forgot = new Forgot_Form();
            forgot.Show();
            this.Hide();
        }

        private void button_SignInClick(object sender, EventArgs e)
        {
            try
            {
                myConnection = new SqlConnection(cs);
                myCommand = new SqlCommand("SELECT * FROM Accounts WHERE username = @username AND password = @password", myConnection);
                SqlParameter username = new SqlParameter("@username", SqlDbType.VarChar);
                SqlParameter password = new SqlParameter("@password", SqlDbType.VarChar);

                username.Value = textBox1.Text;
                password.Value = textBox2.Text;

                myCommand.Parameters.Add(username);
                myCommand.Parameters.Add(password);

                myCommand.Connection.Open();
                SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

                if (myReader.Read() == true)
                {
                    if(textBox1.Text == "admin" && textBox2.Text == "admin")
                    {
                        MessageBox.Show("Admin Login Successfull!", "Login Success");
                        MainAdmin_Form admin = new MainAdmin_Form();
                        admin.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Login Successfull!", "Login Success");
                        MainUser_Form user = new MainUser_Form();
                        user.Show();
                        this.Hide();
                    }
                }
                else
                {
                    if (textBox1.Text == "" && textBox2.Text == "")
                        MessageBox.Show("Username and Password cannot be empty!", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else if (textBox1.Text == "")
                        MessageBox.Show("Username cannot be empty!", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else if (textBox2.Text == "")
                        MessageBox.Show("Password cannot be empty!", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);                   
                    else if(textBox1.Text != username.ToString())
                        MessageBox.Show("Username or Password not found!", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Login failed! Try again!", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox1.Focus();
                }

                if (myConnection.State == ConnectionState.Open)
                    myConnection.Dispose();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_SignUpClick(object sender, EventArgs e)
        {
            try
            {
                if(textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "")
                    MessageBox.Show("You must fill all of the fields!", "Register Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if(!textBox4.Text.Contains("@"))
                    MessageBox.Show("Email must have \"@\" symbol!", "Register Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (textBox5.Text != textBox6.Text)
                    MessageBox.Show("Passwords don't match!", "Register Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (textBox3.Text.Length > 30)
                    MessageBox.Show("Username can't have more than 30 characters!", "Register Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (textBox4.Text.Length > 50)
                    MessageBox.Show("Email can't have more than 50 characters!", "Register Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (textBox5.Text.Length > 256)
                    MessageBox.Show("Password can't have more than 256 characters!", "Register Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    myConnection = new SqlConnection(cs);
                    myCommand = new SqlCommand("INSERT INTO Accounts VALUES('" + textBox3.Text + "','" + textBox5.Text + "','" + textBox4.Text + "')", myConnection);
                    myConnection.Open();
                    myCommand.Parameters.AddWithValue("@username", textBox3.Text);
                    myCommand.Parameters.AddWithValue("@password", textBox5.Text);
                    myCommand.Parameters.AddWithValue("@email", textBox4.Text);

                    myCommand.ExecuteNonQuery();
                    myConnection.Close();

                    MessageBox.Show("Account added successfully!");

                    if (myConnection.State == ConnectionState.Open)
                        myConnection.Dispose();
                }
            }
            catch
            {
                MessageBox.Show("Username is already taken!", "Register Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
