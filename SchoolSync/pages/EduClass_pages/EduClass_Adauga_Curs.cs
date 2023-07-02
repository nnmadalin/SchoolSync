using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SchoolSync.pages.EduClass_pages
{
    public partial class EduClass_Adauga_Curs : UserControl
    {
        public EduClass_Adauga_Curs()
        {
            InitializeComponent();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            navbar_home.page = "EduClass";
            navbar_home.use = false;
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            string code = guna2TextBox1.Text.Trim();
            string token_app = schoolsync.token;

            if (code != "")
            {
                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                var data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from educlass where token = ? and is_visible = 1 and admins not like ? and pending not like ? and students not like ?");

                var param = new Dictionary<string, string>()
                {
                    {"token", code},
                    {"admins", "%" + Convert.ToString(login_signin.login.accounts_user["token"]) + "%"},
                    {"pending", "%" + Convert.ToString(login_signin.login.accounts_user["token"]) + "%"},
                    {"students", "%" + Convert.ToString(login_signin.login.accounts_user["token"]) + "%"},
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                dynamic task = await _class.PostRequestAsync(url, data);

                if(task["message"] == "success")
                {
                    string students = task["0"]["students"];

                    students += (Convert.ToString(login_signin.login.accounts_user["token"]) + ";");

                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", token_app);
                    data.Add("command", "update educlass set students = ? where token = ?");

                    param = new Dictionary<string, string>()
                    {
                        {"students", students},
                        {"token", code},
                    };

                    data.Add("params", JsonConvert.SerializeObject(param));

                    task = await _class.PostRequestAsync(url, data);

                    if (task["message"] == "update success")
                    {

                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.success.message = "Te-ai inscris la curs cu succes!";
                        frm.BringToFront();

                        navbar_home.page = "EduClass";
                        navbar_home.use = false;
                    }
                    else
                    {
                        var frm = new notification.error();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.error.message = "Ceva nu a mers bine, mai incearca!";
                        frm.BringToFront();
                    }
                }
                else
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Nu am gasit curs!";
                    frm.BringToFront();
                }
            }

        }
    }
}
