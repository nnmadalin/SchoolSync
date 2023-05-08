using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolSync
{
    public partial class schoolsync : Form
    {
        public schoolsync()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var frm = new login_signin.login();
            guna2Panel2.Controls.Add(frm);
            frm.Show();
            frm.Location = new Point(0, 0);
            GC.Collect();
        }


        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void schoolsync_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
