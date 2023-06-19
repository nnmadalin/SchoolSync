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

namespace SchoolSync.pages.FlowTalk_pages
{
    public partial class FlowTalk : UserControl
    {
        public FlowTalk()
        {
            InitializeComponent();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            navbar_home.page = "FlowTalk_adauga";
            navbar_home.use = false;
        }

        string token_message = "", count_tab = "", count_message = "";

        private async void click_message(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;

            if (guna2Panel3.Visible == false)
            {
                guna2Panel3.Visible = true;
                flowLayoutPanel2.Visible = true;
                guna2TextBox1.Visible = true;
                guna2Button1.Visible = true;
            }

            token_message = ctrl.Tag.ToString();
            count_message = "";
            flowLayoutPanel2.Tag = token_message;
            flowLayoutPanel2.Controls.Clear();
        }

        async void load_tab()
        {           
            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from flowtalk where people like ? order by data ASC");

            var param = new Dictionary<string, string>()
            {
                {"people", "%" + Convert.ToString(login_signin.login.accounts_user["token"]) +"%"}
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                JObject jb = task;
                if (jb.Count.ToString() != count_tab)
                {
                    flowLayoutPanel1.Controls.Clear();
                    count_tab = jb.Count.ToString();
                    for (int i = 0; i < jb.Count - 1; i++)
                    {
                        Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                        {
                            Size = new Size(273, 68),
                            BorderRadius = 15,
                            UseTransparentBackground = true,
                            FillColor = Color.FromArgb(50, 50, 60),
                            Margin = new Padding(3, 3, 3, 10),
                            Cursor = Cursors.Hand
                        };
                        pnl.Click += click_message;
                        pnl.Tag = task[i.ToString()]["token"];

                        string[] components = Convert.ToString(task[i.ToString()]["color"]).Split(',');
                        int red = int.Parse(components[0]);
                        int green = int.Parse(components[1]);
                        int blue = int.Parse(components[2]);

                        Guna.UI2.WinForms.Guna2CirclePictureBox pct = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                        {
                            Size = new Size(40, 40),
                            Location = new Point(15, 16),
                            FillColor = Color.FromArgb(red, green, blue),
                            UseTransparentBackground = true,
                            Cursor = Cursors.Hand
                        };
                        pct.Click += click_message;
                        pct.Tag = task[i.ToString()]["token"];
                        Label name = new Label()
                        {
                            Font = new Font("Segoe UI", 9, FontStyle.Bold),
                            Location = new Point(62, 16),
                            ForeColor = Color.White,
                            Size = new Size(208, 22),
                            AutoEllipsis = true,
                            Text = task[i.ToString()]["name"],
                            Cursor = Cursors.Hand
                        };
                        name.Click += click_message;
                        name.Tag = task[i.ToString()]["token"];
                        Label last_mess = new Label()
                        {
                            Font = new Font("Segoe UI", 9, FontStyle.Bold),
                            Location = new Point(62, 35),
                            ForeColor = Color.FromArgb(207, 207, 207),
                            Size = new Size(208, 24),
                            AutoEllipsis = true,
                            Cursor = Cursors.Hand
                        };
                        last_mess.Click += click_message;
                        last_mess.Tag = task[i.ToString()]["token"];

                        dynamic sub = JsonConvert.DeserializeObject(Convert.ToString(task[i.ToString()]["messages"]));

                        JObject message = sub;

                        int x = message.Count - 1;
                        if (x >= 0)
                            last_mess.Text = sub[x.ToString()]["text"];

                        pnl.Controls.Add(pct);
                        pnl.Controls.Add(name);
                        pnl.Controls.Add(last_mess);

                        flowLayoutPanel1.Controls.Add(pnl);
                    }
                }
            }
        }

        private async void FlowTalk_Load(object sender, EventArgs e)
        {
            schoolsync.show_loading();

            load_tab();

            schoolsync.hide_loading();
        }

        private async void load_message_Tick(object sender, EventArgs e)
        {
            if(token_message != "")
            {
                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                var data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from flowtalk where token = ?");

                var param = new Dictionary<string, string>()
                {
                    {"token", token_message}
                };

                data.Add("params", JsonConvert.SerializeObject(param));                

                dynamic task = await _class.PostRequestAsync(url, data);

                Console.WriteLine(task);
                if (task["message"] == "success")
                {
                    string[] components = Convert.ToString(task["0"]["color"]).Split(',');
                    int red = int.Parse(components[0]);
                    int green = int.Parse(components[1]);
                    int blue = int.Parse(components[2]);

                    guna2CirclePictureBox2.FillColor = Color.FromArgb(red, green, blue);

                    label4.Text = task["0"]["name"];

                    dynamic sub = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["messages"]));

                    JObject jb = sub;
                    if (count_message == "")
                    {
                        flowLayoutPanel2.Controls.Clear();
                    }
                    if (jb.Count.ToString() != count_message)
                    {
                        count_message = jb.Count.ToString();
                        for (int i = 0; i < jb.Count; i++)
                        {

                            Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                            {
                                MinimumSize = new Size(800, 30),
                                MaximumSize = new Size(800, 0),
                                AutoSize = true,
                                Margin = new Padding(0, 0, 0, 15),
                            };

                            if(sub[i.ToString()]["root"] == "0" && sub[i.ToString()]["file"] == "") 
                            {
                                Guna.UI2.WinForms.Guna2CirclePictureBox pct = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                                {
                                    Size = new Size(40, 40),
                                    Location = new Point(0, 5),
                                    FillColor = Color.White,
                                };
                                pnl.Controls.Add(pct);
                                Label lbl = new Label()
                                {
                                    AutoSize = true,
                                    Location = new Point(55, 15),
                                    MinimumSize = new Size(750, 10),
                                    MaximumSize = new Size(750, 0),
                                    Font = new Font("Segoe UI", 10),
                                    ForeColor = Color.White,
                                    Text = "",
                                };
                                pnl.Controls.Add(lbl);
                            }              
                            else if(sub[i.ToString()]["root"] == "1")
                            {
                                Label lbl = new Label()
                                {
                                    AutoSize = true,
                                    Location = new Point(55, 15),
                                    MinimumSize = new Size(750, 10),
                                    MaximumSize = new Size(750, 0),
                                    Padding = new Padding(100, 0, 100, 0),
                                    Font = new Font("Segoe UI", 10,FontStyle.Bold),
                                    ForeColor = Color.FromArgb(219, 219, 219),
                                    Text = sub[i.ToString()]["text"],
                                    TextAlign = ContentAlignment.TopCenter
                                };
                                pnl.Controls.Add(lbl);
                            }

                            

                            flowLayoutPanel2.Controls.Add(pnl);
                        }
                    }
                }
            }
        }

        private async void load_tab_message_Tick(object sender, EventArgs e)
        {
            load_tab();
        }
    }
}
