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
    public partial class EduClass_Creaza : UserControl
    {
        public EduClass_Creaza()
        {
            InitializeComponent();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {

            guna2MessageDialog1.Caption = "Inchide";
            guna2MessageDialog1.Text = "Esti sigur ca vrei sa inchizi pagina?";

            if(guna2MessageDialog1.Show() == DialogResult.Yes)
            {
                navbar_home.page = "EduClass";
                navbar_home.use = false;
            }

        }

        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            if(!(guna2TextBox1.Text.Trim() != ""  && guna2TextBox2.Text.Trim() != ""))
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Completeaza toate casetele!";
                frm.BringToFront();
            }
            else
            {
                schoolsync.show_loading();

                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/post.php";
                var data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "insert into educlass(token, token_user, created, color, title, description, admins, materials) values (?, ?, ?, ?, ?, ? ,?, ?)");

                string token = _class.generate_token();

                Random random = new Random();
                string randomColor = random.Next(256).ToString() + ", " + random.Next(256).ToString() + ", " + random.Next(256).ToString();

                var param = new Dictionary<string, string>()
                {
                    {"token", token},
                    {"token_user", Convert.ToString(login_signin.login.accounts_user["token"])},
                    {"created", Convert.ToString(login_signin.login.accounts_user["username"])},
                    {"color", randomColor.ToString()},
                    {"title", guna2TextBox1.Text.Trim()},
                    {"description", guna2TextBox2.Text.Trim()},
                    {"admins", Convert.ToString(login_signin.login.accounts_user["token"]) + ";"},
                    {"materials", "{}"},
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                dynamic task = await _class.PostRequestAsync(url, data);

                schoolsync.hide_loading();

                if (task["message"] == "insert success")
                {
                    var frm = new notification.success();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.success.message = "Curs creat cu succes!";
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
                    notification.error.message = "Ceva nu a mers bine";
                    frm.BringToFront();
                }
            }
        }
    }
}
