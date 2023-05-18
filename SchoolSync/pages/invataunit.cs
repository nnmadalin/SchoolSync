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
using System.IO;

namespace SchoolSync.pages
{
    public partial class invataunit : UserControl
    {
        public invataunit()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            foreach (var control in panel_materii.Controls)
            {
                if(control is Guna.UI2.WinForms.Guna2Button)
                {
                    ((Guna.UI2.WinForms.Guna2Button)control).FillColor = Color.FromArgb(255, 255, 255);
                }
            }

            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            btn.FillColor = Color.FromArgb(225, 225, 225);
            sort = btn.Text;
            load_question_panel();
        }

        private void close_add_question(object sender, EventArgs e)
        {
            foreach(Control control in this.Controls)
            {
                if(control.Name == "panel_question")
                {
                    this.Controls.Remove(control);
                    break;
                }
            }
        }
        private void close_question_with_answer(object sender, EventArgs e)
        {
            foreach(Control control in this.Controls)
            {
                if(control.Name == "panel_question_with_answer")
                {
                    this.Controls.Remove(control);
                    break;
                }
            }
        }

        private void open_file_dialog(object sender, EventArgs e)
        {
            if (this.Controls["panel_question"].Controls["sub_panel_question"].Controls["flp_files"].Controls.Count < 5)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.FileName = "";
                opf.Filter = "Files (*.jpg; *.jpeg; *.png; *.svg; *.webp; *.bmp; *.doc; *.docx; *.ppt; *.pptx; *.xlsx; *.xls; *.txt; *.pdf) " +
                    "| *.jpg; *.jpeg; *.png; *.svg; *.webp; *.bmp; *.doc; *.docx; *.ppt; *.pptx; *.xlsx; *.xls; *.txt; *.pdf";
                DialogResult dir = opf.ShowDialog();


                if (dir == DialogResult.OK)
                {
                    FileInfo fl = new FileInfo(opf.FileName);

                    long fileSizeibBytes = fl.Length;
                    long fileSizeibMbs = fileSizeibBytes / (1024 * 1024);

                    if (fileSizeibMbs > 5)
                    {
                        var frm = new notification.error();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.error.message = "Fisierul: " + fl.Name.Substring(0, 20) + "..." + " are mai mult de 5 MB!";
                        frm.BringToFront();
                    }
                    else
                    {
                        Guna.UI2.WinForms.Guna2Chip guna2Chip = new Guna.UI2.WinForms.Guna2Chip()
                        {
                            FillColor = Color.FromArgb(180, 180, 180),
                            BorderColor = Color.FromArgb(180, 180, 180),
                            ForeColor = Color.Black,
                            Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                            AutoRoundedCorners = false,
                            BorderRadius = 10,
                            TextAlign = HorizontalAlignment.Left,
                            Size = new Size(100, 35),
                            Text = System.IO.Path.GetFileName(opf.FileName).Substring(0, 8) + "...",
                            Tag = opf.FileName.ToString()
                        };

                        this.Controls["panel_question"].Controls["sub_panel_question"].Controls["flp_files"].Controls.Add(guna2Chip);
                    }
                }
            }
            else
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                frm.Location = new Point(840, 50);
                frm.Show();
                notification.error.message = "Poti adauga maxim 5 fisiere!";
                frm.BringToFront();
            }
        }

        private void panel_question_textbox(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2TextBox btn = sender as Guna.UI2.WinForms.Guna2TextBox;
            btn.BorderColor = Color.FromArgb(213, 218, 223);
        }
        
        private async void send_question(object sender, EventArgs e)
        {
            if(this.Controls["panel_question"].Controls["sub_panel_question"].Controls["txtbox"].Text.Trim() == "")
            {
                ((Guna.UI2.WinForms.Guna2TextBox)this.Controls["panel_question"].Controls["sub_panel_question"].Controls["txtbox"]).BorderColor = Color.Red;
            }
            else
            {
                //TOTUL ESTE BINE
                schoolsync.show_loading();

                ((Guna.UI2.WinForms.Guna2TextBox)this.Controls["panel_question"].Controls["sub_panel_question"].Controls["txtbox"]).BorderColor = Color.FromArgb(213, 218, 223);

                multiple_class _class = new multiple_class();
                string token = _class.generate_token();

                Control combobox = this.Controls["panel_question"].Controls["sub_panel_question"].Controls["combobox_materii"];
                Control textbox = this.Controls["panel_question"].Controls["sub_panel_question"].Controls["txtbox"];

                string files = "";

                foreach (Control control in this.Controls["panel_question"].Controls["sub_panel_question"].Controls["flp_files"].Controls)
                {
                    string token_file = await _class.UploadFileAsync(control.Tag.ToString());
                    if (token_file != null)
                        files += (token_file + ";");
                }

                string url = "https://schoolsync.nnmadalin.me/api/post.php";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("sql", string.Format("insert into invataunit(token, token_user, created, category, question, answers, files) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", 
                    token, login_signin.login.accounts_user["token"], login_signin.login.accounts_user["username"], ((ComboBox)combobox).SelectedItem, textbox.Text.Trim(), "{}", files));
                
                dynamic task = await _class.PostRequestAsync(url, data);
                if(task["message"] == "insert success")
                {
                    var frm = new notification.success();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    
                    notification.success.message = "Intrebare salvata cu succes!";
                    frm.BringToFront();
                    foreach (Control control in this.Controls)
                    {
                        if (control.Name == "panel_question")
                        {
                            this.Controls.Remove(control);
                            load_question_panel();
                            break;
                        }
                    }
                }
                else
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Ceva nu a functionat bine!";
                    frm.BringToFront();
                }
            }
        }

        private void guna2Button20_Click(object sender, EventArgs e)
        {
            Panel panel_question = new Panel()
            {
                Size = new Size(1192, 690),
                BackColor = Color.FromArgb(125, 96, 186, 247),
                Name = "panel_question"
            };

            Guna.UI2.WinForms.Guna2Panel sub_panel_question = new Guna.UI2.WinForms.Guna2Panel()
            {
                Size = new Size(600, 500),
                FillColor = Color.White,
                UseTransparentBackground = true,
                BorderRadius = 20,
                Location = new Point((1192 - 600) / 2, (690 - 500) / 2),
                Name = "sub_panel_question"
            };

            Label title = new Label()
            {
                Text = "Întreabă ce nu ştii să faci din temă",
                Width = 300,
                Location = new Point(15, 15),
                Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold)
            };

            Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button()
            {
                Image = SchoolSync.Properties.Resources.close_FILL1_wght700_GRAD0_opsz48,
                ImageAlign = HorizontalAlignment.Center,
                Size = new Size(25, 25),
                ImageSize = new Size(25, 25),
                FillColor = Color.Transparent,
                UseTransparentBackground = true,
                Location = new Point(550, 15)
            };
            btn.Click += close_add_question;

            Guna.UI2.WinForms.Guna2TextBox txtbox = new Guna.UI2.WinForms.Guna2TextBox()
            {
                FillColor = Color.FromArgb(235, 242, 247),
                BorderRadius = 20,
                PlaceholderForeColor = Color.Gray,
                ForeColor = Color.Black,
                Size = new Size(500, 200),
                Location = new Point(10, 50),
                Multiline = true,
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Regular),
                PlaceholderText = "Scrie întrebarea aici!",
                Name = "txtbox"
            };
            txtbox.TextChanged += panel_question_textbox;

            FlowLayoutPanel flp = new FlowLayoutPanel()
            {
                Size = new Size(540, 50),
                BackColor = Color.FromArgb(242, 242, 242),
                Location = new Point(40, 300),
                Name = "flp_files"
            };

            Guna.UI2.WinForms.Guna2Button add_file = new Guna.UI2.WinForms.Guna2Button()
            {
                Image = SchoolSync.Properties.Resources.attach_file_FILL1_wght700_GRAD0_opsz48,
                ImageAlign = HorizontalAlignment.Center,
                Size = new Size(25, 25),
                ImageSize = new Size(25, 25),
                FillColor = Color.Transparent,
                UseTransparentBackground = true,
                Location = new Point(10, 310),               
            };

            add_file.Click += open_file_dialog;

            Guna.UI2.WinForms.Guna2ComboBox cmb = new Guna.UI2.WinForms.Guna2ComboBox()
            {
                BorderRadius = 15,
                Width = 200,
                Height = 50,
                Location = new Point(15, 365),
                Items = { "Limba română", "Matematică", "Istorie", "Chimie", "Biologie", "Fizică", "Geografie",
                           "Studii sociale", "Informatică", "Engleza", "Franceza", "Alte limbi", "Ed. tehnologică", "Arte", "Ed. muzicală"},
                FillColor = Color.FromArgb(235, 242, 247),    
                Name = "combobox_materii"
            };
            cmb.SelectedIndex = 0;

            Guna.UI2.WinForms.Guna2Button btn_finish = new Guna.UI2.WinForms.Guna2Button()
            {
                Size = new Size(170, 40),
                FillColor = Color.Black,
                ForeColor = Color.White,
                BorderRadius = 15, 
                Text = "PUNE INTREBAREA TA",
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                UseTransparentBackground = true,
                Location = new Point(10, 430)
            };
            btn_finish.Click += send_question;

            sub_panel_question.Controls.Add(btn);
            sub_panel_question.Controls.Add(title);
            sub_panel_question.Controls.Add(txtbox);
            sub_panel_question.Controls.Add(flp);
            sub_panel_question.Controls.Add(add_file);
            sub_panel_question.Controls.Add(cmb);
            sub_panel_question.Controls.Add(btn_finish);
            panel_question.Controls.Add(sub_panel_question);
            this.Controls.Add(panel_question);
            panel_question.BringToFront();


        }

        string sort = "";

        private async void load_question_with_answer(object sender, EventArgs e)
        {
            multiple_class _Class = new multiple_class();

            string token = (sender as Guna.UI2.WinForms.Guna2Button).Tag.ToString();
            Console.WriteLine(token);
            FlowLayoutPanel panel_question_with_answer = new FlowLayoutPanel()
            {
                Size = new Size(1192, 690),
                Padding = new Padding(0,0, 0, 20),
                AutoScroll = true,
                BackColor = Color.White,
                Name = "panel_question_with_answer"
            };

            Guna.UI2.WinForms.Guna2CircleButton btn = new Guna.UI2.WinForms.Guna2CircleButton()
            {
                Image = SchoolSync.Properties.Resources.reply_FILL1_wght700_GRAD0_opsz48,
                Size = new Size(45, 45),
                ImageAlign = HorizontalAlignment.Center,
                ImageSize = new Size(30, 30),
                UseTransparentBackground = true,
                FillColor = Color.White,
                BorderColor = Color.Black,
                BorderThickness = 2,
                Location = new Point(10, 10),
                Cursor = Cursors.Hand
            };
            btn.Click += close_question_with_answer;

            //panel + elemente intrebare
            Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
            {
                MaximumSize = new Size(1150, 0),
                MinimumSize = new Size(1150, 0),
                Padding = new Padding(0, 0, 0, 30),
                AutoSize = true,
                BorderColor = Color.FromArgb(96, 211, 153),
                BorderRadius = 15,
                BorderThickness = 4
            };

            Guna.UI2.WinForms.Guna2CirclePictureBox cpb = new Guna.UI2.WinForms.Guna2CirclePictureBox()
            {
                Size = new Size(50, 50),
                ErrorImage = SchoolSync.Properties.Resources.standard_avatar,
                InitialImage = SchoolSync.Properties.Resources.standard_avatar,
                SizeMode = PictureBoxSizeMode.StretchImage,
                UseTransparentBackground = true,
                Location = new Point(30, 20)
            };
            cpb.Image = await _Class.IncarcaImagineAsync("https://schoolsync.nnmadalin.me/api/getfile.php?token=userfoto_"+ login_signin.login.accounts_user["token"] + ".png");

            Label lbl_name = new Label()
            {
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                Location = new Point(82, 25),
                Text = "",
                AutoSize = true
            };
            Label lbl_category_time = new Label()
            {
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(145, 145, 145),
                Location = new Point(82, 45),
                Text = "",
                AutoSize = true
            };
            Label lbl_question= new Label()
            {
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(10, 80),
                Text = "",
                AutoSize = true,
                MaximumSize = new Size(1110, 0)
            };
            Label lbl_panel_before= new Label()
            {
                Text = "Fisiere atasate:",
                AutoSize = true, 
                Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
            };            

            //Extrage date din API, dupa token
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("select * from invataunit where token = '{0}'", token));

            FlowLayoutPanel flp = new FlowLayoutPanel()
            {
                Size = new Size(1110, 185),
            };
            Label lbl_panel_file_not = new Label()
            {
                AutoSize = true,
                MinimumSize = new Size(400, 40),
                Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold),
                Text = "Nu sunt fisiere atasate!",
                TextAlign = ContentAlignment.MiddleCenter,
            };
           
            

            dynamic task = await _Class.PostRequestAsync(url, data);

            if (task["message"] == "success")
            {
                lbl_name.Text = task["0"]["created"];
                string date = task["0"]["data"]; DateTime dt = Convert.ToDateTime(date);
                lbl_category_time.Text = task["0"]["category"] + " • "
                        + dt.Day + "/" + dt.Month + "/" + dt.Year + " " + Convert.ToDateTime(date).ToShortTimeString(); ;
                lbl_question.Text = task["0"]["question"];

                if(task["0"]["files"] == "")
                {
                    flp.Controls.Add(lbl_panel_file_not);
                }
                else
                {
                    string row = task["0"]["files"].ToString();
                    string[] split = row.Split(';');
                    for (int i = 0; i < split.Length - 1; i++)
                    {
                        string[] splitplit = split[i].Split('.');

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

                        if(splitplit[0].Length >= 10)
                        {
                            lbl_panel_file.Text = splitplit[0].Substring(0, 10) + "." + splitplit[1];
                        }
                        else
                            lbl_panel_file.Text = split[i];
                        if (splitplit[1] == "jpg" || splitplit[1] == "jpeg" || splitplit[1] == "png" || splitplit[1] == "svg" || splitplit[1] == "webp" || splitplit[1] == "bmp")
                        {
                            gcp.Image = SchoolSync.Properties.Resources.image_FILL1_wght700_GRAD0_opsz48;
                        }
                        else
                            gcp.Image = SchoolSync.Properties.Resources.description_FILL1_wght700_GRAD0_opsz48;

                        flp_files_panel.Controls.Add(panel_file_btn);
                    }
                }
            }

            pnl.Controls.Add(cpb);
            pnl.Controls.Add(lbl_name);
            pnl.Controls.Add(lbl_category_time);
            pnl.Controls.Add(lbl_question);



            panel_question_with_answer.Controls.Add(btn);
            panel_question_with_answer.Controls.Add(pnl);
            panel_question_with_answer.Controls.Add(lbl_panel_before);
            panel_question_with_answer.Controls.Add(flp);
            this.Controls.Add(panel_question_with_answer);
            panel_question_with_answer.BringToFront();
        }

        void load_question_information_add()
        {
            Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
            {
                Size = new Size(925, 137),
                BorderColor = Color.FromArgb(96, 211, 153),
                BorderRadius = 15,
                BorderThickness = 5
            };
            Label lbl = new Label()
            {
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                Location = new Point(28, 9),
                Text = "InvataUnit • Raspuns",
                AutoSize = true
            };
            Label lbl_question = new Label()
            {
                Font = new Font("Segoe UI Black", 35, FontStyle.Regular),
                Location = new Point(21, 19),
                AutoSize = true,
                Text = "Ce vrei să ştii?"
            };
            Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button()
            {
                Text = "Întreabă",
                FillColor = Color.FromArgb(255, 199, 191),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                BorderRadius = 15,
                Size = new Size(187, 36),
                Location = new Point(32, 84)
            };
            btn.Click += guna2Button20_Click;
            pnl.Controls.Add(lbl);
            pnl.Controls.Add(lbl_question);
            pnl.Controls.Add(btn);
            this.Controls["flowLayoutPanel1"].Controls.Add(pnl);
        }

        async void load_question_panel()
        {
            this.Controls["flowLayoutPanel1"].Controls.Clear();

            sort = sort.Trim();
            string answer_sort = guna2ComboBox2.SelectedItem.ToString();

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);

            if (sort.Trim() == "" || sort == "Toate materiile")
            {
                if (answer_sort == "" || answer_sort == "Toate")
                {
                    data.Add("sql", string.Format("select * from invataunit order by data DESC"));

                    if (guna2Button17.BorderThickness == 2)
                        data["sql"] += " where created = '" + login_signin.login.accounts_user["username"] + "' orber by data DESC";
                }
                else
                {
                    if (answer_sort == "Fără răspuns")
                        data.Add("sql", string.Format("select * from invataunit where LENGTH(answers) = 2 order by data DESC"));
                    else
                        data.Add("sql", string.Format("select * from invataunit where LENGTH(answers) > 2 order by data DESC"));
                    if (guna2Button17.BorderThickness == 2)
                        data["sql"] += " and created = '" + login_signin.login.accounts_user["username"] + "' order by data DESC";
                }
            }
            else
            {
                if (answer_sort == "" || answer_sort == "Toate")
                    data.Add("sql", string.Format("select * from invataunit where category = '{0}' order by data DESC", sort));
                else
                {
                    if (answer_sort == "Fără răspuns")
                        data.Add("sql", string.Format("select * from invataunit where category = '{0}' and LENGTH(answers) = 2 order by data DESC", sort));
                    else
                        data.Add("sql", string.Format("select * from invataunit where category = '{0}' and LENGTH(answers) > 2 order by data DESC", sort));
                }
                if (guna2Button17.ShadowDecoration.Enabled == true)
                    data["sql"] += " and created = '" + login_signin.login.accounts_user["username"] + "' order by data DESC";
            }


            dynamic task = await _class.PostRequestAsync(url, data);
            JObject jb = task;
            if (guna2Button17.BorderThickness != 2)
                load_question_information_add();

            if(task["message"] == "success")
            {
                for(int i = 0; i < 15 && i < jb.Count - 1; i++)
                {
                    Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        Size = new Size(925, 137),
                        BorderColor = Color.FromArgb(96, 211, 153),
                        BorderRadius = 15,
                        BorderThickness = 2
                    };
                    Label lbl = new Label()
                    {
                        Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                        Location = new Point(28, 20),
                        Text = "",
                        AutoSize = true
                    };
                    Label lbl_question = new Label()
                    {
                        Font = new Font("Segoe UI Semibold", 20, FontStyle.Regular),
                        Location = new Point(25, 39),
                        AutoSize = true,
                        Text = ""
                    };
                    Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button()
                    {
                        Text = "Răspunde",
                        Tag = "",
                        FillColor = Color.Transparent,
                        ForeColor = Color.Black,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        Cursor = Cursors.Hand,
                        BorderRadius = 15,
                        BorderColor = Color.Black,
                        BorderThickness = 2,
                        Size = new Size(146, 36),
                        Location = new Point(740, 88)
                    };                    

                    string date = task[i.ToString()]["data"]; DateTime dt = Convert.ToDateTime(date);

                    lbl.Text = task[i.ToString()]["created"] + " • " + task[i.ToString()]["category"] + " • " 
                        + dt.Day + "/" + dt.Month  + "/" + dt.Year + " " + Convert.ToDateTime(date).ToShortTimeString();
                    lbl_question.Text = task[i.ToString()]["question"];

                    if (lbl_question.Text.Length > 55)
                    {
                        lbl_question.Text = lbl_question.Text.Substring(0, 55) + "...";
                    }

                    btn.Tag = task[i.ToString()]["token"];
                    btn.Click += load_question_with_answer;

                    pnl.Controls.Add(lbl);
                    pnl.Controls.Add(lbl_question);
                    pnl.Controls.Add(btn);
                    this.Controls["flowLayoutPanel1"].Controls.Add(pnl);

                }
            }

           
        }

        private void invataunit_Load(object sender, EventArgs e)
        {
            load_question_panel();
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_question_panel();
        }

        private void guna2Button19_Click(object sender, EventArgs e)
        {
            guna2Button19.BorderThickness = 2;
            guna2Button17.BorderThickness = 0;
            guna2Button18.BorderThickness = 0;
            load_question_panel();
        }

        private void guna2Button17_Click(object sender, EventArgs e)
        {
            guna2Button19.BorderThickness = 0;
            guna2Button17.BorderThickness = 2;
            guna2Button18.BorderThickness = 0;
            load_question_panel();
        }

        private void guna2Button18_Click(object sender, EventArgs e)
        {
            guna2Button19.BorderThickness = 0;
            guna2Button17.BorderThickness = 0;
            guna2Button18.BorderThickness = 2;
            load_question_panel();
        }
    }
}
