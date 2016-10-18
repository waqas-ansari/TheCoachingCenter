using CrystalDecisions.CrystalReports.Engine;
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

namespace TheCoachingCenter.Forms
{
    public partial class FeeSubmission : Form
    {
        string connectionString = "Data Source=LINTA\\SQLEXPRESS;Initial Catalog=DB_TheCoachingCenter;Integrated Security=True";
        TextObject txtDate, txtSID, txtName, txtClass, txtSection, txtGroup, txtFee, txtLateFee, txtOther, txtTotal;

        ReportDocument report;

        public FeeSubmission()
        {
            InitializeComponent();

            string filename = Application.StartupPath.Substring(0, Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf("\\")).LastIndexOf("\\")) + @"\FeeVoucher.rpt";

            report = new ReportDocument();
            report.Load(filename);

            txtDate = (TextObject)report.ReportDefinition.Sections["Section2"].ReportObjects["txtDate"];
            txtSID = (TextObject)report.ReportDefinition.Sections["Section2"].ReportObjects["txtID"];
            txtName = (TextObject)report.ReportDefinition.Sections["Section2"].ReportObjects["txtName"];
            txtClass = (TextObject)report.ReportDefinition.Sections["Section2"].ReportObjects["txtClass"];
            txtSection = (TextObject)report.ReportDefinition.Sections["Section2"].ReportObjects["txtSection"];
            txtGroup = (TextObject)report.ReportDefinition.Sections["Section2"].ReportObjects["txtGroup"];
            txtFee = (TextObject)report.ReportDefinition.Sections["Section2"].ReportObjects["txtFee"];
            txtLateFee = (TextObject)report.ReportDefinition.Sections["Section2"].ReportObjects["txtLateFee"];
            txtOther = (TextObject)report.ReportDefinition.Sections["Section2"].ReportObjects["txtOther"];
            txtTotal = (TextObject)report.ReportDefinition.Sections["Section2"].ReportObjects["txtTotalCharges"];

            string dateText = DateTime.Today.ToString("ddd, MMM dd, yyyy");

            txtDate.Text = dateText;


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT FeeSubmitted, Total FROM StudentFeeDetail WHERE Month LIKE '" + DateTime.Today.ToString("MMMM") + "'";
                command.Connection = connection;
                connection.Open();

                int totalCash = 0;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string text = String.Format("{0}", reader["FeeSubmitted"]);
                    if (text.CompareTo("true") == 1)
                        totalCash += (Int32)reader["Total"];
                }

                lblTotalCash.Text = "Rs. " + totalCash.ToString() + "/-";
            }

            //updateTotalCharges();

            
        }

        private void FeeSubmission_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string studentDetailQuery = "SELECT Name, Class, Section, [Group], MonthlyFee FROM StudentDetail WHERE Id = " + txtID.Text;
                string feeDetailQuery = "SELECT * FROM StudentFeeDetail WHERE StudentId = " + txtID.Text + " AND Month LIKE '" + DateTime.Today.ToString("MMMM") + "'";

                SqlCommand command = new SqlCommand();
                command.CommandText = feeDetailQuery;
                command.Connection = connection;
                connection.Open();

                int count = 0;
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {

                        string text = String.Format("{0}", reader["FeeSubmitted"]);
                        if (text.CompareTo("true") == 1)
                        {
                            MessageBox.Show("Fee Already Submitted.");
                            return;
                        }

                        count++;
                        txtFee.Text = "Rs. " + String.Format("{0}", reader["FeeAmount"]);
                        txtLateFee.Text = "Rs. " + String.Format("{0}", reader["LateCharges"]);
                        txtOther.Text = "Rs. " + String.Format("{0}", reader["OtherCharges"]);
                        txtTotal.Text = "Rs. " + String.Format("{0}", reader["Total"]) + "/-";

                        txtMonthlyFee.Text = String.Format("{0}", reader["FeeAmount"]);
                        txtLateCharges.Text = String.Format("{0}", reader["LateCharges"]);
                        txtOtherCharges.Text = String.Format("{0}", reader["OtherCharges"]);
                        txtTotalCharges.Text = String.Format("{0}", reader["Total"]);

                    }

                    reader.Close();

                    command.CommandText = studentDetailQuery;
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        txtName.Text = String.Format("{0}", reader["Name"]);
                        txtClass.Text = String.Format("{0}", reader["Class"]);
                        txtGroup.Text = String.Format("{0}", reader["Group"]);
                    }

                    if (count == 0)
                    {
                        MessageBox.Show("Wrong ID provided.");
                        return;
                    }
                    
                }
                catch (Exception)
                {
                    
                    throw;
                }

            }

            feeReport.ReportSource = report;
            feeReport.Refresh();
        }

        private void btnSubmit_Click_1(object sender, EventArgs e)
        {
            string updateQuery = "UPDATE StudentFeeDetail SET FeeSubmitted = 'true' WHERE StudentId = " + txtID.Text + " AND Month LIKE '" + DateTime.Today.ToString("MMMM") + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = updateQuery;
                command.Connection = connection;
                connection.Open();

                command.ExecuteNonQuery();

                //updateTotalCharges();
            }

            report.PrintToPrinter(1, false, 1, 1);
            btnSubmit.Enabled = false;


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT FeeSubmitted, Total FROM StudentFeeDetail WHERE Month LIKE '" + DateTime.Today.ToString("MMMM") + "'";
                command.Connection = connection;
                connection.Open();

                int totalCash = 0;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["FeeSubmitted"].Equals("true"))
                        totalCash += (Int32)reader["Total"];
                }

                lblTotalCash.Text = "Rs. " + totalCash.ToString() + "/-";
            }

        }

        private void updateTotalCharges()
        {

        }
    }
}
