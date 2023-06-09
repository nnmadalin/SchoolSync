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
    public partial class InvataUnit : UserControl
    {
        public InvataUnit()
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
            try
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
                                Tag = opf.FileName.ToString()
                            };

                            string fnm = Path.GetFileName(opf.FileName);
                            if (fnm.Length >= 9)
                                guna2Chip.Text = fnm.Substring(0, 9) + "...";
                            else
                                guna2Chip.Text = fnm;

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
            catch
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Ceva nu a mers bine!";
                frm.BringToFront();
            }
        }

        private void buton_home_selectat(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2TextBox btn = sender as Guna.UI2.WinForms.Guna2TextBox;
            btn.BorderColor = Color.FromArgb(213, 218, 223);
        }       
        
        private void load_adaugare_intrebare_panel(object sender, EventArgs e)
        {
            navbar_home.use = false;
            navbar_home.page = "InvataUnit_adauga";
        }


        string sort = "", token_question, page_now = "home", token_first_question = "", number_answer = "";
        string intrebare_value = "", fisiere_value = "", combobox_value = "";


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
                    string[] split_1 = fisiere_value.Split(';');

                    foreach (Control control in this.Controls["panel_question"].Controls["sub_panel_question"].Controls["flp_files"].Controls)
                    {
                        bool ok = false;
                        for(int i = 0; i < split_1.Length - 1; i++)
                        {
                            if(split_1[i] == control.Tag.ToString())
                            {
                                split_1[i] = "-1";
                                ok = true;
                                break;
                            }
                        }
                        if(ok == true)
                        {
                            files += (control.Tag.ToString() + ";");
                        }
                        else
                        {
                            string token_file = await _class.UploadFileAsync(control.Tag.ToString());
                            if (token_file != null)
                                files += (token_file + ";");
                        }

                        
                    }

                    
                    for (int i = 0; i < split_1.Length - 1; i++)
                    {
                        if (split_1[i] != "-1")
                        {
                            url = "https://schoolsync.nnmadalin.me/api/delete_file.php";
                            data = new Dictionary<string, string>();
                            data.Add("token", token_app);
                            data.Add("file", split_1[i]);
                            task = await _class.PostRequestAsync_norefresh(url, data);
                        }
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

        private async void load_profil(object sender, EventArgs e)
        {
            Label lbl = (sender as Label);
            var frm = new pages.Profil();
            pages.Profil.token = lbl.Tag.ToString();
            pages.Profil.page = "";
            this.Controls.Add(frm);
            frm.BringToFront();
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

        private void intrebare_cu_raspunsuri(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Button;

            navbar_home.page = "InvataUnit_vizualizare";
            navbar_home.token_page = btn.Tag.ToString();
            navbar_home.use = false;
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

            var param = new Dictionary<string, string>();

            bool use_param = false;

            if (sort.Trim() == "" || sort == "Toate materiile")
            {
                if (answer_sort == "" || answer_sort == "Toate")
                {
                    data.Add("command", "select * from invataunit");
                    if (guna2Button17.BorderThickness == 2)
                    {
                        data["command"] += " where created = ?";
                        param.Add("created", login_signin.login.accounts_user["username"]);
                        use_param = true;
                    }
                }
                else
                {
                    if (answer_sort == "Fără răspuns")
                        data.Add("command", "select * from invataunit where LENGTH(answers) = 2");
                    else
                        data.Add("command", "select * from invataunit where LENGTH(answers) > 2");
                    if (guna2Button17.BorderThickness == 2)
                    {
                        data["command"] += " and created = ?";
                        param.Add("created", login_signin.login.accounts_user["username"]);
                        use_param = true;
                    }
                }
                data["command"] += " order by data DESC";
            }
            else
            {
                if (answer_sort == "" || answer_sort == "Toate")
                {
                    data.Add("command", "select * from invataunit where category = ?");
                    param.Add("category", sort);
                    use_param = true;
                }
                else
                {
                    if (answer_sort == "Fără răspuns")
                    {
                        data.Add("command", "select * from invataunit where category = ? and LENGTH(answers) = 2");
                        param.Add("category", sort);
                        use_param = true;
                    }
                    else
                    {
                        data.Add("command", "select * from invataunit where category = ? and LENGTH(answers) > 2");
                        param.Add("category", sort);
                        use_param = true;
                    }
                }
                if (guna2Button17.BorderThickness == 2)
                {
                    data["command"] += " and created = ?";
                    param.Add("created", login_signin.login.accounts_user["username"]);
                    use_param = true;
                }
                data["command"] += " order by data DESC";
            }

            if(use_param == true)
            {
                data.Add("params", JsonConvert.SerializeObject(param));
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
                for (int i = 0; i < jb.Count - 1; i++)
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

                    RichTextBox rich = new RichTextBox();
                    rich.Rtf = Convert.ToString(task[i.ToString()]["question"]);

                    lbl_question.Text = rich.Text;

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
                        //btn.Click += intrebare_cu_raspunsuri;

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

        private void panel_materii_Paint(object sender, PaintEventArgs e)
        {

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
