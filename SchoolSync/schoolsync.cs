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

        public static void show_loading()
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var pnl = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
            pnl.Enabled = false;

            Guna.UI2.WinForms.Guna2WinProgressIndicator loading = new Guna.UI2.WinForms.Guna2WinProgressIndicator()
            {
                Size = new Size(90, 90),
                UseTransparentBackground = true,
                AutoStart = true,
                Location = new Point((1340 - 90) / 2, (690 - 90) / 2),
                Name = "panel_loading"
            };
            pnl.Controls.Add(loading);
            loading.Show();
            loading.BringToFront();
        }

        public static void hide_loading()
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var pnl = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
            pnl.Controls.Remove(pnl.Controls["panel_loading"]);
            pnl.Enabled = true;
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
