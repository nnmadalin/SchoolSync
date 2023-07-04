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
    public partial class EduClass_Vizualizare : UserControl
    {
        public EduClass_Vizualizare()
        {
            InitializeComponent();
        }

        private async void EduClass_Vizualizare_Load(object sender, EventArgs e)
        {
            schoolsync.show_loading();

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from educlass where token = ?");

            var param = new Dictionary<string, string>()
            {
                {"token", navbar_home.token_page},       
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);
            
            schoolsync.hide_loading();

            if (task["message"] == "success")
            {
                string[] components = Convert.ToString(task["0"]["color"]).Split(',');
                int red = int.Parse(components[0]);
                int green = int.Parse(components[1]);
                int blue = int.Parse(components[2]);
                guna2Panel1.FillColor = Color.FromArgb(red, green, blue);

                label1.Text = task["0"]["title"];

                label2.Text = "Creat de: " + task["0"]["created"];

                label3.Text = "Ultima modificare: " + task["0"]["last_edit"];

                string[] admins = Convert.ToString(task["0"]["admins"]).Split(';');
                bool is_admin = false;

                for(int i = 0; i < admins.Length - 1; i++)
                {
                    if(admins[i] == Convert.ToString(login_signin.login.accounts_user["token"]))
                    {
                        is_admin = true;
                        break;
                    }
                }

                if(is_admin == false)
                {
                    guna2Button1.Visible = false;
                    guna2CircleButton2.Visible = guna2CircleButton3.Visible = guna2CircleButton4.Visible = false;
                }

            }
            else
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Ceva nu a mers bine!";
                frm.BringToFront();

                navbar_home.page = "EduClass";
                navbar_home.use = false;
            }
            
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            navbar_home.page = "EduClass";
            navbar_home.use = false;
        }

        private void guna2CircleButton5_Click(object sender, EventArgs e)
        {
            navbar_home.page = "EduClass_vizualizare_persoane";
            navbar_home.use = false;
        }

        private async void guna2CircleButton4_Click(object sender, EventArgs e)
        {
            guna2MessageDialog1.Caption = "Sterge";
            guna2MessageDialog1.Text = "Esti sigur ca vrei sa stergi acest curs?";

            if(guna2MessageDialog1.Show() == DialogResult.Yes)
            {
                schoolsync.show_loading();

                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/delete.php";
                var data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "delete from educlass where token = ?");

                var param = new Dictionary<string, string>()
                {
                    {"token", navbar_home.token_page},
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                dynamic task = await _class.PostRequestAsync(url, data);

                schoolsync.hide_loading();

                if(task["message"] == "delete success")
                {
                    var frm = new notification.success();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.success.message = "Curs sters cu succes";
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
                    notification.error.message = "Ceva nu a mers bine!";
                    frm.BringToFront();
                }
            }
        }

        private void guna2CircleButton6_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(navbar_home.token_page);

            var frm = new notification.success();
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
            panel.Controls.Add(frm);
            notification.success.message = "Cod curs copiat cu succes!";
            frm.BringToFront();
        }

        private async void guna2CircleButton7_Click(object sender, EventArgs e)
        {
            schoolsync.show_loading();

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from educlass where token = ?");

            var param = new Dictionary<string, string>()
            {
                {"token", navbar_home.token_page},
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);

            if(task["message"] == "success")
            {
                bool is_admin = false;

                string[] split = Convert.ToString(task["0"]["admins"]).Split(';');

                for(int i = 0; i < split.Length - 1; i++)
                {
                    if(split[i] == Convert.ToString(login_signin.login.accounts_user["token"]))
                    {
                        is_admin = true;
                        break;
                    }
                }

               
                if(is_admin == true && split.Length - 1 == 1)
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Nu poti parasi cursul deoarece este doar un admin!";
                    frm.BringToFront();

                    schoolsync.show_loading();
                }
                else
                {
                    string newadmin = "";

                    for (int i = 0; i < split.Length - 1; i++)
                    {
                        if (split[i] != Convert.ToString(login_signin.login.accounts_user["token"]))
                        {
                            newadmin += split[i] + ";";
                        }
                    }

                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "update educlass set admins = ? where token = ?");

                    param = new Dictionary<string, string>()
                    {
                        {"admins", newadmin},
                        {"token", navbar_home.token_page},
                    };

                    data.Add("params", JsonConvert.SerializeObject(param));
                    
                    task = await _class.PostRequestAsync(url, data);

                    schoolsync.hide_loading();

                    if (task["message"] == "update success")
                    {
                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.success.message = "Ai parasit cursul!";
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
                        notification.error.message = "Ceva nu a mers bine!";
                        frm.BringToFront();
                    }
                    
                }

            }

            schoolsync.hide_loading();
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            navbar_home.page = "EduClass_editare";
            navbar_home.use = false;
        }

        private async void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            schoolsync.show_loading();

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from educlass where token = ?");

            var param = new Dictionary<string, string>()
            {
                {"token", navbar_home.token_page},
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);

            if (task["message"] == "success")
            {
                string status = task["0"]["is_visible"];

                url = "https://schoolsync.nnmadalin.me/api/put.php";
                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);

                if (status == "0")
                {
                    data.Add("command", "update educlass set is_visible = '1' where token = ?");
                }
                else
                {
                    data.Add("command", "update educlass set is_visible = '0' where token = ?");
                }

                param = new Dictionary<string, string>()
                {
                    {"token", navbar_home.token_page},
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                task = await _class.PostRequestAsync(url, data);

                schoolsync.hide_loading();


                if (task["message"] == "update success")
                {
                    if(status == "0")
                    {
                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.success.message = "Curs este acum: vizibil!";
                        frm.BringToFront();
                    }
                    else
                    {
                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.success.message = "Curs este acum: invizibil!";
                        frm.BringToFront();
                    }
                }
                else
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Ceva nu a mers bine!";
                    frm.BringToFront();
                }
            }
            else
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Ceva nu a mers bine!";
                frm.BringToFront();
            }

            schoolsync.hide_loading();
        }
    }
}
