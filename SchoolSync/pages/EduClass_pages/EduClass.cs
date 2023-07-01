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

        private void curs_Click(object sender, EventArgs e)
        {

        }

        private async void pending_decline(object sender, EventArgs e)
        {
            string token = ((Control)sender).Tag.ToString();

            if (guna2MessageDialog1.Show() == DialogResult.Yes)
            {
                schoolsync.show_loading();

                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                var data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from educlass where token = ?");

                var param = new Dictionary<string, string>()
                {
                    {"token", token},
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                dynamic task = await _class.PostRequestAsync(url, data);

                if (task["message"] == "success")
                {
                    string pending = task["0"]["pending"];
                    string[] split = pending.Split(';');
                    string newpending = "";

                    for (int i = 0; i < split.Length - 1; i++)
                    {
                        if (split[i] != Convert.ToString(login_signin.login.accounts_user["token"]))
                        {
                            newpending += split[i] + ";";
                        }
                    }

                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "update educlass set pending = ? where token = ?");

                    param = new Dictionary<string, string>()
                    {
                        {"pending", newpending},
                        {"token", token},
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
                        notification.success.message = "Cerere stearsa!";
                        frm.BringToFront();

                        flowLayoutPanel1.Controls.Clear();

                        navbar_home.page = "EduClass";
                        navbar_home.use = false;
                    }
                    else
                    {
                        var frm = new notification.error();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.error.message = "Ceva nu a mers bine :( !";
                        frm.BringToFront();
                    }
                }
                schoolsync.hide_loading();
            }
        }

        private async void pending_accept(object sender, EventArgs e)
        {
            string token = ((Control)sender).Tag.ToString();

            schoolsync.show_loading();

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from educlass where token = ?");

            var param = new Dictionary<string, string>()
                {
                    {"token", token},
                };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);

            if (task["message"] == "success")
            {
                string pending = task["0"]["pending"];

                string[] split = pending.Split(';');
                string newpending = "";

                for (int i = 0; i < split.Length - 1; i++)
                {
                    if (split[i] != Convert.ToString(login_signin.login.accounts_user["token"]))
                    {
                        newpending += split[i] + ";";
                    }
                }

                string students = task["0"]["students"];
                students += Convert.ToString(login_signin.login.accounts_user["token"]) + ";";

                url = "https://schoolsync.nnmadalin.me/api/put.php";
                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "update educlass set pending = ?, students = ? where token = ?");

                param = new Dictionary<string, string>()
                    {
                        {"pending", newpending},
                        {"students", students},
                        {"token", token},
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
                    notification.success.message = "Cerere acceptata!";
                    frm.BringToFront();

                    flowLayoutPanel1.Controls.Clear();

                    navbar_home.page = "EduClass";
                    navbar_home.use = false;
                }
                else
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Ceva nu a mers bine :( !";
                    frm.BringToFront();
                }
            }
            schoolsync.hide_loading();
        }

        private async void EduClass_Load(object sender, EventArgs e)
        {
            //incarca din db daca esti in asteptare

            schoolsync.show_loading();

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from educlass where pending like ?");

            var param = new Dictionary<string, string>()
                {
                    {"pending", "%" + Convert.ToString(login_signin.login.accounts_user["token"]) + "%"},
                };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);
            if(task["message"] == "success")
            {
                JObject json = task;
                for(int i = 0; i < json.Count - 1; i++)
                {
                    Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        Size = new Size(354, 200),
                        FillColor = Color.FromArgb(223, 229, 232),
                        UseTransparentBackground = true,
                        BorderRadius = 20,
                        Margin = new Padding(3, 3, 20, 30),
                        Cursor = Cursors.Hand,
                    };
                    pnl.Tag = task[i.ToString()]["token"];
                    pnl.Click += curs_Click;

                    Guna.UI2.WinForms.Guna2Panel pnl_color = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        Size = new Size(120, 120),
                        Location = new Point(13, 43),
                        UseTransparentBackground = true,
                        BorderRadius = 60,
                        Cursor = Cursors.Hand,
                    };
                    string[] components = Convert.ToString(task[i.ToString()]["color"]).Split(',');
                    int red = int.Parse(components[0]);
                    int green = int.Parse(components[1]);
                    int blue = int.Parse(components[2]);
                    pnl_color.FillColor = Color.FromArgb(red, green, blue);
                    pnl_color.Tag = task[i.ToString()]["token"];
                    pnl_color.Click += curs_Click;

                    Label lbl = new Label()
                    {
                        AutoSize = false,
                        AutoEllipsis = true,
                        Size = new Size(212, 177),
                        Location = new Point(139, 12),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold),
                        Cursor = Cursors.Hand,
                    };
                    lbl.Text = task[i.ToString()]["title"];
                    lbl.Tag = task[i.ToString()]["token"];
                    lbl.Click += curs_Click;

                    Guna.UI2.WinForms.Guna2CircleButton gcb_close = new Guna.UI2.WinForms.Guna2CircleButton()
                    {
                        Size = new Size(35, 35),
                        Location = new Point(13, 3),
                        FillColor = Color.White,
                        UseTransparentBackground = true,
                        Image = SchoolSync.Properties.Resources.close_FILL1_wght700_GRAD0_opsz48,
                        Cursor = Cursors.Hand,
                    };
                    gcb_close.Tag = task[i.ToString()]["token"];
                    gcb_close.Click += pending_decline;

                    Guna.UI2.WinForms.Guna2CircleButton gcb_accept = new Guna.UI2.WinForms.Guna2CircleButton()
                    {
                        Size = new Size(35, 35),
                        Location = new Point(98, 3),
                        FillColor = Color.White,
                        UseTransparentBackground = true,
                        Image = SchoolSync.Properties.Resources.add_FILL1_wght700_GRAD0_opsz48,
                        Cursor = Cursors.Hand,
                    };
                    gcb_accept.Tag = task[i.ToString()]["token"];
                    gcb_accept.Click += pending_accept;

                    pnl.Controls.Add(pnl_color);
                    pnl.Controls.Add(lbl);
                    pnl.Controls.Add(gcb_close);
                    pnl.Controls.Add(gcb_accept);

                    flowLayoutPanel1.Controls.Add(pnl);
                }
            }

            //incarca din db daca esti admin, student sau creator

            _class = new multiple_class();
            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from educlass where token_user = ? OR admins like ? OR students like ?");

            param = new Dictionary<string, string>()
            {
                {"token_user", Convert.ToString(login_signin.login.accounts_user["token"])},
                {"admins", "%" + Convert.ToString(login_signin.login.accounts_user["token"]) + "%"},
                {"students", "%" + Convert.ToString(login_signin.login.accounts_user["token"]) + "%"},
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            task = await _class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                JObject json = task;
                for (int i = 0; i < json.Count - 1; i++)
                {
                    bool is_use = false;

                    foreach (Control ctrl in flowLayoutPanel1.Controls)
                    {
                        if(ctrl.Tag.ToString() == Convert.ToString(task[i.ToString()]["token"]))
                        {
                            is_use = true;
                            break;
                        }
                    }

                    if (is_use == false)
                    {
                        Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                        {
                            Size = new Size(354, 200),
                            FillColor = Color.FromArgb(223, 229, 232),
                            UseTransparentBackground = true,
                            BorderRadius = 20,
                            Margin = new Padding(3, 3, 20, 30),
                            Cursor = Cursors.Hand,
                        };
                        pnl.Tag = task[i.ToString()]["token"];
                        pnl.Click += curs_Click;

                        Guna.UI2.WinForms.Guna2Panel pnl_color = new Guna.UI2.WinForms.Guna2Panel()
                        {
                            Size = new Size(120, 120),
                            Location = new Point(13, 43),
                            UseTransparentBackground = true,
                            BorderRadius = 60,
                            Cursor = Cursors.Hand,
                        };
                        string[] components = Convert.ToString(task[i.ToString()]["color"]).Split(',');
                        int red = int.Parse(components[0]);
                        int green = int.Parse(components[1]);
                        int blue = int.Parse(components[2]);
                        pnl_color.FillColor = Color.FromArgb(red, green, blue);
                        pnl_color.Tag = task[i.ToString()]["token"];
                        pnl_color.Click += curs_Click;

                        Label lbl = new Label()
                        {
                            AutoSize = false,
                            AutoEllipsis = true,
                            Size = new Size(212, 177),
                            Location = new Point(139, 12),
                            TextAlign = ContentAlignment.MiddleCenter,
                            Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold),
                            Cursor = Cursors.Hand,
                        };
                        lbl.Text = task[i.ToString()]["title"];
                        lbl.Tag = task[i.ToString()]["token"];
                        lbl.Click += curs_Click;

                        pnl.Controls.Add(pnl_color);
                        pnl.Controls.Add(lbl);

                        flowLayoutPanel1.Controls.Add(pnl);
                    }
                }
            }

            schoolsync.hide_loading();

        }
    }
}
