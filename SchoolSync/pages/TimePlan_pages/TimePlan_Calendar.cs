using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolSync.pages.TimePlan_pages
{
    public partial class TimePlan_Calendar : UserControl
    {
        public TimePlan_Calendar()
        {
            InitializeComponent();
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        DateTime now;

        void load_tab(DateTime time)
        {
            label1.Text = time.Day.ToString();
            if (time.DayOfWeek.ToString() == "Monday")
                label2.Text = "Luni";
            else if (time.DayOfWeek.ToString() == "Tuesday")
                label2.Text = "Marti";
            else if (time.DayOfWeek.ToString() == "Wednesday")
                label2.Text = "Miercuri";
            else if (time.DayOfWeek.ToString() == "Thursday")
                label2.Text = "Joi";
            else if (time.DayOfWeek.ToString() == "Friday")
                label2.Text = "Vineri";
            else if (time.DayOfWeek.ToString() == "Saturday")
                label2.Text = "Sambata";
            else if (time.DayOfWeek.ToString() == "Sunday")
                label2.Text = "Duminica";
            else
                label2.Text = time.DayOfWeek.ToString();
            label23.Text = time.Year.ToString();

            int luna = time.Month;
            int zi = time.Day;

            DateTime dotm = new DateTime(time.Year, time.Month, 1);
            
            int dayofweek = Convert.ToInt32(dotm.DayOfWeek.ToString("d"));
            int daymonth = DateTime.DaysInMonth(time.Year, time.Month);

            if (dayofweek == 0)
                dayofweek = 7;

            luna += 3;
            
            foreach (Control ctrl in guna2Panel1.Controls)
            {
                if (ctrl is Guna.UI2.WinForms.Guna2CircleButton)
                {
                    ((Guna.UI2.WinForms.Guna2CircleButton)ctrl).FillColor = Color.Gainsboro;
                    ((Guna.UI2.WinForms.Guna2CircleButton)ctrl).ForeColor = Color.Black;
                }
            }

            foreach (Control ctrl in guna2Panel1.Controls)
            {
                if (ctrl is Label)
                {
                    ctrl.ForeColor = Color.FromArgb(32, 33, 36);
                }
            }

            foreach (Control ctrl in guna2Panel1.Controls)
            {
                if (ctrl is Label && ctrl.Name.ToString() == "label" + luna.ToString())
                {
                    ctrl.ForeColor = Color.FromArgb(46, 204, 113);
                }
            }
            foreach (Control ctrl in guna2Panel1.Controls)
            {
                if (ctrl is Guna.UI2.WinForms.Guna2CircleButton)
                {
                    ctrl.Visible = false;
                }
            }

            for(int i = 1; i <= daymonth; i++)
            {
                foreach (Control ctrl in guna2Panel1.Controls)
                {
                    if (ctrl is Guna.UI2.WinForms.Guna2CircleButton && ctrl.Name.ToString() == "guna2CircleButton" + (i + dayofweek).ToString())
                    {
                        ctrl.Visible = true;
                        ctrl.Text = i.ToString();
                    }
                }
            }

            
            foreach (Control ctrl in guna2Panel1.Controls)
            {
                if (ctrl is Guna.UI2.WinForms.Guna2CircleButton && ctrl.Name.ToString() == "guna2CircleButton" + (zi + dayofweek).ToString())
                {
                    ((Guna.UI2.WinForms.Guna2CircleButton)ctrl).FillColor = Color.FromArgb(46, 204, 113);
                    ctrl.ForeColor = Color.White;
                }
            }
            
        }

        private void TimePlan_Calendar_Load(object sender, EventArgs e)
        {
            load_tab(DateTime.Now);
            now = DateTime.Now;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in guna2Panel1.Controls)
            {
                if (ctrl is Label)
                {
                    ctrl.ForeColor = Color.FromArgb(32, 33, 36);
                }
            }

            ((Label)sender).ForeColor = Color.FromArgb(46, 204, 113);
            try
            {
                now = new DateTime(now.Year, Convert.ToInt32(((Label)sender).Tag.ToString()), now.Day);
            }
            catch
            {
                now = new DateTime(now.Year, Convert.ToInt32(((Label)sender).Tag.ToString()), DateTime.DaysInMonth(now.Year, Convert.ToInt32(((Label)sender).Tag.ToString())));
            }
            load_tab(now);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            now = now.AddYears(1);
            load_tab(now);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            now = now.AddYears(-1);
            load_tab(now);
        }

        private void guna2CircleButton43_Click(object sender, EventArgs e)
        {

            now = new DateTime(now.Year, now.Month, Convert.ToInt32(((Guna.UI2.WinForms.Guna2CircleButton)sender).Text));
            load_tab(now);
        }
    }
}
