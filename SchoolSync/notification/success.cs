﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolSync.notification
{
    public partial class success : UserControl
    {
        public success()
        {
            InitializeComponent();
        }

        int k = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = message;
            if (k == 301)
            {
                timer1.Stop();
                this.Hide();
            }
            else
            {
                guna2ProgressBar1.Value = k;
                k++;
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Hide();
        }

        public static string message = "";

        private void success_Load(object sender, EventArgs e)
        {
            
        }
    }
}
