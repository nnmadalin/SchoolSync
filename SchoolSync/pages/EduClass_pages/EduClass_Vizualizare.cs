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
using Newtonsoft.Json.Linq;

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

                if(is_admin == true)
                {
                    guna2Button1.Visible = true;
                    guna2CircleButton2.Visible = guna2CircleButton3.Visible = guna2CircleButton4.Visible = true;
                }

                //descriere

                if (Convert.ToString(task["0"]["description"]).Trim() != "")
                {

                    Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        FillColor = Color.FromArgb(223, 229, 232),
                        AutoSize = true,
                        MinimumSize = new Size(1130, 0),
                        MaximumSize = new Size(1130, 0),
                        Margin = new Padding(0, 0, 0, 30),
                        BorderRadius = 20,
                        UseTransparentBackground = true,
                    };
                    Label lbl = new Label()
                    {
                        AutoSize = true,
                        MinimumSize = new Size(1110, 0),
                        MaximumSize = new Size(1110, 0),
                        Location = new Point(10, 10),
                        Margin = new Padding(0, 0, 0, 10),
                        BackColor = Color.Transparent,
                        Font = new Font("Segoe UI", 12),
                    };
                    lbl.Text = task["0"]["description"];
                    pnl.Controls.Add(lbl);
                    flowLayoutPanel1.Controls.Add(pnl);
                }

                dynamic subjson = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["materials"]));

                JObject json = subjson;

                for(int i = json.Count; i >= 0; i--)
                {
                    try
                    {
                        if (Convert.ToString(subjson[i.ToString()]["is_deleted"]) == "0")
                        {
                            if ((is_admin == false && subjson[i.ToString()]["is_visible"] == "1") || is_admin == true)
                            {
                                Guna.UI2.WinForms.Guna2Panel pnl_lectie = new Guna.UI2.WinForms.Guna2Panel()
                                {
                                    FillColor = Color.FromArgb(223, 229, 232),
                                    Size = new Size(1130, 100),
                                    BorderRadius = 10,
                                    UseTransparentBackground = true,
                                    Margin = new Padding(0, 0, 0, 30),
                                    Padding = new Padding(30, 15, 30, 15),
                                    Cursor = Cursors.Hand,
                                };
                                pnl_lectie.Tag = i.ToString();
                                pnl_lectie.Click += click_lectie;

                                Label lbl_titlu = new Label()
                                {
                                    Size = new Size(1110, 40),
                                    Location = new Point(10, 10),
                                    BackColor = Color.Transparent,
                                    TextAlign = ContentAlignment.MiddleCenter,
                                    Font = new Font("Segoe UI", 17, FontStyle.Bold),
                                    Cursor = Cursors.Hand,
                                };
                                lbl_titlu.Tag = i.ToString();
                                lbl_titlu.Click += click_lectie;

                                Label lbl_descriere = new Label()
                                {
                                    Size = new Size(1110, 40),
                                    Location = new Point(10, 50),
                                    BackColor = Color.Transparent,
                                    TextAlign = ContentAlignment.MiddleCenter,
                                    Font = new Font("Segoe UI", 12),
                                    Cursor = Cursors.Hand,
                                };
                                lbl_descriere.Tag = i.ToString();
                                lbl_descriere.Click += click_lectie;

                                lbl_titlu.Text = Convert.ToString(subjson[i.ToString()]["created"]) + " a postat o lectie noua: " + Convert.ToString(subjson[i.ToString()]["title"]);
                                lbl_descriere.Text = "Ultima modificare: " + Convert.ToString(subjson[i.ToString()]["last_edit"]);

                                pnl_lectie.Controls.Add(lbl_titlu);
                                pnl_lectie.Controls.Add(lbl_descriere);

                                flowLayoutPanel1.Controls.Add(pnl_lectie);
                            }

                        }
                    }
                    catch { };
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

        
        private void click_lectie(object sender, EventArgs e)
        {
            navbar_home.token_page_2 = ((Control)sender).Tag.ToString();
            navbar_home.page = "EduClass_vizualizare_lectie";
            navbar_home.use = false;
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
                else if(is_admin == true)
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
                else
                {
                    split = Convert.ToString(task["0"]["students"]).Split(';');
                    string newstd= "";

                    for (int i = 0; i < split.Length - 1; i++)
                    {
                        if (split[i] != Convert.ToString(login_signin.login.accounts_user["token"]))
                        {
                            newstd += split[i] + ";";
                        }
                    }

                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "update educlass set students = ? where token = ?");

                    param = new Dictionary<string, string>()
                    {
                        {"admins", newstd},
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            navbar_home.page = "EduClass_creaza_lectie";
            navbar_home.use = false;
        }
    }
}
