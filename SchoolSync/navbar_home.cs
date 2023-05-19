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
    public partial class navbar_home : UserControl
    {
        public navbar_home()
        {
            InitializeComponent();
        }

        private void home_Load(object sender, EventArgs e)
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
            var label = panel.Controls["label1"];
            label.Text = "SchoolSync | Acasa";
            GC.Collect();

            var frm = new pages.home();
            guna2Panel2.Controls.Add(frm);
            frm.Show();
        }

        void background_color_btn()
        {
            guna2Button1.FillColor = Color.Transparent;
            guna2Button2.FillColor = Color.Transparent;
            guna2Button3.FillColor = Color.Transparent;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
            var label = panel.Controls["label1"];
            label.Text = "SchoolSync | Acasa";
            GC.Collect();
            
            var frm = new pages.home();
            guna2Panel2.Controls.Clear();
            guna2Panel2.Controls.Add(frm);
            frm.Show();

            background_color_btn();
            guna2Button1.FillColor = Color.FromArgb(66, 66, 66);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
            var label = panel.Controls["label1"];
            label.Text = "SchoolSync | EduMentor";
            GC.Collect();

            var frm = new pages.EduMentor();
            guna2Panel2.Controls.Clear();
            guna2Panel2.Controls.Add(frm);
            frm.Show();

            background_color_btn();
            guna2Button2.FillColor = Color.FromArgb(66, 66, 66);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
            var label = panel.Controls["label1"];
            label.Text = "SchoolSync | ÎnvațăUnit";
            GC.Collect();

            var frm = new pages.InvataUnit();
            guna2Panel2.Controls.Clear();
            guna2Panel2.Controls.Add(frm);
            frm.Show();

            background_color_btn();
            guna2Button3.FillColor = Color.FromArgb(66, 66, 66);
        }
    }
}
