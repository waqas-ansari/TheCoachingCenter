﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheCoachingCenter
{
    public partial class StudentAdmission : Form
    {
        string connectionString = "Data Source=LINTA\\SQLEXPRESS;Initial Catalog=DB_TheCoachingCenter;Integrated Security=True";
        public StudentAdmission()
        {
            InitializeComponent();
        }


        private void StudentAdmission_Load(object sender, EventArgs e)
        {
            List<string> classList, groupList;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT DISTINCT Class FROM SubjectClassWise";
                command.Connection = connection;
                connection.Open();
                SqlDataReader sqlReader = command.ExecuteReader();
                classList = new List<string>();
                groupList = new List<string>();
                try
                {
                    while (sqlReader.Read())
                    {
                        classList.Add(String.Format("{0}", sqlReader["Class"]));
                    }
                    sqlReader.Close();
                    command.CommandText = "SELECT DISTINCT [Group] FROM SubjectClassWise";
                    sqlReader = command.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        groupList.Add(String.Format("{0}", sqlReader["Group"]));
                    }

                    sqlReader.Close();

                    command.CommandText = "SELECT COUNT(Id) FROM StudentDetail";
                    Int32 autoGeneratedId = (Int32)command.ExecuteScalar();
                    displayID(autoGeneratedId + 1);
                }
                finally
                {
                    sqlReader.Close();
                }
            }
            string[] classArray = classList.ToArray();
            cmbClass.Items.AddRange(classArray);

            string[] groupArray = groupList.ToArray();
            cmbGroup.Items.AddRange(groupArray);

            string[] secArray = { "A", "B", "C" };
            cmbSection.Items.AddRange(secArray);

        }

        private void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSection.Items.Clear();
            cmbGroup.Items.Clear();
            


            string[] secArray;
            if(cmbClass.Text.Equals("IX")) 
            {
                secArray = new string[2];
                secArray[0] = "A";
                secArray[1] = "B";
            } else {
                secArray = new string[3];
                secArray[0] = "A";
                secArray[1] = "B";
                secArray[2] = "C";
            }
            cmbSection.Items.AddRange(secArray);
            cmbSection.SelectedIndex = 0;

            List<string> groupList, subjectList; ;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT DISTINCT [Group] FROM SubjectClassWise WHERE Class = '" + cmbClass.Text.TrimEnd(' ') + "'";

                command.Connection = connection;
                connection.Open();
                SqlDataReader sqlReader = command.ExecuteReader();
                groupList = new List<string>();
                subjectList = new List<string>();
                try
                {
                    while (sqlReader.Read())
                    {
                        groupList.Add(String.Format("{0}", sqlReader["Group"]));
                    }
                    sqlReader.Close();
                    string[] groupArray = groupList.ToArray();
                    cmbGroup.Items.AddRange(groupArray);
                    cmbGroup.SelectedIndex = 0;

                    command.CommandText = "SELECT Subject FROM SubjectClassWise WHERE Class = '" + cmbClass.Text.TrimEnd(' ') + "' AND [Group] LIKE '" + cmbGroup.Text+ "'";
                    sqlReader = command.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        subjectList.Add(String.Format("{0}", sqlReader["Subject"]));
                    }

                }
                finally
                {
                    sqlReader.Close();
                }
            }


            lstSubjects.Items.Clear();
            string[] subjectArray = subjectList.ToArray();
            lstSubjects.Items.AddRange(subjectArray);

        }

        private void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

            List<string> subjectList;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT Subject FROM SubjectClassWise WHERE Class = '" + cmbClass.Text.TrimEnd(' ') + "' AND [Group] LIKE '" + cmbGroup.Text + "'";

                command.Connection = connection;
                connection.Open();
                SqlDataReader sqlReader = command.ExecuteReader();
                subjectList = new List<string>();
                try
                {
                    while (sqlReader.Read())
                    {
                        subjectList.Add(String.Format("{0}", sqlReader["Subject"]));
                    }

                }
                finally
                {
                    sqlReader.Close();
                }
            }


            lstSubjects.Items.Clear();
            string[] subjectArray = subjectList.ToArray();
            lstSubjects.Items.AddRange(subjectArray);

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string studentDetailQuery = "INSERT INTO StudentDetail (Id, Name, FatherName, DateOfBirth, Gender, DateOfAdmission, School, Class, Section, [Group], MonthlyFee)" + 
                        "VALUES(@id, @name, @fatherName, @dateOfBirth, @gender, @admissionDate, @school, @class, @section, @group, @fee) ";
            string studentContactQuery = "INSERT INTO StudentContactDetail (StudentId, Address, StudentPhone, ParentsPhone, Email)" + 
                        "VALUES (@id1, @address, @studentPhone, @parentsPhone, @email)";
            string studentFeeDetailQuery = "INSERT INTO StudentFeeDetail (StudentId, Month, FeeAmount, LateCharges, OtherCharges, Total, FeeSubmitted)" + 
                        "VALUES (@id2, @month, @fee1, 0, @registration, @total, 'false')";

            //Student Detail Query Execution
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                string dob = dtDOB.Value.ToString("yyyy-MM-dd");
                string doa = DateTime.Today.ToString("yyyy-MM-dd");

                command.CommandText = studentDetailQuery;
                command.Parameters.AddWithValue("@id", Convert.ToInt32(lblID.Text));
                command.Parameters.AddWithValue("@name", txtName.Text);
                command.Parameters.AddWithValue("@fatherName", txtFatherName.Text);
                command.Parameters.AddWithValue("@dateOfBirth", dob);
                command.Parameters.AddWithValue("@gender", cmbGender.Text);
                command.Parameters.AddWithValue("@admissionDate", doa);
                command.Parameters.AddWithValue("@school", txtSchool.Text);
                command.Parameters.AddWithValue("@class", cmbClass.Text);
                command.Parameters.AddWithValue("@section", cmbSection.Text);
                command.Parameters.AddWithValue("@group", cmbGroup.Text);
                command.Parameters.AddWithValue("@fee", Convert.ToInt32(txtMonthlyFee.Text));
                command.ExecuteNonQuery();



                command.CommandText = studentContactQuery;
                command.Parameters.AddWithValue("@id1", Convert.ToInt32(lblID.Text));
                command.Parameters.AddWithValue("@address", txtAddress.Text);
                command.Parameters.AddWithValue("@studentPhone", txtStudentPhone.Text);
                command.Parameters.AddWithValue("@parentsPhone", txtParentPhone.Text);
                command.Parameters.AddWithValue("@email", txtEmail.Text);
                command.ExecuteNonQuery();



                DateTime date = DateTime.Today.Date;
                string month = date.ToString("MMMM");


                command.CommandText = studentFeeDetailQuery;
                command.Parameters.AddWithValue("@id2", Convert.ToInt32(lblID.Text));
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@fee1", Convert.ToInt32(txtMonthlyFee.Text));
                command.Parameters.AddWithValue("@registration", Convert.ToInt32(txtAdmissionFee.Text));
                command.Parameters.AddWithValue("@total", Convert.ToInt32(txtMonthlyFee.Text) + Convert.ToInt32(txtAdmissionFee.Text));
                command.ExecuteNonQuery();


                MessageBox.Show("Successfully added new Student\nAuto Generated ID: " + lblID.Text, "Student Added", MessageBoxButtons.OK);


            }
        }




        private void displayID(int id)
        {
            string idText;
            if (id < 10)
                idText = "000" + id.ToString();
            else if (id < 100)
                idText = "00" + id.ToString();
            else if (id < 1000)
                idText = "0" + id.ToString();
            else idText = id.ToString();

            lblID.Text = idText;
        }


    }
}
