using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheCoachingCenter.Forms;

namespace TheCoachingCenter
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void admissionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StudentAdmission admission = new StudentAdmission();
            admission.Show();
        }

        private void showAttendanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Attendance attendance = new Attendance();
            attendance.Show();
        }

        private void feeSubmissionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FeeSubmission submission = new FeeSubmission();
            submission.Show();
        }
    }
}
