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
    public partial class EduClass_Vizualizare_Persoane : UserControl
    {
        public EduClass_Vizualizare_Persoane()
        {
            InitializeComponent();
        }

        string token_local = "";
        
        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            navbar_home.token_page = token_local;
            navbar_home.page = "EduClass_vizualizare";
            navbar_home.use = false;
        }

        async Task<string> getusername (string token)
        {
            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from accounts where token = ?");

            var param = new Dictionary<string, string>()
            {
                {"token", token},
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);

            if (task["message"] == "success")
            {
                return task["0"]["username"];
            }
            else
                return "null";
        }

        async void deschide_profil(object sender, EventArgs e)
        {
            string token = ((Control)sender).Tag.ToString();

            navbar_home.token_page = token;
            navbar_home.page = "Profil_person";
            navbar_home.use = false;
        }

        async void delete_user(object sender, EventArgs e)
        {
            string[] tokens = Convert.ToString(((Control)sender).Tag.ToString()).Split(';');

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from educlass where token = ?");

            var param = new Dictionary<string, string>()
            {
                {"token", token_local},
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);

            if (task["message"] == "success")
            {
                url = "https://schoolsync.nnmadalin.me/api/put.php";
                data = new Dictionary<string, string>();
                
                if (tokens[0] == "admin")
                {
                    string[] prm = Convert.ToString(task["0"]["admins"]).Split(';');
                    string newprm = "";

                    for(int i = 0; i < prm.Length - 1; i++)
                    {
                        if(prm[i] != tokens[1])
                        {
                            newprm += (prm[i] + ";");
                        }
                    }

                    data.Add("token", schoolsync.token);
                    data.Add("command", "update educlass set admins = ? where token = ?");

                    param = new Dictionary<string, string>()
                    {
                        {"admins", newprm},
                        {"token", token_local},
                    };

                    data.Add("params", JsonConvert.SerializeObject(param));

                    task = await _class.PostRequestAsync(url, data);

                    load_persoane_dinamic();
                }
                else if (tokens[0] == "pending")
                {
                    string[] prm = Convert.ToString(task["0"]["pending"]).Split(';');
                    string newprm = "";

                    for (int i = 0; i < prm.Length - 1; i++)
                    {
                        if (prm[i] != tokens[1])
                        {
                            newprm += (prm[i] + ";");
                        }
                    }

                    data.Add("token", schoolsync.token);
                    data.Add("command", "update educlass set pending = ? where token = ?");

                    param = new Dictionary<string, string>()
                    {
                        {"pending", newprm},
                        {"token", token_local},
                    };

                    data.Add("params", JsonConvert.SerializeObject(param));

                    task = await _class.PostRequestAsync(url, data);

                    load_persoane_dinamic();
                }
                else if (tokens[0] == "student")
                {
                    string[] prm = Convert.ToString(task["0"]["students"]).Split(';');
                    string newprm = "";

                    for (int i = 0; i < prm.Length - 1; i++)
                    {
                        if (prm[i] != tokens[1])
                        {
                            newprm += (prm[i] + ";");
                        }
                    }

                    data.Add("token", schoolsync.token);
                    data.Add("command", "update educlass set students = ? where token = ?");

                    param = new Dictionary<string, string>()
                    {
                        {"students", newprm},
                        {"token", token_local},
                    };

                    data.Add("params", JsonConvert.SerializeObject(param));

                    task = await _class.PostRequestAsync(url, data);

                    load_persoane_dinamic();
                }
            }
        }

        async void load_persoane_dinamic()
        {
            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from educlass where token = ?");

            var param = new Dictionary<string, string>()
            {
                {"token", token_local},
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);

            schoolsync.hide_loading();

            if (task["message"] == "success")
            {
                string[] admins = Convert.ToString(task["0"]["admins"]).Split(';');

                for (int i = 0; i < admins.Length - 1; i++)
                {
                    if (admins[i] == Convert.ToString(login_signin.login.accounts_user["token"]))
                    {
                        is_admin = true;
                        break;
                    }
                }

                if (is_admin == false)
                {
                    guna2Panel1.Visible = false;
                }

                //incarcare studenti / admini / pending

                flowLayoutPanel2.Controls.Clear();
                flowLayoutPanel3.Controls.Clear();
                flowLayoutPanel4.Controls.Clear();

                //admini
                string[] admini = Convert.ToString(task["0"]["admins"]).Split(';');
                for (int i = 0; i < admini.Length - 1; i++)
                {
                    Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        Size = new Size(990, 50),
                        BorderColor = Color.FromArgb(196, 196, 196),
                        BorderThickness = 1,
                        BorderRadius = 15,
                        Margin = new Padding(0, 0, 0, 10),
                        Cursor = Cursors.Hand,
                    };
                    pnl.Tag = admini[i];
                    pnl.Click += deschide_profil;

                    Guna.UI2.WinForms.Guna2CirclePictureBox gcp = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                    {
                        Size = new Size(40, 40),
                        Location = new Point(10, 5),
                        Cursor = Cursors.Hand,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                    };
                    gcp.Image = await _class.IncarcaAvatar(admini[i]);
                    gcp.Tag = admini[i];
                    gcp.Click += deschide_profil;

                    Label lbl = new Label()
                    {
                        AutoSize = false,
                        AutoEllipsis = true,
                        Size = new Size(870, 40),
                        Location = new Point(60, 5),
                        TextAlign = ContentAlignment.MiddleLeft,
                        Cursor = Cursors.Hand,
                        Font = new Font("Segoe UI", 15, FontStyle.Bold),
                    };
                    lbl.Text = await getusername(admini[i]);
                    lbl.Tag = admini[i];
                    lbl.Click += deschide_profil;

                    if(is_admin == true && admini[i] != Convert.ToString(login_signin.login.accounts_user["token"]))
                    {
                        Guna.UI2.WinForms.Guna2CircleButton gcb = new Guna.UI2.WinForms.Guna2CircleButton()
                        {
                            FillColor = Color.WhiteSmoke,
                            Size = new Size(40, 40),
                            Image = SchoolSync.Properties.Resources.delete_FILL1_wght700_GRAD0_opsz48,
                            Location = new Point(940, 5),
                            Cursor = Cursors.Hand,
                        };
                        gcb.Tag = "admin;" + admini[i];
                        gcb.Click += delete_user;
                        pnl.Controls.Add(gcb);
                    }

                    pnl.Controls.Add(gcp);
                    pnl.Controls.Add(lbl);

                    flowLayoutPanel2.Controls.Add(pnl);
                }

                //pending
                string[] pending = Convert.ToString(task["0"]["pending"]).Split(';');
                for (int i = 0; i < pending.Length - 1; i++)
                {
                    Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        Size = new Size(990, 50),
                        BorderColor = Color.FromArgb(196, 196, 196),
                        BorderThickness = 1,
                        BorderRadius = 15,
                        Margin = new Padding(0, 0, 0, 10),
                        Cursor = Cursors.Hand,
                    };
                    pnl.Tag = pending[i];
                    pnl.Click += deschide_profil;

                    Guna.UI2.WinForms.Guna2CirclePictureBox gcp = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                    {
                        Size = new Size(40, 40),
                        Location = new Point(10, 5),
                        Cursor = Cursors.Hand,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                    };
                    gcp.Image = await _class.IncarcaAvatar(pending[i]);
                    gcp.Tag = pending[i];
                    gcp.Click += deschide_profil;

                    Label lbl = new Label()
                    {
                        AutoSize = false,
                        AutoEllipsis = true,
                        Size = new Size(870, 40),
                        Location = new Point(60, 5),
                        TextAlign = ContentAlignment.MiddleLeft,
                        Cursor = Cursors.Hand,
                        Font = new Font("Segoe UI", 15, FontStyle.Bold),
                    };
                    lbl.Text = await getusername(pending[i]);
                    lbl.Tag = pending[i];
                    lbl.Click += deschide_profil;

                    if (is_admin == true && pending[i] != Convert.ToString(login_signin.login.accounts_user["token"]))
                    {
                        Guna.UI2.WinForms.Guna2CircleButton gcb = new Guna.UI2.WinForms.Guna2CircleButton()
                        {
                            FillColor = Color.WhiteSmoke,
                            Size = new Size(40, 40),
                            Image = SchoolSync.Properties.Resources.delete_FILL1_wght700_GRAD0_opsz48,
                            Location = new Point(940, 5),
                            Cursor = Cursors.Hand,
                        };
                        gcb.Tag = "pending;" + pending[i];
                        gcb.Click += delete_user;
                        pnl.Controls.Add(gcb);
                    }

                    pnl.Controls.Add(gcp);
                    pnl.Controls.Add(lbl);

                    flowLayoutPanel3.Controls.Add(pnl);
                }

                //students
                string[] students = Convert.ToString(task["0"]["students"]).Split(';');
                for (int i = 0; i < students.Length - 1; i++)
                {
                    Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        Size = new Size(990, 50),
                        BorderColor = Color.FromArgb(196, 196, 196),
                        BorderThickness = 1,
                        BorderRadius = 15,
                        Margin = new Padding(0, 0, 0, 10),
                        Cursor = Cursors.Hand,
                    };
                    pnl.Tag = students[i];
                    pnl.Click += deschide_profil;

                    Guna.UI2.WinForms.Guna2CirclePictureBox gcp = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                    {
                        Size = new Size(40, 40),
                        Location = new Point(10, 5),
                        Cursor = Cursors.Hand,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                    };
                    gcp.Image = await _class.IncarcaAvatar(students[i]);
                    gcp.Tag = students[i];
                    gcp.Click += deschide_profil;

                    Label lbl = new Label()
                    {
                        AutoSize = false,
                        AutoEllipsis = true,
                        Size = new Size(870, 40),
                        Location = new Point(60, 5),
                        TextAlign = ContentAlignment.MiddleLeft,
                        Cursor = Cursors.Hand,
                        Font = new Font("Segoe UI", 15, FontStyle.Bold),
                    };
                    lbl.Text = await getusername(students[i]);
                    lbl.Tag = students[i];
                    lbl.Click += deschide_profil;

                    if (is_admin == true && students[i] != Convert.ToString(login_signin.login.accounts_user["token"]))
                    {
                        Guna.UI2.WinForms.Guna2CircleButton gcb = new Guna.UI2.WinForms.Guna2CircleButton()
                        {
                            FillColor = Color.WhiteSmoke,
                            Size = new Size(40, 40),
                            Image = SchoolSync.Properties.Resources.delete_FILL1_wght700_GRAD0_opsz48,
                            Location = new Point(940, 5),
                            Cursor = Cursors.Hand,
                        };
                        gcb.Tag = "student;" + students[i];
                        gcb.Click += delete_user;
                        pnl.Controls.Add(gcb);
                    }

                    pnl.Controls.Add(gcp);
                    pnl.Controls.Add(lbl);

                    flowLayoutPanel4.Controls.Add(pnl);
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

                navbar_home.page = "EduClass_vizualizare";
                navbar_home.use = false;
            }
            
        }

        bool is_admin = false;

        private async void EduClass_Vizualizare_Persoane_Load(object sender, EventArgs e)
        {
            token_local = navbar_home.token_page;

            schoolsync.show_loading();

            multiple_class _class = new multiple_class();

            //incarca date in textbox suggest

            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from accounts");
            dynamic task = await _class.PostRequestAsync(url, data);

            if (task["message"] == "success")
            {
                AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
                JObject jb = task;
                for (int i = 0; i < jb.Count - 1; i++)
                {
                    string username = Convert.ToString(task[i.ToString()]["username"]);
                    collection.Add(username);

                }
                guna2TextBox2.AutoCompleteCustomSource = collection;
            }



            load_persoane_dinamic();

            schoolsync.hide_loading();
            
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            string username = guna2TextBox2.Text.Trim();

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from accounts where username like ?");

            var param = new Dictionary<string, string>()
            {
                {"username", "%" + username +"%"}
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                string token_user = task["0"]["token"];

                url = "https://schoolsync.nnmadalin.me/api/get.php";
                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from educlass where token = ?");

                param = new Dictionary<string, string>()
                {
                    {"token", token_local}
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                task = await _class.PostRequestAsync(url, data);

                if (task["message"] == "success")
                {

                    if (guna2RadioButton1.Checked == true)
                    {
                        string[] split = Convert.ToString(task["0"]["admins"]).Split(';');

                        for (int i = 0; i < split.Length - 1; i++)
                        {
                            if (split[i] == token_user)
                            {
                                var frm = new notification.error();
                                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                                panel.Controls.Add(frm);
                                notification.error.message = "Utilizatorul este deja! (grad: admin)";
                                frm.BringToFront();

                                return;
                            }
                        }

                        split = Convert.ToString(task["0"]["pending"]).Split(';');

                        for (int i = 0; i < split.Length - 1; i++)
                        {
                            if (split[i] == token_user)
                            {
                                var frm = new notification.error();
                                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                                panel.Controls.Add(frm);
                                notification.error.message = "Cererea este deja trimisa catre utilizator!";
                                frm.BringToFront();

                                return;
                            }
                        }

                        split = Convert.ToString(task["0"]["students"]).Split(';');

                        for (int i = 0; i < split.Length - 1; i++)
                        {
                            if (split[i] == token_user)
                            {
                                var frm = new notification.error();
                                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                                panel.Controls.Add(frm);
                                notification.error.message = "Utilizatorul este deja! (grad: student)";
                                frm.BringToFront();

                                return;
                            }
                        }

                    }
                    else
                    {
                        string[] split = Convert.ToString(task["0"]["students"]).Split(';');

                        bool is_student = false;
                        for (int i = 0; i < split.Length - 1; i++)
                        {
                            if (split[i] == token_user)
                            {
                                is_student = true;
                                break;
                            }
                        }

                        if (is_student == false)
                        {
                            var frm = new notification.error();
                            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                            panel.Controls.Add(frm);
                            notification.error.message = "Utilizatorul nu este student!";
                            frm.BringToFront();

                            return;
                        }

                        split = Convert.ToString(task["0"]["admins"]).Split(';');

                        is_student = false;
                        for (int i = 0; i < split.Length - 1; i++)
                        {
                            if (split[i] == token_user)
                            {
                                is_student = true;
                                break;
                            }
                        }

                        if (is_student == true)
                        {
                            var frm = new notification.error();
                            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                            panel.Controls.Add(frm);
                            notification.error.message = "Utilizatorul este deja admin!";
                            frm.BringToFront();

                            return;
                        }
                    }

                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);

                    string prm = "";


                    if (guna2RadioButton1.Checked == true)
                    {
                        data.Add("command", "update educlass set pending = ? where token = ?");
                        prm = task["0"]["pending"];
                        prm += token_user + ";";
                        param = new Dictionary<string, string>()
                        {
                            {"prm", prm},
                            {"token", token_local}
                        };
                    }
                    else
                    {
                        data.Add("command", "update educlass set admins = ?, students = ? where token = ?");
                        prm = task["0"]["admins"];
                        prm += token_user + ";";

                        string[] prm2 = Convert.ToString(task["0"]["students"]).Split(';');
                        string newprm = "";

                        for(int i = 0; i < prm2.Length - 1; i++)
                        {
                            if(prm2[i] != token_user)
                            {
                                newprm += prm2[i] + ";";
                            }
                        }
                        
                        param = new Dictionary<string, string>()
                        {
                            {"prm", prm},
                            {"students", newprm},
                            {"token", token_local}
                        };
                    }
                    

                    data.Add("params", JsonConvert.SerializeObject(param));

                    task = await _class.PostRequestAsync(url, data);
                    if (task["message"] == "update success")
                    {
                        load_persoane_dinamic();
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

                    navbar_home.token_page = token_local;
                    navbar_home.page = "EduClass_vizualizare";
                    navbar_home.use = false;
                }
            }
            else
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Nu am gasit user!";
                frm.BringToFront();
            }
        }
    }
}
