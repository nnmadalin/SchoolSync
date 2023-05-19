using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Dynamic;
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

        private void buton_sortare_materie(object sender, EventArgs e)
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
            load_intrebari_panel();
        }

        private void inchide_panel_intrebare(object sender, EventArgs e)
        {
            foreach(Control control in this.Controls)
            {
                if(control.Name == "panel_question")
                {
                    this.Controls.Remove(control);
                    break;
                }
            }
            if (guna2Button19.BorderThickness == 2)
            {
                page_now = "home";
            }
            if (guna2Button18.BorderThickness == 2)
            {
                page_now = "question_answer";
            }
        }

        private void inchide_panel_intrebare_cu_raspuns(object sender, EventArgs e)
        {
            foreach(Control control in this.Controls)
            {
                if(control.Name == "panel_question_with_answer")
                {
                    this.Controls.Remove(control);
                    break;
                }
            }
            
            if (guna2Button19.BorderThickness == 2)
            {
                load_intrebari_panel();
            }
            if (guna2Button18.BorderThickness == 2)
            {
                load_favorite_panel();
            }

        }

        private void file_dialog(object sender, EventArgs e)
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

        private void buton_home_selectat(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2TextBox btn = sender as Guna.UI2.WinForms.Guna2TextBox;
            btn.BorderColor = Color.FromArgb(213, 218, 223);
        }
        
        private async void trimite_intrebare_api(object sender, EventArgs e)
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
                            load_intrebari_panel();
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
        
        private async void trimite_raspuns_api(object sender, EventArgs e)
        {
            if (this.Controls["panel_question"].Controls["sub_panel_question"].Controls["txtbox"].Text.Trim() == "")
            {
                ((Guna.UI2.WinForms.Guna2TextBox)this.Controls["panel_question"].Controls["sub_panel_question"].Controls["txtbox"]).BorderColor = Color.Red;
            }
            else
            {
                //TOTUL ESTE BINE
                schoolsync.show_loading();

                ((Guna.UI2.WinForms.Guna2TextBox)this.Controls["panel_question"].Controls["sub_panel_question"].Controls["txtbox"]).BorderColor = Color.FromArgb(213, 218, 223);

                multiple_class _class = new multiple_class();
                string token = ((Guna.UI2.WinForms.Guna2Button)sender).Tag.ToString();

                Control textbox = this.Controls["panel_question"].Controls["sub_panel_question"].Controls["txtbox"];

                string files = "";

                foreach (Control control in this.Controls["panel_question"].Controls["sub_panel_question"].Controls["flp_files"].Controls)
                {
                    string token_file = await _class.UploadFileAsync(control.Tag.ToString());
                    if (token_file != null)
                        files += (token_file + ";");
                }

                //get json file

                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("sql", string.Format("select * from invataunit where token = '{0}'", token));

                dynamic task = await _class.PostRequestAsync(url, data);
                
                if (task["message"] == "success") 
                {
                    int x = 0;
                    dynamic mini = JsonConvert.DeserializeObject((string)task["0"]["answers"]);
                    JObject jbo = JObject.FromObject(mini);
                    JObject sub_json = new JObject();

                    sub_json.Add("username", login_signin.login.accounts_user["username"]);
                    sub_json.Add("data", DateTime.Now.ToString());
                    sub_json.Add("answer", textbox.Text.Trim());
                    sub_json.Add("files", files);
                    jbo.Add(jbo.Count.ToString(), sub_json);

                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("sql", string.Format("update invataunit set answers = '{0}' where token = '{1}'", JsonConvert.SerializeObject(jbo), token));
                    task = await _class.PostRequestAsync(url, data);
                    if (task["message"] == "update success")
                    {
                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);

                        notification.success.message = "Raspuns salvata cu succes!";
                        frm.BringToFront();
                        foreach (Control control in this.Controls)
                        {
                            if (control.Name == "panel_question")
                            {
                                this.Controls.Remove(control);                                
                                break;
                            }
                        }
                        load_intrebare_cu_raspunsuri();
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
        }

        private void load_adaugare_intrebare_panel(object sender, EventArgs e)
        {
            page_now = "add_question";
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
            btn.Click += inchide_panel_intrebare;

            Guna.UI2.WinForms.Guna2TextBox txtbox = new Guna.UI2.WinForms.Guna2TextBox()
            {
                FillColor = Color.FromArgb(235, 242, 247),
                BorderRadius = 20,
                PlaceholderForeColor = Color.Gray,
                ForeColor = Color.Black,
                Size = new Size(500, 200),
                Location = new Point(10, 50),
                Multiline = true,
                MaxLength = 2000,
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Regular),
                PlaceholderText = "Scrie întrebarea aici!",
                Name = "txtbox"
            };
            txtbox.TextChanged += buton_home_selectat;

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

            add_file.Click += file_dialog;

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
            btn_finish.Click += trimite_intrebare_api;

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

        private void load_adaugare_raspuns_panel(object sender, EventArgs e)
        {
            page_now = "add_answer";
            Panel panel_question = new Panel()
            {
                Size = new Size(1192, 690),
                BackColor = Color.FromArgb(125, 96, 186, 247),
                Name = "panel_question"
            };

            Guna.UI2.WinForms.Guna2Panel sub_panel_question = new Guna.UI2.WinForms.Guna2Panel()
            {
                Size = new Size(600, 420),
                FillColor = Color.White,
                UseTransparentBackground = true,
                BorderRadius = 20,
                Location = new Point((1192 - 600) / 2, (690 - 500) / 2),
                Name = "sub_panel_question"
            };

            Label title = new Label()
            {
                Text = "Raspunde la intrebare!",
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
                Location = new Point(550, 15),
                Cursor = Cursors.Hand
            };
            btn.Click += inchide_panel_intrebare;

            Guna.UI2.WinForms.Guna2TextBox txtbox = new Guna.UI2.WinForms.Guna2TextBox()
            {
                FillColor = Color.FromArgb(235, 242, 247),
                BorderRadius = 20,
                PlaceholderForeColor = Color.Gray,
                ForeColor = Color.Black,
                Size = new Size(500, 200),
                Location = new Point(10, 50),
                MaxLength = 2000,
                Multiline = true,
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Regular),
                PlaceholderText = "Scrie raspunsul aici!",
                Name = "txtbox"
            };
            txtbox.TextChanged += buton_home_selectat;

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
                Cursor = Cursors.Hand
            };

            add_file.Click += file_dialog;


            Guna.UI2.WinForms.Guna2Button btn_finish = new Guna.UI2.WinForms.Guna2Button()
            {
                Size = new Size(170, 40),
                FillColor = Color.Black,
                ForeColor = Color.White,
                BorderRadius = 15,
                Text = "Raspunde!",
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                UseTransparentBackground = true,
                Location = new Point(10, 370),
                Cursor = Cursors.Hand,
                Tag = token_question
            };
            btn_finish.Click += trimite_raspuns_api;

            sub_panel_question.Controls.Add(btn);
            sub_panel_question.Controls.Add(title);
            sub_panel_question.Controls.Add(txtbox);
            sub_panel_question.Controls.Add(flp);
            sub_panel_question.Controls.Add(add_file);
            sub_panel_question.Controls.Add(btn_finish);
            panel_question.Controls.Add(sub_panel_question);
            this.Controls.Add(panel_question);
            panel_question.BringToFront();

        }

        string sort = "", token_question, page_now = "home", token_first_question = "", number_answer = "";
        string intrebare_value = "", fisiere_value = "", combobox_value = "";

        private void deschide_fisier_buton(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            System.Diagnostics.Process.Start(@"https://schoolsync.nnmadalin.me/api/getfile.php?token=" + btn.Tag.ToString());
        }

        private void deschide_fisier_chip(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Chip;
            System.Diagnostics.Process.Start(@"https://schoolsync.nnmadalin.me/api/getfile.php?token=" + btn.Tag.ToString());
        }

        private async void adauga_sterge_favorit(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;

            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("select * from accounts where token = '{0}'", login_signin.login.accounts_user["token"]));

            bool ok = false;
            
            if (btn.Tag == "1")
            {
                btn.Image = SchoolSync.Properties.Resources.favorite_FILL0_wght700_GRAD0_opsz48;
                btn.Tag = "0";
                ok = true;
            }
            else
            {
                btn.Image = SchoolSync.Properties.Resources.favorite_FILL1_wght700_GRAD0_opsz48;
                btn.Tag = "1";
            }

            multiple_class _Class = new multiple_class();
            dynamic task = await _Class.PostRequestAsync(url, data);

            if(task["message"] == "success")
            {
                string str = task["0"]["favorite_invataunit"];
                if(ok == false)
                {
                    str += (token_question + ";");
                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("sql", string.Format("update accounts set favorite_invataunit = '{0}'", str));
                    task = await _Class.PostRequestAsync(url, data);
                    if(task["message"] == "update success")
                    {
                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);

                        notification.success.message = "Adaugat la favorite cu succes!";
                        frm.BringToFront();
                    }
                    else
                    {
                        var frm = new notification.error();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.error.message = "Ceva nu e mers bine!";
                        frm.BringToFront();
                    }
                }
                else
                {
                    string[] split = str.Split(';');
                    string fns = "";
                    for(int i = 0; i < split.Length - 1; i++)
                    {
                        if(split[i] != token_question)
                        {
                            fns += (split[i] + ";");
                        }
                    }
                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("sql", string.Format("update accounts set favorite_invataunit = '{0}'", fns));
                    task = await _Class.PostRequestAsync(url, data);
                    if (task["message"] == "update success")
                    {
                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);

                        notification.success.message = "Scos de la favorite cu succes!";
                        frm.BringToFront();
                    }
                    else
                    {
                        var frm = new notification.error();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.error.message = "Ceva nu e mers bine!";
                        frm.BringToFront();
                    }
                }
            }
        }

        private async void sterge_intrebare(object sender, EventArgs e)
        {
            
            string token = ((Guna.UI2.WinForms.Guna2CircleButton)sender).Tag.ToString();
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
            guna2MessageDialog1.Caption = "Sterge intrebare!";
            guna2MessageDialog1.Text = "Esti sigur ca vrei sa stergi intrebarea?";
            DialogResult dr = guna2MessageDialog1.Show();
            if(dr == DialogResult.Yes)
            {
                timer2.Enabled = false;
                string url = "https://schoolsync.nnmadalin.me/api/delete.php";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("sql", string.Format("delete from invataunit where token = '{0}'", token));

                multiple_class _Class = new multiple_class();
                dynamic task = await _Class.PostRequestAsync(url, data);
                string token_app = schoolsync.token;

                if (task["message"] == "delete success")
                {
                    string[] split_1 = fisiere_value.Split(';');
                    for (int i = 0; i < split_1.Length - 1; i++)
                    {
                        url = "https://schoolsync.nnmadalin.me/api/delete_file.php";
                        data = new Dictionary<string, string>();
                        data.Add("token", token_app);
                        data.Add("file", split_1[i]);
                        task = await _Class.PostRequestAsync(url, data);
                    }

                    var frm = new notification.success();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);

                    notification.success.message = "Intrebare stearsa cu succes!";
                    frm.BringToFront();                    
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
                foreach (Control control in this.Controls)
                {
                    if (control.Name == "panel_question_with_answer")
                    {
                        this.Controls.Remove(control);
                        break;
                    }
                }

                if (guna2Button19.BorderThickness == 2)
                {
                    load_intrebari_panel();
                }
                if (guna2Button18.BorderThickness == 2)
                {
                    load_favorite_panel();
                }
            }
        }
        
        private async void editeaza_intrebare(object sender, EventArgs e)
        {
            page_now = "edit_question";
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
                Text = "Editeaza intrebarea!",
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
            btn.Click += inchide_panel_intrebare;

            Guna.UI2.WinForms.Guna2TextBox txtbox = new Guna.UI2.WinForms.Guna2TextBox()
            {
                FillColor = Color.FromArgb(235, 242, 247),
                BorderRadius = 20,
                PlaceholderForeColor = Color.Gray,
                ForeColor = Color.Black,
                Size = new Size(500, 200),
                Location = new Point(10, 50),
                Multiline = true,
                MaxLength = 2000,
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Regular),
                PlaceholderText = "Scrie întrebarea aici!",
                Name = "txtbox"
            };
            txtbox.TextChanged += buton_home_selectat;

            FlowLayoutPanel flp = new FlowLayoutPanel()
            {
                Size = new Size(540, 50),
                BackColor = Color.FromArgb(242, 242, 242),
                Location = new Point(40, 300),
                Name = "flp_files"
            };

            string[] split = fisiere_value.Split(';');
            for(int i = 0; i < split.Length - 1; i++)
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
                    Text = split[i].Substring(0, 8) + "...",
                    Tag = split[i].ToString()
                };

                flp.Controls.Add(guna2Chip);
            }

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

            add_file.Click += file_dialog;

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
                Text = "Editeaza intrebarea!",
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                UseTransparentBackground = true,
                Location = new Point(10, 430),
                Cursor = Cursors.Hand
            };
            btn_finish.Click += api_editare_intrebare;

            txtbox.Text = intrebare_value;
            cmb.SelectedIndex = cmb.Items.IndexOf(combobox_value);

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
        
        private async void api_editare_intrebare(object sender, EventArgs e)
        {
            if (this.Controls["panel_question"].Controls["sub_panel_question"].Controls["txtbox"].Text.Trim() == "")
            {
                ((Guna.UI2.WinForms.Guna2TextBox)this.Controls["panel_question"].Controls["sub_panel_question"].Controls["txtbox"]).BorderColor = Color.Red;
            }
            else
            {
                string token = token_question;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Caption = "Editeaza intrebare!";
                guna2MessageDialog1.Text = "Esti sigur ca vrei sa editezi intrebarea?";
                DialogResult dr = guna2MessageDialog1.Show();
                if (dr == DialogResult.Yes)
                {
                    //TOTUL ESTE BINE
                    schoolsync.show_loading();

                    ((Guna.UI2.WinForms.Guna2TextBox)this.Controls["panel_question"].Controls["sub_panel_question"].Controls["txtbox"]).BorderColor = Color.FromArgb(213, 218, 223);

                    multiple_class _class = new multiple_class();

                    Control combobox = this.Controls["panel_question"].Controls["sub_panel_question"].Controls["combobox_materii"];
                    Control textbox = this.Controls["panel_question"].Controls["sub_panel_question"].Controls["txtbox"];                       

                    string url;
                    Dictionary<string, string> data;
                    dynamic task;

                    string token_app = schoolsync.token;
                    string files = "";
                    foreach (Control control in this.Controls["panel_question"].Controls["sub_panel_question"].Controls["flp_files"].Controls)
                    {
                        string token_file = await _class.UploadFileAsync(control.Tag.ToString());
                        if (token_file != null)
                            files += (token_file + ";");
                    }

                    string[] split_1 = fisiere_value.Split(';');
                    for (int i = 0; i < split_1.Length - 1; i++)
                    {
                        url = "https://schoolsync.nnmadalin.me/api/delete_file.php";
                        data = new Dictionary<string, string>();
                        data.Add("token", token_app);
                        data.Add("file", split_1[i]);
                        task = await _class.PostRequestAsync_norefresh(url, data);
                    }

                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("sql", string.Format("update invataunit set category = '{0}', question = '{1}', files = '{2}' where token = '{3}'",
                        ((ComboBox)combobox).SelectedItem, textbox.Text.Trim(), files, token));

                    task = await _class.PostRequestAsync(url, data);
                    if (task["message"] == "update success")
                    {
                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);

                        notification.success.message = "Intrebare modificata cu succes!";
                        frm.BringToFront();
                        foreach (Control control in this.Controls)
                        {
                            if (control.Name == "panel_question")
                            {
                                this.Controls.Remove(control);
                                load_intrebari_panel();
                                break;
                            }
                        }
                        load_intrebare_cu_raspunsuri();
                    }
                    else
                    {
                        var frm = new notification.error();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.error.message = "Ceva nu a functionat bine, mai incearca!";
                        frm.BringToFront();
                    }
                }
            }           
        }

        
        async void load_intrebare_cu_raspunsuri()
        {
            page_now = "question_answer";
            multiple_class _Class = new multiple_class();

            this.Controls["panel_question_with_answer"].Controls.Clear();

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
            btn.Click += inchide_panel_intrebare_cu_raspuns;

            Guna.UI2.WinForms.Guna2CircleButton btn_edit = new Guna.UI2.WinForms.Guna2CircleButton()
            {
                Image = SchoolSync.Properties.Resources.edit_FILL1_wght700_GRAD0_opsz48,
                Size = new Size(35, 35),
                ImageAlign = HorizontalAlignment.Center,
                ImageSize = new Size(20, 20),
                UseTransparentBackground = true,
                FillColor = Color.White,
                BorderColor = Color.Black,
                Tag = token_question,
                BorderThickness = 2,
                Location = new Point(1060,  10),
                Cursor = Cursors.Hand
            };
            btn_edit.Click += editeaza_intrebare;

            Guna.UI2.WinForms.Guna2CircleButton btn_delete = new Guna.UI2.WinForms.Guna2CircleButton()
            {
                Image = SchoolSync.Properties.Resources.delete_FILL1_wght700_GRAD0_opsz48,
                Size = new Size(35, 35),
                ImageAlign = HorizontalAlignment.Center,
                ImageSize = new Size(20, 20),
                UseTransparentBackground = true,
                FillColor = Color.White,
                BorderColor = Color.Black,
                Tag = token_question,
                BorderThickness = 2,
                Location = new Point(1100, 10),
                Cursor = Cursors.Hand
            };
            btn_delete.Click += sterge_intrebare;


            Guna.UI2.WinForms.Guna2Button btn_add_favorite = new Guna.UI2.WinForms.Guna2Button()
            {
                Image = SchoolSync.Properties.Resources.favorite_FILL0_wght700_GRAD0_opsz48,
                Size = new Size(140, 45),
                ImageAlign = HorizontalAlignment.Left,
                ImageSize = new Size(30, 30),
                UseTransparentBackground = true,
                Name = "0",
                Tag = token_question,
                ForeColor = Color.Black,
                FillColor = Color.White,
                BorderColor = Color.Black,
                BorderThickness = 2,
                BorderRadius = 15,
                Location = new Point(10, 10),
                Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold),
                Text = "Favorite!",
                TextAlign = HorizontalAlignment.Right,
                Cursor = Cursors.Hand
            };

            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("select * from accounts where token = '{0}'", login_signin.login.accounts_user["token"]));

            dynamic task = await _Class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                string str = task["0"]["favorite_invataunit"];
                string[] split = str.Split(';');
                for(int i = 0; i < split.Length - 1; i++)
                {
                    if(split[i] == token_question)
                    {
                        btn_add_favorite.Image = SchoolSync.Properties.Resources.favorite_FILL1_wght700_GRAD0_opsz48;
                        btn_add_favorite.Tag = "1";
                        break;
                    }
                }
            }

            btn_add_favorite.Click += adauga_sterge_favorit;

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
            cpb.Image = await _Class.IncarcaImagineAsync("https://schoolsync.nnmadalin.me/api/getfile.php?token=userfoto_" + login_signin.login.accounts_user["token"] + ".png");

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
            Label lbl_question = new Label()
            {
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(10, 80),
                Text = "",
                AutoSize = true,
                MaximumSize = new Size(1110, 0)
            };
            Label lbl_panel_before = new Label()
            {
                Text = "Fisiere atasate:",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
            };

            //Extrage date din API, dupa token
            url = "https://schoolsync.nnmadalin.me/api/get.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("select * from invataunit where token = '{0}'", token_question));

            FlowLayoutPanel flp = new FlowLayoutPanel()
            {
                MinimumSize = new Size(1110, 0),
                AutoSize = true,
                Padding = new Padding(0, 0, 0, 20)
            };
            Label lbl_panel_file_not = new Label()
            {
                AutoSize = true,
                MinimumSize = new Size(400, 40),
                Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold),
                Text = "Nu sunt fisiere atasate!",
                TextAlign = ContentAlignment.MiddleCenter,
            };
            Guna.UI2.WinForms.Guna2Button btn_add_answer = new Guna.UI2.WinForms.Guna2Button()
            {
                FillColor = Color.Black,
                Size = new Size(150, 40),
                Text = "Adauga Mesaj",
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn_add_answer.Click += load_adaugare_raspuns_panel;

            task = await _Class.PostRequestAsync(url, data);

            this.Controls["panel_question_with_answer"].Controls.Add(btn);           
            this.Controls["panel_question_with_answer"].Controls.Add(btn_add_favorite);
            if (task["message"] == "success")
            {
                lbl_name.Text = task["0"]["created"];
                string date = task["0"]["data"]; DateTime dt = Convert.ToDateTime(date);
                lbl_category_time.Text = task["0"]["category"] + " • "
                        + dt.Day + "/" + dt.Month + "/" + dt.Year + " " + Convert.ToDateTime(date).ToShortTimeString(); ;
                lbl_question.Text = task["0"]["question"];

                intrebare_value = task["0"]["question"];
                combobox_value = task["0"]["category"];
                fisiere_value = task["0"]["files"];

                if (task["0"]["files"] == "")
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

                        if (splitplit[0].Length >= 10)
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
                        panel_file_btn.Tag = split[i];
                        panel_file_btn.Click += deschide_fisier_buton;
                    }
                }

                string dst = task["0"]["answers"];
                dynamic sub_task = JsonConvert.DeserializeObject(dst);
                JObject jb = JObject.FromObject(sub_task);

                if(task["0"]["created"] == login_signin.login.accounts_user["username"])
                {
                    pnl.Controls.Add(btn_edit);
                    pnl.Controls.Add(btn_delete);
                }

                pnl.Controls.Add(cpb);
                pnl.Controls.Add(lbl_name);
                pnl.Controls.Add(lbl_category_time);
                pnl.Controls.Add(lbl_question);
                this.Controls["panel_question_with_answer"].Controls.Add(pnl);
                this.Controls["panel_question_with_answer"].Controls.Add(lbl_panel_before);
                this.Controls["panel_question_with_answer"].Controls.Add(flp);
                this.Controls["panel_question_with_answer"].Controls.Add(btn_add_answer);

                number_answer = jb.Count.ToString();
                for (int i = jb.Count - 1; i >= 0; i--)
                {
                    Guna.UI2.WinForms.Guna2Panel pnl_answer = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        MaximumSize = new Size(1150, 0),
                        MinimumSize = new Size(1150, 0),
                        Padding = new Padding(0, 0, 0, 10),
                        AutoSize = true,
                        BorderColor = Color.FromArgb(96, 211, 153),
                        BorderRadius = 15,
                        BorderThickness = 1
                    };

                    Guna.UI2.WinForms.Guna2CirclePictureBox cpb_answer = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                    {
                        Size = new Size(50, 50),
                        ErrorImage = SchoolSync.Properties.Resources.standard_avatar,
                        InitialImage = SchoolSync.Properties.Resources.standard_avatar,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        UseTransparentBackground = true,
                        Location = new Point(30, 20)
                    };
                    cpb_answer.Image = await _Class.IncarcaImagineAsync("https://schoolsync.nnmadalin.me/api/getfile.php?token=userfoto_" + login_signin.login.accounts_user["token"] + ".png");

                    Label lbl_name_answer = new Label()
                    {
                        Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                        Location = new Point(82, 25),
                        Text = "",
                        AutoSize = true
                    };
                    lbl_name_answer.Text = jb[i.ToString()]["username"].ToString();
                    Label lbl_time_answer = new Label()
                    {
                        Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                        ForeColor = Color.FromArgb(145, 145, 145),
                        Location = new Point(82, 45),
                        Text = "",
                        AutoSize = true
                    };
                    lbl_time_answer.Text = jb[i.ToString()]["data"].ToString();
                    Label lbl_question_answer = new Label()
                    {
                        Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                        ForeColor = Color.Black,
                        Location = new Point(10, 80),
                        Text = "",
                        AutoSize = true,
                        MaximumSize = new Size(1110, 0)
                    };
                    lbl_question_answer.Text = jb[i.ToString()]["answer"].ToString();
                    FlowLayoutPanel flp_answer = new FlowLayoutPanel()
                    {
                        MaximumSize = new Size(1150, 0),
                        MinimumSize = new Size(1150, 0),
                        AutoSize = true,
                        Name = "flp_files"
                    };

                    string files = jb[i.ToString()]["files"].ToString();
                    string[] split = files.Split(';');

                    if (files != "")
                    {
                        for (int j = 0; j < split.Length - 1; j++)
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
                                Size = new Size(150, 40),
                                IsClosable = false,
                                Cursor = Cursors.Hand
                            };

                            string[] splitsplit = split[j].Split('.');
                            guna2Chip.Tag = split[j].ToString();
                            if (splitsplit[0].Length > 10)
                                guna2Chip.Text = splitsplit[0].Substring(0, 10) + "___." + splitsplit[1];
                            else
                                guna2Chip.Text = split[j];
                            guna2Chip.Click += deschide_fisier_chip;
                            flp_answer.Controls.Add(guna2Chip);
                        }
                    }


                    pnl_answer.Controls.Add(cpb_answer);
                    pnl_answer.Controls.Add(lbl_name_answer);
                    pnl_answer.Controls.Add(lbl_time_answer);
                    pnl_answer.Controls.Add(lbl_question_answer);
                    pnl_answer.Controls.Add(flp_answer);

                    this.Controls["panel_question_with_answer"].Controls.Add(pnl_answer);
                    this.Controls["panel_question_with_answer"].Controls.Add(flp_answer);
                }

                
            }
            
            timer2.Enabled = true;
        }
        
        private async void intrebare_cu_raspunsuri(object sender, EventArgs e)
        {
            page_now = "question_answer";
            string token = (sender as Guna.UI2.WinForms.Guna2Button).Tag.ToString();
            token_question = token;

            FlowLayoutPanel panel_question_with_answer = new FlowLayoutPanel()
            {
                Size = new Size(1192, 690),
                Padding = new Padding(0, 0, 0, 20),
                AutoScroll = true,
                BackColor = Color.White,
                Name = "panel_question_with_answer"
            };
            this.Controls.Add(panel_question_with_answer);
            panel_question_with_answer.BringToFront();

            load_intrebare_cu_raspunsuri();
        }

        void load_intrebari_panel_informare()
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
            btn.Click += load_adaugare_intrebare_panel;
            pnl.Controls.Add(lbl);
            pnl.Controls.Add(lbl_question);
            pnl.Controls.Add(btn);
            this.Controls["flowLayoutPanel1"].Controls.Add(pnl);
        }

        async void load_intrebari_panel()
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
                    data.Add("sql", string.Format("select * from invataunit"));
                    if (guna2Button17.BorderThickness == 2)
                        data["sql"] += " where created = '" + login_signin.login.accounts_user["username"] + "'";
                }
                else
                {
                    if (answer_sort == "Fără răspuns")
                        data.Add("sql", string.Format("select * from invataunit where LENGTH(answers) = 2"));
                    else
                        data.Add("sql", string.Format("select * from invataunit where LENGTH(answers) > 2"));
                    if (guna2Button17.BorderThickness == 2)
                        data["sql"] += " and created = '" + login_signin.login.accounts_user["username"] + "'";
                }
                data["sql"] += " order by data DESC";
            }
            else
            {
                if (answer_sort == "" || answer_sort == "Toate")
                    data.Add("sql", string.Format("select * from invataunit where category = '{0}'", sort));
                else
                {
                    if (answer_sort == "Fără răspuns")
                        data.Add("sql", string.Format("select * from invataunit where category = '{0}' and LENGTH(answers) = 2", sort));
                    else
                        data.Add("sql", string.Format("select * from invataunit where category = '{0}' and LENGTH(answers) > 2", sort));
                }
                if (guna2Button17.BorderThickness == 2)
                    data["sql"] += " and created = '" + login_signin.login.accounts_user["username"] + "'";
                data["sql"] += " order by data DESC";
            }

            dynamic task = await _class.PostRequestAsync(url, data);
            JObject jb = task;
            if (guna2Button17.BorderThickness != 2)
                load_intrebari_panel_informare();
            else
            {
                Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                {
                    Size = new Size(925, 90),
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
                    Text = "Intrebarile tale!"
                };
                pnl.Controls.Add(lbl);
                pnl.Controls.Add(lbl_question);
                this.Controls["flowLayoutPanel1"].Controls.Add(pnl);
            }

            if(task["message"] == "success")
            {
                if(jb.Count - 1 > 0)
                    token_first_question = task["0"]["token"];
                for (int i = 0; i < 15 && i < jb.Count - 1; i++)
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
                        MaximumSize = new Size(0, 45),
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
                    btn.Click += intrebare_cu_raspunsuri;

                    pnl.Controls.Add(lbl);
                    pnl.Controls.Add(lbl_question);
                    pnl.Controls.Add(btn);
                    this.Controls["flowLayoutPanel1"].Controls.Add(pnl);

                }
            }

            timer1.Enabled = true;
        }

        async void load_favorite_panel()
        {
            this.Controls["flowLayoutPanel1"].Controls.Clear();

            Guna.UI2.WinForms.Guna2Panel pnl_top = new Guna.UI2.WinForms.Guna2Panel()
            {
                Size = new Size(925, 90),
                BorderColor = Color.FromArgb(96, 211, 153),
                BorderRadius = 15,
                BorderThickness = 5
            };
            Label lbl_top = new Label()
            {
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                Location = new Point(28, 9),
                Text = "InvataUnit • Raspuns",
                AutoSize = true
            };
            Label lbl_question_top = new Label()
            {
                Font = new Font("Segoe UI Black", 35, FontStyle.Regular),
                Location = new Point(21, 19),
                AutoSize = true,
                Text = "Intrebarile tale favorite!"
            };
            pnl_top.Controls.Add(lbl_top);
            pnl_top.Controls.Add(lbl_question_top);
            this.Controls["flowLayoutPanel1"].Controls.Add(pnl_top);

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("select * from accounts where token = '{0}'", login_signin.login.accounts_user["token"]));

            dynamic task = await _class.PostRequestAsync(url, data);
            if(task["message"] == "success")
            {
                string favorite = task["0"]["favorite_invataunit"];
                string[] split = favorite.Split(';');

                for (int i = 0; i < split.Length - 1; i++)
                {
                    url = "https://schoolsync.nnmadalin.me/api/get.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("sql", string.Format("select * from invataunit where token = '{0}'", split[i]));
                    task = await _class.PostRequestAsync(url, data);
                    if (task["message"] == "success")
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
                            Text = "Vezi intrebare",
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

                        string date = task["0"]["data"]; DateTime dt = Convert.ToDateTime(date);

                        lbl.Text = task["0"]["created"] + " • " + task["0"]["category"] + " • "
                            + dt.Day + "/" + dt.Month + "/" + dt.Year + " " + Convert.ToDateTime(date).ToShortTimeString();
                        lbl_question.Text = task["0"]["question"];

                        if (lbl_question.Text.Length > 55)
                        {
                            lbl_question.Text = lbl_question.Text.Substring(0, 55) + "...";
                        }

                        btn.Tag = task["0"]["token"];
                        btn.Click += intrebare_cu_raspunsuri;

                        pnl.Controls.Add(lbl);
                        pnl.Controls.Add(lbl_question);
                        pnl.Controls.Add(btn);
                        this.Controls["flowLayoutPanel1"].Controls.Add(pnl);
                    }
                }
            }
        }

        private void invataunit_Load(object sender, EventArgs e)
        {
            load_intrebari_panel();
        }

        private void combobox1(object sender, EventArgs e)
        {
            load_intrebari_panel();
        }

        private void home(object sender, EventArgs e)
        {
            page_now = "home";
            guna2Button19.BorderThickness = 2;
            guna2Button17.BorderThickness = 0;
            guna2Button18.BorderThickness = 0;
            load_intrebari_panel();
        }

        private async void timer_refresh_home(object sender, EventArgs e)
        {
            if (page_now == "home")
            {
                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);

                sort = sort.Trim();
                string answer_sort = guna2ComboBox2.SelectedItem.ToString();

                if (sort.Trim() == "" || sort == "Toate materiile")
                {
                    if (answer_sort == "" || answer_sort == "Toate")
                    {
                        data.Add("sql", string.Format("select * from invataunit"));
                        if (guna2Button17.BorderThickness == 2)
                            data["sql"] += " where created = '" + login_signin.login.accounts_user["username"] + "'";
                    }
                    else
                    {
                        if (answer_sort == "Fără răspuns")
                            data.Add("sql", string.Format("select * from invataunit where LENGTH(answers) = 2"));
                        else
                            data.Add("sql", string.Format("select * from invataunit where LENGTH(answers) > 2"));
                        if (guna2Button17.BorderThickness == 2)
                            data["sql"] += " and created = '" + login_signin.login.accounts_user["username"] + "'";
                    }
                    data["sql"] += " order by data DESC";
                }
                else
                {
                    if (answer_sort == "" || answer_sort == "Toate")
                        data.Add("sql", string.Format("select * from invataunit where category = '{0}'", sort));
                    else
                    {
                        if (answer_sort == "Fără răspuns")
                            data.Add("sql", string.Format("select * from invataunit where category = '{0}' and LENGTH(answers) = 2", sort));
                        else
                            data.Add("sql", string.Format("select * from invataunit where category = '{0}' and LENGTH(answers) > 2", sort));
                    }
                    if (guna2Button17.BorderThickness == 2)
                        data["sql"] += " and created = '" + login_signin.login.accounts_user["username"] + "'";
                    data["sql"] += " order by data DESC";
                }

                dynamic task = await _class.PostRequestAsync_norefresh(url, data);
                JObject jb = task;
                if (jb.Count - 1 >= 1)
                {
                    if (task["0"]["token"] != token_first_question)
                    {
                        token_first_question = task["0"]["token"];
                        schoolsync.show_loading();
                        load_intrebari_panel();
                        schoolsync.hide_loading();
                    }
                }
            }
            else
                timer1.Enabled = false;

        }

        private async void timer_refresh_intrebare(object sender, EventArgs e)
        {
            if (page_now == "question_answer")
            {
                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("sql", string.Format("select * from invataunit where token = '{0}' order by data DESC", token_question));
                dynamic task = await _class.PostRequestAsync_norefresh(url, data);

                string dst = task["0"]["answers"];
                dynamic sub_task = JsonConvert.DeserializeObject(dst);
                JObject jb = JObject.FromObject(sub_task);

                if (number_answer != jb.Count.ToString())
                {
                    number_answer = jb.Count.ToString();
                    schoolsync.show_loading();
                    load_intrebare_cu_raspunsuri();
                    schoolsync.hide_loading();
                }
            }
            else
                timer2.Enabled = false;
        }

        private void intrebarile_tale(object sender, EventArgs e)
        {
            page_now = "your_question";
            guna2Button19.BorderThickness = 0;
            guna2Button17.BorderThickness = 2;
            guna2Button18.BorderThickness = 0;
            load_intrebari_panel();
        }

        private void favorite(object sender, EventArgs e)
        {
            page_now = "favorite";
            guna2Button19.BorderThickness = 0;
            guna2Button17.BorderThickness = 0;
            guna2Button18.BorderThickness = 2;
            load_favorite_panel();
        }
    }
}
