using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SchoolSync.pages
{
    public partial class EduClass : UserControl
    {
        public EduClass()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            navbar_home.page = "EduClass_creaza";
            navbar_home.use = false;
        }
    }
}
