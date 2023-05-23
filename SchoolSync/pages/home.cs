using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace SchoolSync.pages
{
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }

       

        private async void home_Load(object sender, EventArgs e)
        {
            schoolsync.show_loading();
            multiple_class _Class = new multiple_class();

            label2.Text = login_signin.login.accounts_user["username"] + "!";
            guna2CirclePictureBox1.Image =  await _Class.IncarcaImagineAsync("https://schoolsync.nnmadalin.me/api/getfile.php?token=userfoto_" + login_signin.login.accounts_user["token"] + ".png");

            dynamic task = await _Class.getstring("https://type.fit/api/quotes");

            Random rand = new Random();
            int p = Convert.ToInt32(rand.Next(0, 1201).ToString());

            label10.Text = "'" + task[p]["text"] + "'" + " - " + task[p]["author"];

            //incarca informatii materiale educative
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("select * from edumentor "));
            task = await _Class.PostRequestAsync_norefresh(url, data);
            JObject jb = task;
            label7.Text = (jb.Count - 1).ToString();

            //incarca informatii intrebari invataunit
            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("select * from invataunit "));
            task = await _Class.PostRequestAsync_norefresh(url, data);
            jb = task;
            label8.Text = (jb.Count - 1).ToString();

            //incarca informatii materiale educative

            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("select * from edumentor order by data DESC"));
            task = await _Class.PostRequestAsync_norefresh(url, data);
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
                string inimi = task[i.ToString()]["users_hearts"];
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
            Panel pnl = new Panel()
            {
                Size = new Size(1192, 690),
                AutoScroll = true,
                BackColor = Color.FromArgb(237, 237, 237),
                Name = "panel_material"
            };
            Guna.UI2.WinForms.Guna2CircleButton btn = new Guna.UI2.WinForms.Guna2CircleButton()
            {
                Image = SchoolSync.Properties.Resources.close_FILL1_wght700_GRAD0_opsz48,
                Size = new Size(35, 35),
                ImageAlign = HorizontalAlignment.Center,
                ImageSize = new Size(20, 20),
                UseTransparentBackground = true,
                FillColor = Color.White,
                Location = new Point(1130, 20),
                Cursor = Cursors.Hand
            };
            pnl.Controls.Add(btn);
            btn.Click += inchide_panel_material;


            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("select * from edumentor where token = '{0}'", token_btn));

            dynamic task = await _class.PostRequestAsync(url, data);
            JObject jb = task;

            if (task["message"] == "success")
            {
                Guna.UI2.WinForms.Guna2Panel panglica = new Guna.UI2.WinForms.Guna2Panel()
                {
                    Location = new Point(0, 0),
                    Size = new Size(1172, 200),
                    FillColor = task["0"]["color"]
                };

                Guna.UI2.WinForms.Guna2Panel center = new Guna.UI2.WinForms.Guna2Panel()
                {
                    Location = new Point(196, 100),
                    Size = new Size(800, 300),
                    FillColor = Color.White,
                    BorderRadius = 15,
                    UseTransparentBackground = true,
                };
                Label title = new Label()
                {
                    Location = new Point(0, 0),
                    Size = new Size(800, 200),
                    Font = new Font("Segoe UI Semibold", 22, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    MaximumSize = new Size(800, 200),
                    MinimumSize = new Size(800, 200),
                    Text = task["0"]["title"],
                };
                Guna.UI2.WinForms.Guna2Separator gsp = new Guna.UI2.WinForms.Guna2Separator()
                {
                    UseTransparentBackground = true,
                    Location = new Point(0, 210),
                    Size = new Size(800, 2),
                };
                Guna.UI2.WinForms.Guna2CirclePictureBox pct_usr = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                {
                    FillColor = Color.Gray,
                    Size = new Size(50, 50),
                    Location = new Point(20, 230),
                    InitialImage = SchoolSync.Properties.Resources.standard_avatar,
                    ErrorImage = SchoolSync.Properties.Resources.standard_avatar,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    UseTransparentBackground = true
                };
                Label nume = new Label()
                {
                    Location = new Point(75, 235),
                    AutoSize = true,
                    Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                    TextAlign = ContentAlignment.TopLeft,
                    Text = task["0"]["created"]
                };
                string date = task["0"]["data"];
                Label descriere = new Label()
                {
                    Location = new Point(75, 255),
                    AutoSize = true,
                    Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                    TextAlign = ContentAlignment.TopLeft,
                    ForeColor = Color.FromArgb(140, 140, 140),
                    Text = task["0"]["category"] + " • " + Convert.ToDateTime(date).ToShortDateString(),
                };
                Guna.UI2.WinForms.Guna2CirclePictureBox pct_ceas = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                {
                    FillColor = Color.Gray,
                    Size = new Size(20, 20),
                    Location = new Point(675, 237),
                    Image = SchoolSync.Properties.Resources.schedule_FILL1_wght700_GRAD0_opsz48,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    UseTransparentBackground = true
                };
                Label read_time = new Label()
                {
                    Location = new Point(700, 235),
                    AutoSize = true,
                    Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                    TextAlign = ContentAlignment.TopRight,
                    Text = task["0"]["reading_time"] + " min"
                };
                Guna.UI2.WinForms.Guna2CirclePictureBox pct_inima = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                {
                    FillColor = Color.Gray,
                    Size = new Size(20, 20),
                    Location = new Point(675, 260),
                    Image = SchoolSync.Properties.Resources.favorite_FILL1_wght700_GRAD0_opsz48,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    UseTransparentBackground = true
                };
                string inimi = task["0"]["users_hearts"];
                string[] spinimi = inimi.Split(';');
                Label loves = new Label()
                {
                    Location = new Point(700, 257),
                    AutoSize = true,
                    Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                    TextAlign = ContentAlignment.TopLeft,
                    Text = (spinimi.Length - 1).ToString()
                };

                FlowLayoutPanel flp_main = new FlowLayoutPanel()
                {
                    Location = new Point(0, 450),
                    MinimumSize = new Size(1172, 20),
                    MaximumSize = new Size(1172, 0),
                    Padding = new Padding(0, 0, 0, 20),
                    AutoSize = true,
                };

                Guna.UI2.WinForms.Guna2Panel flp_panel = new Guna.UI2.WinForms.Guna2Panel()
                {
                    AutoSize = true,
                    MinimumSize = new Size(1000, 0),
                    MaximumSize = new Size(1000, 0),
                    FillColor = Color.White,
                    BorderRadius = 15,
                    UseTransparentBackground = true,
                    Margin = new Padding(92, 0, 0, 0),
                };

                FlowLayoutPanel flp = new FlowLayoutPanel()
                {
                    Location = new Point(10, 10),
                    MinimumSize = new Size(980, 0),
                    MaximumSize = new Size(980, 0),
                    Padding = new Padding(0, 0, 0, 20),
                    BackColor = Color.White,
                    AutoSize = true,
                };

                Label descriere_material = new Label()
                {
                    MinimumSize = new Size(980, 0),
                    MaximumSize = new Size(980, 0),
                    AutoSize = true,
                    Padding = new Padding(0, 0, 0, 10),
                    Font = new Font("Segoe UI", 13),
                    Text = task["0"]["description"],
                };

                pct_usr.Image = await _class.IncarcaImagineAsync("https://schoolsync.nnmadalin.me/api/getfile.php?token=userfoto_" + task["0"]["token_user"] + ".png");

                center.Controls.Add(title);
                center.Controls.Add(gsp);
                center.Controls.Add(pct_usr);
                center.Controls.Add(nume);
                center.Controls.Add(descriere);
                center.Controls.Add(pct_ceas);
                center.Controls.Add(read_time);
                center.Controls.Add(pct_inima);
                center.Controls.Add(loves);

                flp_main.Controls.Add(flp_panel);

                flp_panel.Controls.Add(flp);

                flp.Controls.Add(descriere_material);

                string file = task["0"]["files"];
                string[] file_split = file.Split(';');

                for (int i = 0; i < file_split.Length - 1; i++)
                {
                    string[] splitplit = file_split[i].Split('.');
                    Guna.UI2.WinForms.Guna2Panel flp_files_panel = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        Size = new Size(200, 180),
                        FillColor = Color.FromArgb(235, 241, 244),
                        BorderRadius = 10,
                        UseTransparentBackground = true
                    };
                    Guna.UI2.WinForms.Guna2CirclePictureBox gcp = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                    {
                        Size = new Size(60, 60),
                        UseTransparentBackground = true,
                        SizeMode = PictureBoxSizeMode.CenterImage,
                        FillColor = Color.FromArgb(208, 216, 220),
                        Location = new Point(65, 15),
                    };
                    Label lbl_panel_file = new Label()
                    {
                        Location = new Point(0, 80),
                        AutoSize = true,
                        MinimumSize = new Size(190, 0),
                        Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold),
                        TextAlign = ContentAlignment.TopCenter,
                    };
                    Guna.UI2.WinForms.Guna2Button panel_file_btn = new Guna.UI2.WinForms.Guna2Button()
                    {
                        FillColor = Color.FromArgb(112, 204, 97),
                        ForeColor = Color.Black,
                        BorderRadius = 15,
                        Cursor = Cursors.Hand,
                        Text = "Download",
                        TextAlign = HorizontalAlignment.Left,
                        Image = SchoolSync.Properties.Resources.download_FILL1_wght700_GRAD0_opsz48,
                        ImageSize = new Size(15, 15),
                        ImageAlign = HorizontalAlignment.Right,
                        Size = new Size(110, 30),
                        Font = new Font("Segoe UI Semibold", 9, FontStyle.Bold),
                        Location = new Point((200 - 110) / 2, 140)
                    };

                    flp.Controls.Add(flp_files_panel);
                    flp_files_panel.Controls.Add(gcp);
                    flp_files_panel.Controls.Add(lbl_panel_file);

                    if (splitplit[0].Length >= 10)
                    {
                        lbl_panel_file.Text = splitplit[0].Substring(0, 10) + "." + splitplit[1];
                    }
                    else
                        lbl_panel_file.Text = file_split[i];
                    if (splitplit[1] == "jpg" || splitplit[1] == "jpeg" || splitplit[1] == "png" || splitplit[1] == "svg" || splitplit[1] == "webp" || splitplit[1] == "bmp")
                    {
                        gcp.Image = SchoolSync.Properties.Resources.image_FILL1_wght700_GRAD0_opsz48;
                    }
                    else
                        gcp.Image = SchoolSync.Properties.Resources.description_FILL1_wght700_GRAD0_opsz48;

                    flp_files_panel.Controls.Add(panel_file_btn);

                    panel_file_btn.Tag = file_split[i];
                    panel_file_btn.Click += deschide_fisier_buton;

                }




                pnl.Controls.Add(flp_main);
                pnl.Controls.Add(center);
                pnl.Controls.Add(panglica);

            }


            this.Controls.Add(pnl);
            pnl.BringToFront();
        }
        
        private void deschide_fisier_buton(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            System.Diagnostics.Process.Start(@"https://schoolsync.nnmadalin.me/api/getfile.php?token=" + btn.Tag.ToString());
        }
    }
}
