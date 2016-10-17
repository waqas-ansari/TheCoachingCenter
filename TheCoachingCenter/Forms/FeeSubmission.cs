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

        public FeeSubmission()
        {
            InitializeComponent();

            string filename = Application.StartupPath.Substring(0, Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf("\\")).LastIndexOf("\\")) + @"\FeeVoucher.rpt";

            ReportDocument report = new ReportDocument();
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

            string dateText = DateTime.Today.ToString("EEE, MMM dd, yyyy");

            txtDate.Text = dateText;
        }

        private void FeeSubmission_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT Name, Class, Section, [Group], MonthlyFee FROM StudentDetail WHERE Id = " + txtID.Text;
                command.Connection = connection;
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {

                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
        }
    }
}
