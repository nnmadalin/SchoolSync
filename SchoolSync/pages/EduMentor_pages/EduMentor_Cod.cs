using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace SchoolSync.pages.EduMentor_pages
{
    public partial class EduMentor_Cod : UserControl
    {
        public EduMentor_Cod()
        {
            InitializeComponent();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            multiple_class _class = new multiple_class();

            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from edumentor where token = ?");
            var param = new Dictionary<string, string>()
                {
                    {"token", guna2TextBox1.Text}
                };

            data.Add("params", JsonConvert.SerializeObject(param));
            dynamic task = null;
            task = await _class.PostRequestAsync(url, data);
            if(task["message"] == "success")
            {
                navbar_home.token_page = task["0"]["token"];
                navbar_home.page = "EduMentor_vizualizare";
                navbar_home.use = false;
            }
            else
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Nu am gasit material!";
                frm.BringToFront();
            }
        }
    }
}
