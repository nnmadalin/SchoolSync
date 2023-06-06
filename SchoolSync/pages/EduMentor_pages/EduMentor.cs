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

        private async void load_material(object sender, EventArgs e)
        {
            navbar_home.use = false;
            navbar_home.page = "EduMentor_vizualizare";
            navbar_home.token_page = ((Label)sender).Tag.ToString();
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
                data.Add("command", string.Format("select * from edumentor"));
                if (guna2Button1.BorderThickness == 2)
                        data["command"] += " where created = '" + login_signin.login.accounts_user["username"] + "'";
                if (guna2Button18.BorderThickness == 2)
                    data["command"] += " where favourites like '%" + login_signin.login.accounts_user["token"] + "%'";
                data["command"] += " order by data DESC";
            }
            else
            {
                data.Add("command", string.Format("select * from edumentor where category = '{0}'", sort));

                if (guna2Button1.BorderThickness == 2)
                    data["command"] += " and created = '" + login_signin.login.accounts_user["username"] + "'";
                if (guna2Button18.BorderThickness == 2)
                    data["command"] += " and favourites like '%" + login_signin.login.accounts_user["token"] + "%'";
                data["command"] += " order by data DESC";
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
                    string users = task[i.ToString()]["favourites"];
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
            
        } 

        private void EduMentor_Load(object sender, EventArgs e)
        {
            load_panel();
            page = "home";
        }       
        
        private void adauga_material(object sender, EventArgs e)
        {
            navbar_home.use = false;
            navbar_home.page = "EduMentor_adauga";            
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

        private void label2_Click(object sender, EventArgs e)
        {

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
