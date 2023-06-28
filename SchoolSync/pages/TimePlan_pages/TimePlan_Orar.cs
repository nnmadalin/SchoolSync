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
    public partial class TimePlan_Orar : UserControl
    {
        public TimePlan_Orar()
        {
            InitializeComponent();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            navbar_home.page = "TimePlan_calendar";
            navbar_home.use = false;
        }

        private void guna2Button21_Click(object sender, EventArgs e)
        {
            navbar_home.page = "TimePlan_orar";
            navbar_home.use = false;
        }
    }
}
