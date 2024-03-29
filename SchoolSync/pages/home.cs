﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SchoolSync.pages
{
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }

        private async void deschide_mesaj(object sender, EventArgs e)
        {
            string token = ((Control)sender).Tag.ToString();

            navbar_home.page = "FlowTalk_home";
            navbar_home.use = false;
            navbar_home.token_page = token;
        }

        private async void home_Load(object sender, EventArgs e)
        {
            schoolsync.show_loading();
            multiple_class _Class = new multiple_class();

            label2.Text = login_signin.login.accounts_user["username"] + "!";
            guna2CirclePictureBox1.Image =  await _Class.IncarcaAvatar(Convert.ToString(login_signin.login.accounts_user["token"]));
            dynamic task;
            
            //incarca informatii materiale educative
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", string.Format("select * from edumentor where is_deleted = 0 and is_visible = 1"));
            task = await _Class.PostRequestAsync(url, data);
            JObject jb = task;
            label7.Text = (jb.Count - 1).ToString();

            try
            {
                url = "https://schoolsync.nnmadalin.me/api/get.php";
                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", string.Format("select * from quotes"));
                task = await _Class.PostRequestAsync(url, data);
                
                label10.Text = "'" + task["0"]["quote"] + "'" + " - " + task["0"]["author"];
            }
            catch
            {
                label10.Text = "'Învingerea nu înseamnă să fii întotdeauna primul. Înseamnă să ajungi acolo unde ai vrut să ajungi, să fii ceea ce ai vrut să fii.'" + " - Kathy Ireland";
            }

            //incarca informatii intrebari invataunit
            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", string.Format("select * from invataunit where is_deleted = 0"));
            task = await _Class.PostRequestAsync(url, data);
            jb = task;
            label8.Text = (jb.Count - 1).ToString();

            //incarca informatii materiale educative

            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", string.Format("select * from edumentor where is_deleted = 0 and is_visible = 1 order by data DESC"));
            task = await _Class.PostRequestAsync(url, data);
            jb = task;

            
            for (int i = 0; i < 3 && i < jb.Count - 1; i++)
            {
                Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                {
                    Size = new Size(700, 70),
                    FillColor = Color.FromArgb(223, 229, 232),
                    Margin = new Padding(0, 0, 0, 20),
                    BorderRadius = 20,                    
                };

                string x = task[i.ToString()]["category"];
                Guna.UI2.WinForms.Guna2CirclePictureBox gpc = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                {
                    Size = new Size(40, 40),
                    Location = new Point(20, 15),
                    Image = EduMentor.incarca_imagine_specifica(x),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    UseTransparentBackground = true,
                    FillColor = Color.Gray,
                };
                Label lbl_title = new Label()
                {
                    Text = task[i.ToString()]["title"],
                    AutoEllipsis = true,
                    AutoSize = false,
                    Location = new Point(70, 15),
                    Size = new Size(320, 20),
                    Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                    BackColor = Color.Transparent,
                };
                Label lbl_creat = new Label()
                {
                    Text = "De: " + task[i.ToString()]["created"],
                    AutoEllipsis = true,
                    AutoSize = false,
                    Location = new Point(70, 37),
                    Size = new Size(320, 30),
                    Font = new Font("Segoe UI", 10),
                    BackColor = Color.Transparent,
                };

                Guna.UI2.WinForms.Guna2CirclePictureBox pct_ceas = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                {
                    FillColor = Color.Gray,
                    Size = new Size(20, 20),
                    Location = new Point(420, 27),
                    Image = SchoolSync.Properties.Resources.schedule_FILL1_wght700_GRAD0_opsz48,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    UseTransparentBackground = true
                };
                Label read_time = new Label()
                {
                    Location = new Point(440, 27),
                    AutoSize = true,
                    Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                    TextAlign = ContentAlignment.TopRight,
                    Text = task[i.ToString()]["reading_time"] + " min",
                    BackColor = Color.Transparent
                };
                Guna.UI2.WinForms.Guna2CirclePictureBox pct_inima = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                {
                    FillColor = Color.Gray,
                    Size = new Size(20, 20),
                    Location = new Point(510, 27),
                    Image = SchoolSync.Properties.Resources.favorite_FILL1_wght700_GRAD0_opsz48,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    UseTransparentBackground = true
                };
                string inimi = task[i.ToString()]["favourites"];
                string[] spinimi = inimi.Split(';');
                Label loves = new Label()
                {
                    Location = new Point(530, 26),
                    AutoSize = true,
                    Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                    TextAlign = ContentAlignment.TopLeft,
                    Text = (spinimi.Length - 1).ToString(),
                    BackColor = Color.Transparent
                };

                var frm = new EduMentor();

                Guna.UI2.WinForms.Guna2Button btn_material = new Guna.UI2.WinForms.Guna2Button()
                {
                    Size = new Size(110, 30),
                    Location = new Point(580, 25),
                    FillColor = Color.Black,
                    BorderRadius = 5,
                    Text = "Vezi material",
                    Tag = task[i.ToString()]["token"],
                    Cursor = Cursors.Hand,
                };
                btn_material.Click += load_material_alt_panel;

                pnl.Controls.Add(gpc);
                pnl.Controls.Add(lbl_title);
                pnl.Controls.Add(lbl_creat);
                pnl.Controls.Add(pct_ceas);
                pnl.Controls.Add(read_time);
                pnl.Controls.Add(pct_inima);
                pnl.Controls.Add(loves);
                pnl.Controls.Add(btn_material);
                flowLayoutPanel1.Controls.Add(pnl);
            }

            //incarcare ultimele 2 convorbiri

            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from flowtalk where people like ? order by data DESC");

            var param = new Dictionary<string, string>()
            {
                {"people", "%" + Convert.ToString(login_signin.login.accounts_user["token"]) +"%"}
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            task = await _Class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                JObject json = task;
                if (json == null)
                    json = new JObject();
                
                for (int i = 0; i < 2 && i < json.Count - 1; i++)
                {
                    try
                    {
                        Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                        {
                            Size = new Size(380, 80),
                            FillColor = Color.FromArgb(223, 229, 232),
                            BorderRadius = 20,
                            Margin = new Padding(0, 0, 0, 40),
                            Cursor = Cursors.Hand,
                            Tag = task[i.ToString()]["token"],
                        };
                        pnl.Click += deschide_mesaj;

                        Guna.UI2.WinForms.Guna2CirclePictureBox gpb = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                        {
                            Size = new Size(70, 70),
                            Location = new Point(25, 5),
                            UseTransparentBackground = true,
                            Cursor = Cursors.Hand,
                            Tag = task[i.ToString()]["token"],
                        };
                        gpb.Click += deschide_mesaj;

                        Label lbl = new Label()
                        {
                            AutoEllipsis = true,
                            Location = new Point(100, 15),
                            Size = new Size(270, 20),
                            Text = task[i.ToString()]["name"],
                            TextAlign = ContentAlignment.MiddleLeft,
                            Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                            BackColor = Color.Transparent,
                            Cursor = Cursors.Hand,
                            Tag = task[i.ToString()]["token"],
                        };
                        lbl.Click += deschide_mesaj;

                        Label lbl_text = new Label()
                        {
                            AutoEllipsis = true,
                            Location = new Point(100, 40),
                            Size = new Size(270, 20),
                            TextAlign = ContentAlignment.MiddleLeft,
                            Text = "Dsad sad sad sad sa dsa",
                            Font = new Font("Segoe UI", 10),
                            BackColor = Color.Transparent,
                            Cursor = Cursors.Hand,
                            Tag = task[i.ToString()]["token"],
                        };
                        lbl_text.Click += deschide_mesaj;

                        dynamic sub = JsonConvert.DeserializeObject(Convert.ToString(task[i.ToString()]["messages"]));

                        JObject message = sub;

                        int x = message.Count - 1;
                        if (x >= 0)
                        {
                            if (Convert.ToString(sub[x.ToString()]["text"]) != "")
                            {
                                if (Convert.ToString(sub[x.ToString()]["root"]) == "0")
                                {
                                    lbl_text.Text = sub[x.ToString()]["user"] + ": " + sub[x.ToString()]["text"];
                                }
                                else
                                {
                                    lbl_text.Text = sub[x.ToString()]["text"];
                                }
                            }
                            else
                            {
                                string y = sub[x.ToString()]["file"];
                                string[] split = y.Split('/');
                                lbl_text.Text = sub[x.ToString()]["user"] + ": " + split[2];

                            }
                        }


                        string[] components = Convert.ToString(task[i.ToString()]["color"]).Split(',');
                        int red = int.Parse(components[0]);
                        int green = int.Parse(components[1]);
                        int blue = int.Parse(components[2]);
                        gpb.FillColor = Color.FromArgb(red, green, blue);

                        pnl.Controls.Add(gpb);
                        pnl.Controls.Add(lbl);
                        pnl.Controls.Add(lbl_text);

                        flowLayoutPanel2.Controls.Add(pnl);
                    }
                    catch {; };
                    
                }
            }

            //incarcare evenimentele de azi - TimePlan

            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from timeplan where token_user = ?");

            param = new Dictionary<string, string>()
            {
                {"token_user",  Convert.ToString(login_signin.login.accounts_user["token"])}
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            task = await _Class.PostRequestAsync(url, data);

            if(task["message"] == "success")
            {
                dynamic calendar = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["calendar"]));
                JObject json = calendar;
                try
                {
                    string dateact = DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year;
                    JArray array = (JArray)json.SelectToken(dateact);
                    if (array != null && array.Type == JTokenType.Array)
                    {
                        foreach (var item in array)
                        {
                            listBox1.Items.Add("• TimePlan: " + item.ToString());
                        }
                    }
                }
                catch
                {
                    try
                    {
                        string dateact = DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year;
                        listBox1.Items.Add("• TimePlan: " + json[dateact]);
                    }
                    catch { };
                }
            }

            //incarcare evenimentele de azi - EduClass

            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from educlass where students like ?");

            param = new Dictionary<string, string>()
            {
                {"students",  "%" + Convert.ToString(login_signin.login.accounts_user["token"]) + "%"}
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            task = await _Class.PostRequestAsync(url, data);

            if (task["message"] == "success")
            {
                dynamic subjson = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["materials"]));
                JObject json = subjson;
                for(int i = 0; i < json.Count; i++)
                {
                    string datetime = subjson[i.ToString()]["deadline"];
                    string[] spl = datetime.Split(' ');
                    if(datetime != "-1")
                    {
                        string dateact = DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year;
                        if (spl[0] == dateact)
                        {
                            listBox1.Items.Add("• EduClass: " + Convert.ToString(subjson[i.ToString()]["title"]));
                        }
                    }
                }
            }


            //incarca date in textbox cautare


            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from accounts");

            task = await _Class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();

                JObject json = task;

                for (int i = 0; i < json.Count - 1; i++)
                {
                    acsc.Add(Convert.ToString(task[i.ToString()]["username"]));
                    acsc.Add(Convert.ToString(task[i.ToString()]["email"]));
                }

                guna2TextBox1.AutoCompleteCustomSource = acsc;
            }

            schoolsync.hide_loading();
        }

        private async void inchide_panel_material(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control.Name == "panel_material")
                {
                    this.Controls.Remove(control);
                    break;
                }
            }
        }

        private async void load_material_alt_panel(object sender, EventArgs e)
        {
            string token_btn = (sender as Guna.UI2.WinForms.Guna2Button).Tag.ToString();
            navbar_home.token_page = token_btn;
            navbar_home.page = "EduMentor_vizualizare_->_home";
            navbar_home.use = false;
        }
        
        private void deschide_fisier_buton(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            System.Diagnostics.Process.Start(@"https://schoolsync.nnmadalin.me/api/getfile.php?token=" + btn.Tag.ToString());
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            string user = guna2TextBox1.Text;

            schoolsync.show_loading();
            multiple_class _Class = new multiple_class();

            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", string.Format("select * from accounts where username like ? or email like ?"));

            var param = new Dictionary<string, string>()
            {
                {"username",  "%" + user + "%"},
                {"email",  "%" + user + "%"},
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _Class.PostRequestAsync(url, data);

            schoolsync.hide_loading();  

            if(task["message"] == "success")
            {
                navbar_home.page = "Profil_person";
                navbar_home.token_page = task["0"]["token"];
                navbar_home.use = false;
            }
            else if(task["message"] == "database no value")
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Nu am gasit persoana!";
                frm.BringToFront();

                guna2TextBox1.Clear();
            }
            else
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Ceva nu a mers bine!";
                frm.BringToFront();

                guna2TextBox1.Clear();
            }

        }
    }
}
