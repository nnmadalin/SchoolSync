﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SchoolSync.pages.EduMentor_pages
{
    public partial class EduMentor_Adauga : UserControl
    {
        public EduMentor_Adauga()
        {
            InitializeComponent();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text.Trim() == "")
            {
                guna2TextBox1.BorderColor = Color.Red;
                return;
            }


            schoolsync.show_loading();

            multiple_class _class = new multiple_class();
            string token = _class.generate_token();

            string files = "";

            Random rand = new Random();
            string random_color = rand.Next(256) + ", " + rand.Next(256) + ", " + rand.Next(256);

            foreach (Control control in flowLayoutPanel1.Controls)
            {
                string token_file = await _class.UploadFileAsync(control.Tag.ToString());
                if (token_file != null)
                    files += (token_file + ";");
            }

            string url = "https://schoolsync.nnmadalin.me/api/post.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "insert into edumentor(token, token_user, created, category, color, title, description, files, reading_time, users_hearts) values (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

            //token, login_signin.login.accounts_user["token"], login_signin.login.accounts_user["username"], ((ComboBox)combobox).SelectedItem, random_color, txt_title.Text.Trim(), txt_descriere.Text.Trim(), files, ((Guna.UI2.WinForms.Guna2NumericUpDown)gnu).Value, ""

            dynamic task = await _class.PostRequestAsync(url, data);
            if (task["message"] == "insert success")
            {
                var frm = new notification.success();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);

                schoolsync.hide_loading();

                notification.success.message = "Material salvat cu succes!";
                frm.BringToFront();
                this.Dispose();
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
            schoolsync.hide_loading();
        }

        private void texteditor_button(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;
            if(btn.Name == "fontup")
            {
                int x = Convert.ToInt32(richTextBox1.SelectionFont.Size);
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont.FontFamily, ++x, richTextBox1.SelectionFont.Style); ;
            }
            else if (btn.Name == "fontdown")
            {
                int x = Convert.ToInt32(richTextBox1.SelectionFont.Size);
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont.FontFamily, --x, richTextBox1.SelectionFont.Style); ;
            }
            else if (btn.Name == "fontbold")
            {
                if (richTextBox1.SelectionFont.Bold == false)
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Bold);
                else
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Regular);
            }
            else if (btn.Name == "fontitalic")
            {
                if (richTextBox1.SelectionFont.Italic == false)
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Italic);
                else
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Regular);
            }
            else if (btn.Name == "fontunderline")
            {
                if (richTextBox1.SelectionFont.Underline == false)
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Underline);
                else
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Regular);
            }
            else if (btn.Name == "fontleft")
            {
                richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
            }
            else if (btn.Name == "fontcenter")
            {
                richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            }
            else if (btn.Name == "fontright")
            {
                richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2TextBox btn = sender as Guna.UI2.WinForms.Guna2TextBox;
            btn.BorderColor = Color.FromArgb(213, 218, 223);
        }       

        private void adauga_fisier_Click(object sender, EventArgs e)
        {
            try
            {
                if (flowLayoutPanel1.Controls.Count < 5)
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

                        if (fileSizeibMbs > 10)
                        {
                            var frm = new notification.error();
                            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                            panel.Controls.Add(frm);
                            notification.error.message = "Fisierul: " + fl.Name.Substring(0, 20) + "..." + " are mai mult de 10 MB!";
                            frm.BringToFront();
                        }
                        else
                        {

                            Guna.UI2.WinForms.Guna2Chip guna2Chip = new Guna.UI2.WinForms.Guna2Chip()
                            {
                                FillColor = Color.White,
                                BorderColor = Color.FromArgb(94, 148, 255),
                                ForeColor = Color.Black,
                                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                                AutoRoundedCorners = false,
                                BorderRadius = 10,
                                TextAlign = HorizontalAlignment.Left,
                                Size = new Size(160, 35),
                                Tag = opf.FileName.ToString()
                            };
                            string fnm = Path.GetFileName(opf.FileName);
                            if (fnm.Length >= 16)
                                guna2Chip.Text = fnm.Substring(0, 16) + "...";
                            else
                                guna2Chip.Text = fnm;

                            flowLayoutPanel1.Controls.Add(guna2Chip);
                        }
                    }
                }
                else
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
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
    }
}
