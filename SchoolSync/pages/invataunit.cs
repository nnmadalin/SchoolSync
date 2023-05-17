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
                data.Add("sql", string.Format("insert into invataunit(token, created, category, question, answers, files) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", 
                    token, login_signin.login.accounts_user["username"], ((ComboBox)combobox).SelectedItem, textbox.Text, "{}", files));
                
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

        
    }
}
