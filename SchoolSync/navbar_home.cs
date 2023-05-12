﻿using System;
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
    }
}
