using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;

namespace SchoolSync.pages
{
    public partial class EduMentor : UserControl
    {
        public EduMentor()
        {
            InitializeComponent();
        }

        string page = "", token_first_material = "", sort = "";

        public static Image incarca_imagine_specifica(string str)
        {
            if (str == "Limba română")
                return SchoolSync.Properties.Resources.clarisse_meyer_jKU2NneZAbI_unsplash;
            if (str == "Matematică")
                return SchoolSync.Properties.Resources.artturi_jalli_gYrYa37fAKI_unsplash;
            if (str == "Istorie")
                return SchoolSync.Properties.Resources.old_bible_wooden_table;
            if (str == "Chimie")
                return SchoolSync.Properties.Resources.terry_vlisidis_RflgrtzU3Cw_unsplash__1_;
            if (str == "Biologie")
                return SchoolSync.Properties.Resources.timothy_dykes_zVU_3H3cwjk_unsplash;
            if (str == "Fizică")
                return SchoolSync.Properties.Resources.engin_akyurt_KUeJcc4YUug_unsplash;
            if (str == "Geografie")
                return SchoolSync.Properties.Resources.kyle_glenn_nXt5HtLmlgE_unsplash;
            if (str == "Studii sociale")
                return SchoolSync.Properties.Resources.aaron_burden_1zR3WNSTnvY_unsplash;
            if (str == "Informatică")
                return SchoolSync.Properties.Resources.luca_bravo_XJXWbfSo2f0_unsplash;
            if (str == "Engleza")
                return SchoolSync.Properties.Resources.simon_frederick_vuV25OfnGa8_unsplash;
            if (str == "Franceza")
                return SchoolSync.Properties.Resources.anthony_choren_lYzap0eubDY_unsplash;
            if (str == "Alte limbi")
                return SchoolSync.Properties.Resources.brett_jordan_POMpXtcVYHo_unsplash;
            if (str == "Ed. tehnologică")
                return SchoolSync.Properties.Resources.adi_goldstein_EUsVwEOsblE_unsplash;
            if (str == "Arte")
                return SchoolSync.Properties.Resources.aaron_burden_1zR3WNSTnvY_unsplash;
            if (str == "Ed. muzicală")
                return SchoolSync.Properties.Resources.marcela_laskoski_YrtFlrLo2DQ_unsplash;

            return SchoolSync.Properties.Resources.clarisse_meyer_jKU2NneZAbI_unsplash;
        }

        private async void sterge_adauga_inima(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2PictureBox pct = sender as Guna.UI2.WinForms.Guna2PictureBox;

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("select * from edumentor where token = '{0}'", pct.Name.ToString()));

            dynamic task = await _class.PostRequestAsync(url, data);
            
            string users = task["0"]["users_hearts"];
                            
            string[] split_user = users.Split(';');

            bool ok = false;
            Bitmap bit = SchoolSync.Properties.Resources.favorite_FILL0_wght700_GRAD0_opsz48;
            if (pct.Tag.ToString() == "0")
            {
                pct.Image = SchoolSync.Properties.Resources.favorite_FILL1_wght700_GRAD0_opsz48;
                users += (login_signin.login.accounts_user["token"] + ";");
                pct.Tag = "1";
            }
            else
            {
                pct.Image = SchoolSync.Properties.Resources.favorite_FILL0_wght700_GRAD0_opsz48;

                string newlove = "";
                for (int j = 0; j < split_user.Length - 1; j++)
                {
                    string tkn = login_signin.login.accounts_user["token"];
                    if (split_user[j] != tkn)
                    {
                        newlove += (split_user[j] + ";");
                    }
                }

                ok = true;
                users = newlove;
                pct.Tag = "0";
            }
            
            url = "https://schoolsync.nnmadalin.me/api/put.php";
            data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("update edumentor set users_hearts = '{1}' where token = '{0}'", pct.Name.ToString(), users));
            task = await _class.PostRequestAsync(url, data);
            if (task["message"] == "update success")
            {
                var frm = new notification.success();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.success.message = "Adaugat cu succes la favorite!";
                if (ok == true)
                {
                    notification.success.message = "Eliminat cu succes de la favorite!";
                }
                frm.BringToFront();
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
            load_panel();
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
            if (guna2Button18.BorderThickness != 2)
            {
                page = "home";
                load_panel();
            }

        }

        private void deschide_fisier_buton(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            System.Diagnostics.Process.Start(@"https://schoolsync.nnmadalin.me/api/getfile.php?token=" + btn.Tag.ToString());
        }

