using System;
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
    public partial class Attendance : Form
    {
        string connectionString = "Data Source=LINTA\\SQLEXPRESS;Initial Catalog=DB_TheCoachingCenter;Integrated Security=True";
        public Attendance()
        {
            InitializeComponent();
        }

        private void Attendance_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            attendanceGrid.Width = this.Width - 40;
            attendanceGrid.Height = this.Height - groupBox1.Height - 60;

            HashSet<string> classList, groupList;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT DISTINCT Class, [Group] FROM SubjectClassWise";
                command.Connection = connection;
                connection.Open();
                SqlDataReader sqlReader = command.ExecuteReader();
                classList = new HashSet<string>();
                groupList = new HashSet<string>();
                try
                {
                    while (sqlReader.Read())
                    {
                        classList.Add(String.Format("{0}", sqlReader["Class"]));
                        groupList.Add(String.Format("{0}", sqlReader["Group"]));
                    }

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
           

            string[] secArray;
            if (cmbClass.Text.Equals("IX"))
            {
                secArray = new string[2];
                secArray[0] = "A";
                secArray[1] = "B";
            }
            else
            {
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

                }
                finally
                {
                    sqlReader.Close();
                }
            }

        }

        private void btnShowAttendance_Click(object sender, EventArgs e)
        {
            string selectAttendanceQuery = "SELECT StudentId AS ID, StudentName AS Name," +
                "[1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14]," +
                "[15], [16], [17], [18], [19], [20], [21], [22], [23], [24], [25], [26], [27], [28], [29], [30], [31] " +
                "FROM Attendance WHERE Class LIKE '" + cmbClass.Text + "' AND Section LIKE '" + cmbSection.Text + "' AND [Group] LIKE '" + cmbGroup.Text + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = selectAttendanceQuery;
                command.Connection = connection;
                connection.Open();


                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    using (DataTable dt = new DataTable())
                    {
                        adapter.Fill(dt);
                        attendanceGrid.DataSource = dt;
                    }
                }

            }

            attendanceGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

        }
    }
}