        private async void load_profil(object sender, EventArgs e)
        {
            Label lbl = (sender as Label);
            var frm = new pages.Profil();
            pages.Profil.token = lbl.Tag.ToString();
            Console.WriteLine(lbl.Tag.ToString());
            pages.Profil.page = "";
            this.Controls.Add(frm);
            frm.BringToFront();
        }

        string fisiere_value = "";

        private async void sterge_material(object sender, EventArgs e)
        {
            string token = ((Guna.UI2.WinForms.Guna2CircleButton)sender).Tag.ToString();
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
            guna2MessageDialog1.Caption = "Sterge material education!";
            guna2MessageDialog1.Text = "Esti sigur ca vrei sa stergi materialul?";
            DialogResult dr = guna2MessageDialog1.Show();
            if (dr == DialogResult.Yes)
            {
                string url = "https://schoolsync.nnmadalin.me/api/delete.php";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("sql", string.Format("delete from edumentor where token = '{0}'", token));

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

                    notification.success.message = "Material sters cu succes!";
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
                    if (control.Name == "panel_material")
                    {
                        this.Controls.Remove(control);
                        break;
                    }
                }

                load_panel();
            }
        }

        private async void editare_material_api(object sender, EventArgs e)
        {
            string token = ((Guna.UI2.WinForms.Guna2Button)sender).Tag.ToString();
            if (this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_titlu"].Text.Trim() == "")
            {
                ((Guna.UI2.WinForms.Guna2TextBox)this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_titlu"]).BorderColor = Color.Red;
            }
            else if (this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_descriere"].Text.Trim() == "")
            {
                ((Guna.UI2.WinForms.Guna2TextBox)this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_descriere"]).BorderColor = Color.Red;
            }
            else
            {
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Caption = "Editeaza materialul!";
                guna2MessageDialog1.Text = "Esti sigur ca vrei sa editezi materialul?";
                DialogResult dr = guna2MessageDialog1.Show();
                if (dr == DialogResult.Yes)
                {
                    //TOTUL ESTE BINE
                    schoolsync.show_loading();

                    ((Guna.UI2.WinForms.Guna2TextBox)this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_titlu"]).BorderColor = Color.FromArgb(213, 218, 223);
                    ((Guna.UI2.WinForms.Guna2TextBox)this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_descriere"]).BorderColor = Color.FromArgb(213, 218, 223);

                    multiple_class _class = new multiple_class();

                    Control combobox = this.Controls["pnl_fullpage"].Controls["pnl"].Controls["combobox_materii"];
                    Control textbox_title = this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_titlu"];
                    Control textbox_descriere = this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_descriere"];
                    Control min = this.Controls["pnl_fullpage"].Controls["pnl"].Controls["gnu"];

                    string url;
                    Dictionary<string, string> data;
                    dynamic task;

                    string token_app = schoolsync.token;
                    string files = "";
                    string[] split_1 = fisiere_value.Split(';');

                    foreach (Control control in this.Controls["pnl_fullpage"].Controls["pnl"].Controls["flp_fisiere"].Controls)
                    {
                        bool ok = false;
                        for (int i = 0; i < split_1.Length - 1; i++)
                        {
                            if (split_1[i] == control.Tag.ToString())
                            {
                                split_1[i] = "-1";
                                ok = true;
                                break;
                            }
                        }
                        if (ok == true)
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
                    data.Add("sql", string.Format("update edumentor set category = '{0}', title = '{1}', description = '{2}' , reading_time = '{3}', files = '{4}' where token = '{5}'",
                        ((ComboBox)combobox).SelectedItem, textbox_title.Text.Trim(), textbox_descriere.Text.Trim(), ((Guna.UI2.WinForms.Guna2NumericUpDown)min).Value, files, token));

                    task = await _class.PostRequestAsync(url, data);
                    if (task["message"] == "update success")
                    {
                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);

                        notification.success.message = "Material modificat cu succes!";
                        frm.BringToFront();
                        foreach (Control control in this.Controls)
                        {
                            if (control.Name == "pnl_fullpage")
                            {
                                this.Controls.Remove(control);
                                break;
                            }
                        }
                        load_panel();
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

        private async void editare_material(object sender, EventArgs e)
        {
            page = "adauga_material";
            string token = ((Guna.UI2.WinForms.Guna2CircleButton)sender).Tag.ToString();

            Panel pnl_fullpage = new Panel()
            {
                Size = new Size(1192, 690),
                BackColor = Color.FromArgb(133, 135, 237),
                Name = "pnl_fullpage"
            };
            Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
            {
                Size = new Size(950, 650),
                Location = new Point(120, 20),
                FillColor = Color.FromArgb(93, 94, 165),
                BorderRadius = 20,
                UseTransparentBackground = true,
                Name = "pnl"
            };

            Guna.UI2.WinForms.Guna2CircleButton btn_inchide_panel = new Guna.UI2.WinForms.Guna2CircleButton()
            {
                Size = new Size(35, 35),
                BorderColor = Color.White,
                BorderThickness = 2,
                UseTransparentBackground = true,
                FillColor = Color.White,
                Image = SchoolSync.Properties.Resources.close_FILL1_wght700_GRAD0_opsz48,
                ImageAlign = HorizontalAlignment.Center,
                Location = new Point(900, 20),
                Cursor = Cursors.Hand
            };
            btn_inchide_panel.Click += inchide_panel_adauga_material;

            Label title = new Label()
            {
                Text = "Modifica materialul!",
                Font = new Font("Segoe UI Semibold", 17, FontStyle.Bold),
                Location = new Point(40, 20),
                ForeColor = Color.White,
                AutoSize = true,
            };

            Label lbl_titlu = new Label()
            {
                Text = "Titlu:",
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                Location = new Point(20, 70),
                ForeColor = Color.White,
                AutoSize = true,
            };
            Guna.UI2.WinForms.Guna2TextBox txt_titlu = new Guna.UI2.WinForms.Guna2TextBox()
            {
                Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                Size = new Size(900, 40),
                Location = new Point(20, 100),
                BorderRadius = 10,
                PlaceholderText = "Adauga titlu materialului!",
                ForeColor = Color.Black,
                Name = "txt_titlu",
            };
            txt_titlu.TextChanged += border_none;

            Label lbl_descriere = new Label()
            {
                Text = "Descriere:",
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                Location = new Point(20, 150),
                ForeColor = Color.White,
                AutoSize = true,
            };
            Guna.UI2.WinForms.Guna2TextBox txt_descriere = new Guna.UI2.WinForms.Guna2TextBox()
            {
                Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                Size = new Size(900, 250),
                Location = new Point(20, 180),
                BorderRadius = 10,
                PlaceholderText = "Adauga o descriere materialului!",
                ForeColor = Color.Black,
                Multiline = true,
                AutoScroll = true,
                ScrollBars = ScrollBars.Vertical,
                Name = "txt_descriere",
            };
            txt_descriere.TextChanged += border_none;

            Guna.UI2.WinForms.Guna2CircleButton btn_fisier = new Guna.UI2.WinForms.Guna2CircleButton()
            {
                Size = new Size(35, 35),
                BorderColor = Color.White,
                BorderThickness = 2,
                UseTransparentBackground = true,
                FillColor = Color.White,
                Image = SchoolSync.Properties.Resources.attach_file_FILL1_wght700_GRAD0_opsz48,
                ImageAlign = HorizontalAlignment.Center,
                Location = new Point(20, 450),
                Cursor = Cursors.Hand
            };

            btn_fisier.Click += adauga_fisier;

            FlowLayoutPanel flp_fisiere = new FlowLayoutPanel()
            {
                Location = new Point(60, 450),
                Size = new Size(850, 40),
                Name = "flp_fisiere"
            };

            Guna.UI2.WinForms.Guna2ComboBox cmb = new Guna.UI2.WinForms.Guna2ComboBox()
            {
                BorderRadius = 15,
                Size = new Size(200, 50),
                Location = new Point(20, 500),
                Items = { "Limba română", "Matematică", "Istorie", "Chimie", "Biologie", "Fizică", "Geografie",
                    "Studii sociale", "Informatică", "Engleza", "Franceza", "Alte limbi", "Ed. tehnologică", "Arte", "Ed. muzicală" },
                FillColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Name = "combobox_materii",
                Cursor = Cursors.Hand,
            };
            cmb.SelectedIndex = 0;

            Label lbl_time = new Label()
            {
                Text = "Cat dureaza sa fie citit materialul? (min)",
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                Location = new Point(20, 550),
                ForeColor = Color.White,
                AutoSize = true,
            };
            Guna.UI2.WinForms.Guna2NumericUpDown gnu = new Guna.UI2.WinForms.Guna2NumericUpDown()
            {
                Minimum = 2,
                Maximum = 999,
                Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                Size = new Size(90, 30),
                Location = new Point(370, 550),
                Value = 2,
                BorderRadius = 5,
                Name = "gnu"
            };

            Guna.UI2.WinForms.Guna2Button btn_trimite = new Guna.UI2.WinForms.Guna2Button()
            {
                Location = new Point(20, 600),
                Text = "Modifica material!",
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                Size = new Size(200, 40),
                FillColor = Color.FromArgb(107, 150, 108),
                BorderRadius = 10,
                Cursor = Cursors.Hand,
                Tag = token,
            };
            btn_trimite.Click += editare_material_api;

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("sql", string.Format("select * from edumentor where token = '{0}'", token));

            dynamic task = await _class.PostRequestAsync(url, data);

            if(task["message"] == "success")
            {
                string files = task["0"]["files"];
                string[] splitfiles = files.Split(';');

                for(int i = 0; i < splitfiles.Length - 1; i++)
                {
                    Guna.UI2.WinForms.Guna2Chip guna2Chip = new Guna.UI2.WinForms.Guna2Chip()
                    {
                        FillColor = Color.White,
                        BorderColor = Color.White,
                        ForeColor = Color.Black,
                        Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                        AutoRoundedCorners = false,
                        BorderRadius = 10,
                        TextAlign = HorizontalAlignment.Left,
                        Size = new Size(160, 35),
                        Tag = splitfiles[i].ToString()
                    };                    
                    if (splitfiles[i].Length > 16)
                        guna2Chip.Text = System.IO.Path.GetFileName(splitfiles[i]).Substring(0, 16) + "...";
                    else
                        guna2Chip.Text = splitfiles[i];

                    flp_fisiere.Controls.Add(guna2Chip);
                }

                txt_titlu.Text = task["0"]["title"];
                txt_descriere.Text = task["0"]["description"];
                string x = task["0"]["category"];
                cmb.SelectedIndex = cmb.Items.IndexOf(x);
                gnu.Value = Convert.ToInt32(task["0"]["reading_time"]);
            }

            pnl.Controls.Add(title);
            pnl.Controls.Add(btn_inchide_panel);

            pnl.Controls.Add(lbl_titlu);
            pnl.Controls.Add(txt_titlu);

            pnl.Controls.Add(lbl_descriere);
            pnl.Controls.Add(txt_descriere);

            pnl.Controls.Add(btn_fisier);
            pnl.Controls.Add(flp_fisiere);

            pnl.Controls.Add(cmb);
            pnl.Controls.Add(lbl_time);
            pnl.Controls.Add(gnu);

            pnl.Controls.Add(btn_trimite);

            pnl_fullpage.Controls.Add(pnl);

            this.Controls.Add(pnl_fullpage);
            pnl_fullpage.BringToFront();
        }

        private async void load_material(object sender, EventArgs e)
        {
            Label lbl_token = sender as Label;
            page = "vizualizare_material";

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
            data.Add("sql", string.Format("select * from edumentor where token = '{0}'", lbl_token.Tag));

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
                    Text = task["0"]["created"],
                    Tag = task["0"]["token_user"],
                    Cursor = Cursors.Hand
                };
                nume.Click += load_profil;

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
                    Location = new Point(625, 237),
                    Image = SchoolSync.Properties.Resources.schedule_FILL1_wght700_GRAD0_opsz48,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    UseTransparentBackground = true
                };
                Label read_time = new Label()
                {
                    Location = new Point(650, 235),
                    AutoSize = true,
                    Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                    TextAlign = ContentAlignment.TopRight,
                    Text = task["0"]["reading_time"] + " min"
                };
                Guna.UI2.WinForms.Guna2CirclePictureBox pct_inima = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                {
                    FillColor = Color.Gray,
                    Size = new Size(20, 20),
                    Location = new Point(625, 260),
                    Image = SchoolSync.Properties.Resources.favorite_FILL1_wght700_GRAD0_opsz48,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    UseTransparentBackground = true
                };
                string inimi = task["0"]["users_hearts"];
                string[] spinimi = inimi.Split(';');
                Label loves = new Label()
                {
                    Location = new Point(650, 257),
                    AutoSize = true,
                    Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                    TextAlign = ContentAlignment.TopLeft,
                    Text = (spinimi.Length - 1).ToString()
                };

                Guna.UI2.WinForms.Guna2CircleButton btn_edit = new Guna.UI2.WinForms.Guna2CircleButton()
                {
                    Image = SchoolSync.Properties.Resources.edit_FILL1_wght700_GRAD0_opsz48,
                    Size = new Size(30, 30),
                    ImageAlign = HorizontalAlignment.Center,
                    ImageSize = new Size(15, 15),
                    UseTransparentBackground = true,
                    FillColor = Color.White,
                    BorderColor = Color.Black,
                    Tag = task["0"]["token"],
                    BorderThickness = 1,
                    Location = new Point(760, 220),
                    Cursor = Cursors.Hand
                };
                btn_edit.Click += editare_material;
                Guna.UI2.WinForms.Guna2CircleButton btn_delete = new Guna.UI2.WinForms.Guna2CircleButton()
                {
                    Image = SchoolSync.Properties.Resources.delete_FILL1_wght700_GRAD0_opsz48,
                    Size = new Size(30, 30),
                    ImageAlign = HorizontalAlignment.Center,
                    ImageSize = new Size(15, 15),
                    UseTransparentBackground = true,
                    FillColor = Color.White,
                    BorderColor = Color.Black,
                    Tag = task["0"]["token"],
                    BorderThickness = 1,
                    Location = new Point(760, 260),
                    Cursor = Cursors.Hand
                };
                btn_delete.Click += sterge_material;

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
                    Margin = new Padding(92, 0, 0 , 0),
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

                fisiere_value = task["0"]["files"];

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


                if (task["0"]["created"] == login_signin.login.accounts_user["username"])
                {
                    center.Controls.Add(btn_edit);
                    center.Controls.Add(btn_delete);
                }

                pnl.Controls.Add(flp_main);
                pnl.Controls.Add(center);
                pnl.Controls.Add(panglica);
                
            }

            
            this.Controls.Add(pnl);
            pnl.BringToFront();
        }

        async void load_panel()
        {
            page = "home";
            flowLayoutPanel1.Controls.Clear();

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);

            if (sort.Trim() == "" || sort == "Toate materiile")
            {
                data.Add("sql", string.Format("select * from edumentor"));
                if (guna2Button1.BorderThickness == 2)
                        data["sql"] += " where created = '" + login_signin.login.accounts_user["username"] + "'";
                if (guna2Button18.BorderThickness == 2)
                    data["sql"] += " where users_hearts like '%" + login_signin.login.accounts_user["token"] + "%'";
                data["sql"] += " order by data DESC";
            }
            else
            {
                data.Add("sql", string.Format("select * from edumentor where category = '{0}'", sort));

                if (guna2Button1.BorderThickness == 2)
                    data["sql"] += " and created = '" + login_signin.login.accounts_user["username"] + "'";
                if (guna2Button18.BorderThickness == 2)
                    data["sql"] += " and users_hearts like '%" + login_signin.login.accounts_user["token"] + "%'";
                data["sql"] += " order by data DESC";
            }

            if (guna2Button1.BorderThickness == 2)
            {
                Label title = new Label()
                {
                    Text = "Materialele mele",
                    Font = new Font("Segoe UI Black", 30, FontStyle.Bold),
                    Size = new Size(1140, 70),
                    TextAlign = ContentAlignment.TopCenter 
                };
                flowLayoutPanel1.Controls.Add(title);
            }
            if (guna2Button18.BorderThickness == 2)
            {
                Label title = new Label()
                {
                    Text = "Materialele mele favorite",
                    Font = new Font("Segoe UI Black", 30, FontStyle.Bold),
                    Size = new Size(1140, 70),
                    TextAlign = ContentAlignment.TopCenter
                };
                flowLayoutPanel1.Controls.Add(title);
            }

            dynamic task = await _class.PostRequestAsync(url, data);
            JObject jb = task;

            if (task["message"] == "success")
            {
                if (jb.Count - 1 > 0)
                    token_first_material = task["0"]["token"];
                for (int i = 0; i < jb.Count - 1; i++)
                {
                    Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                    {
                        Size = new Size(277, 376),
                        BorderRadius = 5,
                        UseTransparentBackground = true,
                        Padding = new Padding(0, 0, 0, 10),
                        FillColor = Color.FromArgb(223, 229, 232),
                        AutoSize = true,
                        BorderColor = task[i.ToString()]["color"],
                        BorderThickness = 2
                    };
                    Guna.UI2.WinForms.Guna2PictureBox gpb = new Guna.UI2.WinForms.Guna2PictureBox()
                    {
                        Size = new Size(271, 200),
                        Location = new Point(3, 3),
                        UseTransparentBackground = true,
                        BorderRadius = 5,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                    };                    
                    Label lbl = new Label()
                    {
                        TextAlign = ContentAlignment.MiddleCenter,
                        Size = new Size(260, 100),
                        AutoEllipsis = true,
                        Location = new Point(10, 219),
                        Font = new Font("Segoe UI", 13, FontStyle.Bold),
                    };                    
                    Panel pnl_jos = new Panel()
                    {
                        Size = new Size(271, 34),
                        Location = new Point(3, 329),
                    };
                    Label lbl_read = new Label()
                    {
                        Location = new Point(8, 9),
                        Size = new Size(117, 19),
                        Cursor = Cursors.Hand,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.Black,
                        Text = "Citeşte mai mult",
                        Tag = task[i.ToString()]["token"]
                    };
                    lbl_read.Click += load_material;
                    Guna.UI2.WinForms.Guna2PictureBox pct = new Guna.UI2.WinForms.Guna2PictureBox()
                    {
                        Image = SchoolSync.Properties.Resources.favorite_FILL0_wght700_GRAD0_opsz48,
                        UseTransparentBackground = true,
                        Size = new Size(17, 17),                        
                        Location = new Point(233, 9),
                        BackColor = Color.DimGray,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Cursor = Cursors.Hand,
                        Tag = 0,
                        Name = task[i.ToString()]["token"]

                    };

                    pct.Click += sterge_adauga_inima;
                    string users = task[i.ToString()]["users_hearts"];
                    string[] split_user = users.Split(';');                    
                    for(int j = 0; j < split_user.Length - 1; j++)
                    {
                        string tkn = login_signin.login.accounts_user["token"];
                        if (split_user[j] == tkn)
                        {
                            pct.Tag = "1";
                            pct.Image = SchoolSync.Properties.Resources.favorite_FILL1_wght700_GRAD0_opsz48;
                            break;
                        }
                    }

                    string sttr = task[i.ToString()]["category"];
                    gpb.Image = incarca_imagine_specifica(sttr);
                    lbl.Text = task[i.ToString()]["title"];

                    pnl.Controls.Add(gpb);
                    pnl.Controls.Add(lbl);
                    pnl_jos.Controls.Add(lbl_read);
                    pnl_jos.Controls.Add(pct);
                    pnl.Controls.Add(pnl_jos);

                    flowLayoutPanel1.Controls.Add(pnl);
                }
            }
            timer1.Enabled = true;
            
        }

        private void EduMentor_Load(object sender, EventArgs e)
        {
            load_panel();
            page = "home";
        }
        
        private void adauga_fisier(object sender, EventArgs e)
        {
            try
            {
                if (this.Controls["pnl_fullpage"].Controls["pnl"].Controls["flp_fisiere"].Controls.Count < 5)
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
                                BorderColor = Color.White,
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
                            
                            this.Controls["pnl_fullpage"].Controls["pnl"].Controls["flp_fisiere"].Controls.Add(guna2Chip);
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
        
        private void inchide_panel_adauga_material(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if(control.Name == "pnl_fullpage")
                {
                    this.Controls.Remove(control);
                    break;
                }
            }
            load_panel();
        }

        private void border_none(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2TextBox btn = sender as Guna.UI2.WinForms.Guna2TextBox;
            btn.BorderColor = Color.FromArgb(213, 218, 223);
        }

        private async void trimite_material_api(object sender, EventArgs e)
        {
            if (this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_titlu"].Text.Trim() == "")
            {
                ((Guna.UI2.WinForms.Guna2TextBox)this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_titlu"]).BorderColor = Color.Red;
            }
            else if (this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_descriere"].Text.Trim() == "")
            {
                ((Guna.UI2.WinForms.Guna2TextBox)this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_descriere"]).BorderColor = Color.Red;
            }
            else
            {
                schoolsync.show_loading();

                multiple_class _class = new multiple_class();
                string token = _class.generate_token();


                Control combobox = this.Controls["pnl_fullpage"].Controls["pnl"].Controls["combobox_materii"];
                Control txt_title = this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_titlu"];
                Control txt_descriere = this.Controls["pnl_fullpage"].Controls["pnl"].Controls["txt_descriere"];
                Control gnu = this.Controls["pnl_fullpage"].Controls["pnl"].Controls["gnu"];

                string files = "";

                Random rand = new Random();
                string random_color = rand.Next(256) + ", " + rand.Next(256) + ", " + rand.Next(256);

                foreach (Control control in this.Controls["pnl_fullpage"].Controls["pnl"].Controls["flp_fisiere"].Controls)
                {
                    string token_file = await _class.UploadFileAsync(control.Tag.ToString());
                    if (token_file != null)
                        files += (token_file + ";");
                }

                string url = "https://schoolsync.nnmadalin.me/api/post.php";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("sql", string.Format("insert into edumentor(token, token_user, created, category, color, title, description, files, reading_time, users_hearts) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')",
                    token, login_signin.login.accounts_user["token"], login_signin.login.accounts_user["username"], ((ComboBox)combobox).SelectedItem, random_color, txt_title.Text.Trim(), txt_descriere.Text.Trim(), files, ((Guna.UI2.WinForms.Guna2NumericUpDown)gnu).Value, ""));

                dynamic task = await _class.PostRequestAsync(url, data);
                if (task["message"] == "insert success")
                {
                    var frm = new notification.success();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);

                    notification.success.message = "Material salvat cu succes!";
                    frm.BringToFront();
                    foreach (Control control in this.Controls)
                    {
                        if (control.Name == "pnl_fullpage")
                        {
                            this.Controls.Remove(control);
                            load_panel();
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

        private void adauga_material(object sender, EventArgs e)
        {
            page = "adauga_material";
            Panel pnl_fullpage = new Panel()
            {
                Size = new Size(1192, 690),
                BackColor = Color.FromArgb(133, 135, 237),
                Name = "pnl_fullpage"
            };
            Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
            {
                Size = new Size(950, 650),
                Location = new Point(120, 20),
                FillColor = Color.FromArgb(93, 94, 165),
                BorderRadius = 20,
                UseTransparentBackground = true,
                Name = "pnl"
            };

            Guna.UI2.WinForms.Guna2CircleButton btn_inchide_panel = new Guna.UI2.WinForms.Guna2CircleButton()
            {
                Size = new Size(35, 35),
                BorderColor = Color.White,
                BorderThickness = 2,
                UseTransparentBackground = true,
                FillColor = Color.White,
                Image = SchoolSync.Properties.Resources.close_FILL1_wght700_GRAD0_opsz48,
                ImageAlign = HorizontalAlignment.Center,
                Location = new Point(900, 20),
                Cursor = Cursors.Hand
            };
            btn_inchide_panel.Click += inchide_panel_adauga_material;

            Label title = new Label() 
            { 
                Text = "Adauga un material nou!",
                Font = new Font("Segoe UI Semibold", 17, FontStyle.Bold),
                Location = new Point(40, 20),
                ForeColor = Color.White,
                AutoSize = true,
            };

            Label lbl_titlu = new Label()
            {
                Text = "Titlu:",
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                Location = new Point(20, 70),
                ForeColor = Color.White,
                AutoSize = true,
            };
            Guna.UI2.WinForms.Guna2TextBox txt_titlu = new Guna.UI2.WinForms.Guna2TextBox()
            {
                Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                Size = new Size(900, 40),
                Location = new Point(20, 100),
                BorderRadius = 10,
                PlaceholderText = "Adauga titlu materialului!",
                ForeColor = Color.Black,
                Name = "txt_titlu",
            };
            txt_titlu.TextChanged += border_none;

            Label lbl_descriere = new Label()
            {
                Text = "Descriere:",
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                Location = new Point(20, 150),
                ForeColor = Color.White,
                AutoSize = true,
            };
            Guna.UI2.WinForms.Guna2TextBox txt_descriere = new Guna.UI2.WinForms.Guna2TextBox()
            {
                Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                Size = new Size(900, 250),
                Location = new Point(20, 180),
                BorderRadius = 10,
                PlaceholderText = "Adauga o descriere materialului!",
                ForeColor = Color.Black,
                Multiline = true, 
                AutoScroll = true, 
                ScrollBars = ScrollBars.Vertical,
                Name = "txt_descriere",
            };
            txt_descriere.TextChanged += border_none;

            Guna.UI2.WinForms.Guna2CircleButton btn_fisier = new Guna.UI2.WinForms.Guna2CircleButton()
            {
                Size = new Size(35, 35),
                BorderColor = Color.White,                
                BorderThickness = 2,
                UseTransparentBackground = true,
                FillColor = Color.White,
                Image = SchoolSync.Properties.Resources.attach_file_FILL1_wght700_GRAD0_opsz48,
                ImageAlign = HorizontalAlignment.Center,
                Location = new Point(20, 450),
                Cursor = Cursors.Hand
            };

            btn_fisier.Click += adauga_fisier;

            FlowLayoutPanel flp_fisiere = new FlowLayoutPanel()
            {
                Location = new Point(60, 450),
                Size = new Size(850, 40),
                Name = "flp_fisiere"
            };

            Guna.UI2.WinForms.Guna2ComboBox cmb = new Guna.UI2.WinForms.Guna2ComboBox()
            {
                BorderRadius = 15,
                Size = new Size(200, 50),
                Location = new Point(20, 500),
                Items = { "Limba română", "Matematică", "Istorie", "Chimie", "Biologie", "Fizică", "Geografie",
                    "Studii sociale", "Informatică", "Engleza", "Franceza", "Alte limbi", "Ed. tehnologică", "Arte", "Ed. muzicală" },
                FillColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Name = "combobox_materii",
                Cursor = Cursors.Hand,
            };
            cmb.SelectedIndex = 0;

            Label lbl_time = new Label()
            {
                Text = "Cat dureaza sa fie citit materialul? (min)",
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                Location = new Point(20, 550),
                ForeColor = Color.White,
                AutoSize = true,
            };
            Guna.UI2.WinForms.Guna2NumericUpDown gnu = new Guna.UI2.WinForms.Guna2NumericUpDown()
            {
                Minimum = 2,
                Maximum = 999,
                Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                Size = new Size(90, 30),
                Location = new Point(370, 550),
                Value = 2,
                BorderRadius = 5,
                Name = "gnu"
            };

            Guna.UI2.WinForms.Guna2Button btn_trimite = new Guna.UI2.WinForms.Guna2Button()
            {
                Location = new Point(20, 600),
                Text = "Adauga material!",
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                Size = new Size(180, 40),
                FillColor = Color.FromArgb(107, 150, 108),
                BorderRadius = 10,
                Cursor = Cursors.Hand,
            };
            btn_trimite.Click += trimite_material_api;

            pnl.Controls.Add(title);
            pnl.Controls.Add(btn_inchide_panel);

            pnl.Controls.Add(lbl_titlu);
            pnl.Controls.Add(txt_titlu);

            pnl.Controls.Add(lbl_descriere);
            pnl.Controls.Add(txt_descriere);

            pnl.Controls.Add(btn_fisier);
            pnl.Controls.Add(flp_fisiere);
            
            pnl.Controls.Add(cmb);
            pnl.Controls.Add(lbl_time);
            pnl.Controls.Add(gnu);
            
            pnl.Controls.Add(btn_trimite);

            pnl_fullpage.Controls.Add(pnl);
          
            this.Controls.Add(pnl_fullpage);
            pnl_fullpage.BringToFront();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            guna2Button1.BorderThickness = 0;
            guna2Button18.BorderThickness = 0;
            guna2Button2.BorderThickness = 2;
            load_panel();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            guna2Button1.BorderThickness = 2;
            guna2Button18.BorderThickness = 0;
            guna2Button2.BorderThickness = 0;
            load_panel();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            if (page == "home")
            {
                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);

                if (sort.Trim() == "" || sort == "Toate materiile")
                {
                    data.Add("sql", string.Format("select * from edumentor"));
                    if (guna2Button1.BorderThickness == 2)
                        data["sql"] += " where created = '" + login_signin.login.accounts_user["username"] + "'";
                    if (guna2Button18.BorderThickness == 2)
                        data["sql"] += " where users_hearts like '%" + login_signin.login.accounts_user["token"] + "%'";
                    data["sql"] += " order by data DESC";
                }
                else
                {
                    data.Add("sql", string.Format("select * from edumentor where category = '{0}'", sort));

                    if (guna2Button1.BorderThickness == 2)
                        data["sql"] += " and created = '" + login_signin.login.accounts_user["username"] + "'";
                    if (guna2Button18.BorderThickness == 2)
                        data["sql"] += " and users_hearts like '%" + login_signin.login.accounts_user["token"] + "%'";
                    data["sql"] += " order by data DESC";
                }

                dynamic task = await _class.PostRequestAsync_norefresh(url, data);
                JObject jb = task;
                if (jb.Count - 1 >= 1)
                {
                    string tkn = task["0"]["token"];
                    
                    if (tkn.Trim() != token_first_material.Trim())
                    {
                        token_first_material = task["0"]["token"];
                        schoolsync.show_loading();
                        load_panel();
                        schoolsync.hide_loading();
                    }
                }
            }
            else
                timer1.Enabled = false;
        }

        private void guna2Button18_Click(object sender, EventArgs e)
        {
            guna2Button1.BorderThickness = 0;
            guna2Button18.BorderThickness = 2;
            guna2Button2.BorderThickness = 0;
            load_panel();
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            sort = guna2ComboBox2.SelectedItem.ToString();
            load_panel();
        }

        

    }
}
