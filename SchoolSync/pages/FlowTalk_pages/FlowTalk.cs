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
        bool use = false, go = false;

        private async void click_message(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;

            token_message = ctrl.Tag.ToString();            
            use = true;
            go = true;
            flowLayoutPanel2.Tag = token_message;
            flowLayoutPanel2.Controls.Clear();

            if (guna2Panel3.Visible == false)
            {
                guna2Panel3.Visible = true;
                flowLayoutPanel2.Visible = true;
                guna2TextBox1.Visible = true;
                guna2Button1.Visible = true;
            }
        }

        async void load_tab()
        {
            try
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
                            try
                            {
                                if (x >= 0)
                                {

                                    if (Convert.ToString(sub[x.ToString()]["text"]) != "")
                                    {
                                        if (Convert.ToString(sub[x.ToString()]["root"]) == "0")
                                            last_mess.Text = sub[x.ToString()]["user"] + ": " + sub[x.ToString()]["text"];
                                        else
                                            last_mess.Text = sub[x.ToString()]["text"];
                                    }
                                    else
                                    {
                                        string y = sub[x.ToString()]["file"];
                                        string[] split = y.Split('/');
                                        last_mess.Text = sub[x.ToString()]["user"] + ": " + split[2];
                                    }
                                }
                            }
                            catch { };

                            pnl.Controls.Add(pct);
                            pnl.Controls.Add(name);
                            pnl.Controls.Add(last_mess);

                            flowLayoutPanel1.Controls.Add(pnl);
                        }
                    }
                }
            }
            catch (Exception ex){ Console.WriteLine(ex.Message); };
        }

        private async void FlowTalk_Load(object sender, EventArgs e)
        {
            schoolsync.show_loading();

            if (navbar_home.page == "FlowTalk_home")
            {
                token_message = navbar_home.token_page;
                use = true;
                go = true;
                flowLayoutPanel2.Tag = token_message;
                flowLayoutPanel2.Controls.Clear();

                if (guna2Panel3.Visible == false)
                {
                    guna2Panel3.Visible = true;
                    flowLayoutPanel2.Visible = true;
                    guna2TextBox1.Visible = true;
                    guna2Button1.Visible = true;
                }
            }

            load_tab();           

            schoolsync.hide_loading();
        }

        async void send_message()
        {
            string message = guna2TextBox1.Text;

            if (message.Trim() == "")
                return;

            JObject json = new JObject();
            json.Add("date", DateTime.Now.ToString());
            json.Add("file", "");
            json.Add("root", "0");
            json.Add("text", message);
            json.Add("user", Convert.ToString(login_signin.login.accounts_user["username"]));
            json.Add("user_token", Convert.ToString(login_signin.login.accounts_user["token"]));
            json.Add("is_deleted", "0");

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
            if (task["message"] == "success")
            {
                dynamic x = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["messages"]));
                JObject jbo = x;
                jbo.Add(jbo.Count.ToString(), json);

                url = "https://schoolsync.nnmadalin.me/api/put.php";
                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "update flowtalk set messages = ?, seen = ? where token = ?");
                param = new Dictionary<string, string>()
                {
                    {"messages", JsonConvert.SerializeObject(jbo)},
                    {"seen", ""},
                    {"token", token_message}
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                task = await _class.PostRequestAsync(url, data);
                if(task["message"] != "update success")
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Ceva nu a mers bine :(!";
                    frm.BringToFront();
                }
            }

            guna2TextBox1.Clear();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            send_message();
            flowLayoutPanel2.VerticalScroll.Value = flowLayoutPanel2.VerticalScroll.Maximum;
        }

        private void guna2TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                send_message();
            }
        }

        public static string token_mess = "", name_mess = "", persoane_mess = "", admini_mess = "";

        private async void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.FileName = "";
                opf.Filter = "Files (*.jpg; *.jpeg; *.png; *.svg; *.webp; *.bmp; *.doc; *.docx; *.ppt; *.pptx; *.xlsx; *.xls; *.txt; *.pdf; *.zip; *.rar) " +
                    "| *.jpg; *.jpeg; *.png; *.svg; *.webp; *.bmp; *.doc; *.docx; *.ppt; *.pptx; *.xlsx; *.xls; *.txt; *.pdf; *.zip; *.rar";
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
                        multiple_class _class = new multiple_class();

                        string token_file = _class.generate_token_250();

                        var data = new Dictionary<string, string>();
                        data.Add("token", schoolsync.token);
                        data.Add("token_user", Convert.ToString(login_signin.login.accounts_user["token"]));
                        data.Add("token_file", token_file);
                        data.Add("filename", fl.Name);

                        await _class.UploadFileAsync(data, opf.FileName);

                        string fname = Convert.ToString(login_signin.login.accounts_user["token"]) + "/" + token_file + "/" + fl.Name;

                        JObject json = new JObject();
                        json.Add("date", DateTime.Now.ToString());
                        json.Add("file", fname);
                        json.Add("root", "0");
                        json.Add("text", "");
                        json.Add("user", Convert.ToString(login_signin.login.accounts_user["username"]));
                        json.Add("user_token", Convert.ToString(login_signin.login.accounts_user["token"]));
                        json.Add("is_deleted", "0");

                        
                        string url = "https://schoolsync.nnmadalin.me/api/get.php";
                        data = new Dictionary<string, string>();
                        data.Add("token", schoolsync.token);
                        data.Add("command", "select * from flowtalk where token = ?");

                        var param = new Dictionary<string, string>()
                        {
                            {"token", token_message}
                        };

                        data.Add("params", JsonConvert.SerializeObject(param));

                        dynamic task = await _class.PostRequestAsync(url, data);
                        if (task["message"] == "success")
                        {
                            dynamic x = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["messages"]));
                            JObject jbo = x;
                            jbo.Add(jbo.Count.ToString(), json);

                            url = "https://schoolsync.nnmadalin.me/api/put.php";
                            data = new Dictionary<string, string>();
                            data.Add("token", schoolsync.token);
                            data.Add("command", "update flowtalk set messages = ? where token = ?");
                            param = new Dictionary<string, string>()
                            {
                                {"messages", JsonConvert.SerializeObject(jbo)},
                                {"token", token_message}
                            };

                            data.Add("params", JsonConvert.SerializeObject(param));

                            task = await _class.PostRequestAsync(url, data);
                            if (task["message"] != "update success")
                            {
                                var frm = new notification.error();
                                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                                panel.Controls.Add(frm);
                                notification.error.message = "Ceva nu a mers bine :(!";
                                frm.BringToFront();
                            }
                            flowLayoutPanel2.VerticalScroll.Value = flowLayoutPanel2.VerticalScroll.Maximum;
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Ceva nu a mers bine!";
                Console.WriteLine(ex.Message.ToString());
                frm.BringToFront();
            }
        }

        private async void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            guna2MessageDialog1.Caption = "Vrei sa parasesti grupul?";
            guna2MessageDialog1.Text = "Esti sigur ca vrei sa parasesti grupul?";
            DialogResult dr = guna2MessageDialog1.Show();
            if(dr == DialogResult.Yes)
            {
                guna2Panel3.Visible = false;
                flowLayoutPanel2.Visible = false;
                guna2TextBox1.Visible = false;
                guna2Button1.Visible = false;

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
                if(task["message"] == "success")
                {
                    string people = task["0"]["people"];
                    string[] split = people.Split(';');
                    string new_people = "";

                    for(int i = 0; i < split.Length - 1; i++)
                    {
                        if(split[i] != Convert.ToString(login_signin.login.accounts_user["token"]))
                        {
                            new_people += (split[i] + ";");
                        }
                    }

                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "update flowtalk set people = ?, messages = ? where token = ?");

                    JObject json = new JObject();
                    json.Add("date", DateTime.Now.ToString());
                    json.Add("file", "");
                    json.Add("root", "1");
                    json.Add("text", Convert.ToString(login_signin.login.accounts_user["username"]) + " a parasit grupul!");
                    json.Add("user", "");
                    json.Add("user_token", "");
                    json.Add("is_deleted", "0");

                    dynamic x = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["messages"]));
                    JObject jbo = x;
                    jbo.Add(jbo.Count.ToString(), json);

                    param = new Dictionary<string, string>()
                    {
                        {"people", new_people},
                        {"messages", JsonConvert.SerializeObject(jbo)},
                        {"token", token_message}
                    };

                    data.Add("params", JsonConvert.SerializeObject(param));

                    task = await _class.PostRequestAsync(url, data);
                }
            }
        }

        private void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            navbar_home.page = "FlowTalk_editare";
            navbar_home.use = false;
        }

        private void click_chip(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Chip;
            System.Diagnostics.Process.Start(@"https://schoolsync.nnmadalin.me/attachments/" + btn.Tag.ToString());
        }

        private async void delete_bnt(object sender, EventArgs e)
        {
            guna2MessageDialog1.Caption = "Sterge mesaj";
            guna2MessageDialog1.Text = "Esti sigur ca vrei sa stergi acest mesaj?";
            DialogResult dr = guna2MessageDialog1.Show();
            if (dr == DialogResult.Yes)
            {
                string id = ((Control)sender).Tag.ToString();
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
                if (task["message"] == "success")
                {
                    dynamic x = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["messages"]));

                    try
                    {
                        x[id]["text"] = "[ Mesaj sters de: " + Convert.ToString(login_signin.login.accounts_user["username"]) + " ]";
                        x[id]["file"] = "";
                        x[id]["is_deleted"] = "1";                      

                        url = "https://schoolsync.nnmadalin.me/api/put.php";
                        data = new Dictionary<string, string>();
                        data.Add("token", schoolsync.token);
                        data.Add("command", "update flowtalk set messages = ?, seen = ? where token = ?");
                        param = new Dictionary<string, string>()
                        {
                            {"messages", JsonConvert.SerializeObject(x)},
                            {"seen", ""},
                            {"token", token_message}
                        };

                        data.Add("params", JsonConvert.SerializeObject(param));

                        task = await _class.PostRequestAsync(url, data);
                        if (task["message"] != "update success")
                        {
                            var frm = new notification.error();
                            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                            panel.Controls.Add(frm);
                            notification.error.message = "Ceva nu a mers bine :(!";
                            frm.BringToFront();
                        }

                    }
                    catch { };
                }
            }
        }

        private async void redirect_profile(object sender, EventArgs e)
        {
            string tag = ((Control)sender).Tag.ToString();

            navbar_home.page = "Profil_person";
            navbar_home.use = false;
            navbar_home.token_page = tag;

        }

        private async void load_message_Tick(object sender, EventArgs e)
        {
            if(use == true)
            {
                count_message = "";
                use = false;
            }
            try
            {
                if (token_message != "")
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
     
                    if (task["message"] == "success")
                    {
                        string[] components = Convert.ToString(task["0"]["color"]).Split(',');
                        int red = int.Parse(components[0]);
                        int green = int.Parse(components[1]);
                        int blue = int.Parse(components[2]);

                        guna2CirclePictureBox2.FillColor = Color.FromArgb(red, green, blue);

                        label4.Text = task["0"]["name"];
                        name_mess = task["0"]["name"];
                        token_mess = token_message;
                        persoane_mess = task["0"]["people"];
                        admini_mess = task["0"]["admins"];

                        dynamic sub = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["messages"]));

                        JObject jb = sub;
                        if (count_message == "")
                        {
                            flowLayoutPanel2.Controls.Clear();
                        }

                        bool seen = false;
                        string isme = task["0"]["seen"];
                        string[] sple = isme.Split(';');

                        for (int i = 0; i < sple.Length - 1; i++)
                        {
                            if(sple[i] == Convert.ToString(login_signin.login.accounts_user["token"]))
                            {
                                seen = true;
                            }
                        }


                        if (seen == false)
                        {
                            count_message = "";
                            flowLayoutPanel2.Controls.Clear();
                        }

                        if (count_message != jb.Count.ToString())
                        {
                            go = false;

                            if (seen == false)
                            {
                                isme = isme + Convert.ToString(login_signin.login.accounts_user["token"]) + ";";

                                url = "https://schoolsync.nnmadalin.me/api/put.php";
                                data = new Dictionary<string, string>();
                                data.Add("token", schoolsync.token);
                                data.Add("command", "update flowtalk set seen = ? where token = ?");
                                param = new Dictionary<string, string>()
                                {
                                    {"seen", isme},
                                    {"token", token_message}
                                };
                                data.Add("params", JsonConvert.SerializeObject(param));
                                task = await _class.PostRequestAsync(url, data);
                            }
                            
                            string admin = task["0"]["admins"];
                            string[] spl = admin.Split(';');
                            bool isadmin = false;
                            for (int i = 0; i < spl.Length - 1; i++)
                            {
                                if(spl[i] == Convert.ToString(login_signin.login.accounts_user["token"]))
                                {
                                    isadmin = true;
                                    break;
                                }
                            }
                            int x = 0;
                            try
                            {
                                x = Convert.ToInt32(count_message);
                            }
                            catch { };
                            count_message = jb.Count.ToString();

                            

                            for (int i = x; i < jb.Count; i++)
                            {

                                Guna.UI2.WinForms.Guna2Panel pnl = new Guna.UI2.WinForms.Guna2Panel()
                                {
                                    MinimumSize = new Size(800, 30),
                                    MaximumSize = new Size(800, 0),
                                    AutoSize = true,
                                    Margin = new Padding(0, 0, 0, 15),
                                };
                                
                                if (sub[i.ToString()]["root"] == "0")
                                {
                                    if (sub[i.ToString()]["user"] == login_signin.login.accounts_user["username"] && sub[i.ToString()]["file"] == "")
                                    {
                                        Guna.UI2.WinForms.Guna2CirclePictureBox pct = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                                        {
                                            Size = new Size(40, 40),
                                            Location = new Point(760, 5),
                                            FillColor = Color.White,
                                            Tag = sub[i.ToString()]["user_token"],
                                            SizeMode = PictureBoxSizeMode.StretchImage,
                                            Cursor = Cursors.Hand,
                                        };
                                        pct.Click += redirect_profile;
                                        pnl.Controls.Add(pct);

                                        pct.Image = await _class.IncarcaAvatar(Convert.ToString(sub[i.ToString()]["user_token"]));

                                        Label lbl_username = new Label()
                                        {
                                            AutoSize = true,
                                            Location = new Point(0, 5),
                                            MinimumSize = new Size(750, 10),
                                            MaximumSize = new Size(750, 0),
                                            Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                            ForeColor = Color.White,
                                            Text = sub[i.ToString()]["user"],
                                            TextAlign = ContentAlignment.TopRight,
                                            Tag = sub[i.ToString()]["user_token"],
                                            Cursor = Cursors.Hand,
                                        };

                                        lbl_username.Click += redirect_profile;
                                        pnl.Controls.Add(lbl_username);

                                        Label lbl_date = new Label()
                                        {
                                            AutoSize = true,
                                            Location = new Point(0, 25),
                                            MinimumSize = new Size(750, 10),
                                            MaximumSize = new Size(750, 0),
                                            Font = new Font("Segoe UI", 9, FontStyle.Bold),
                                            ForeColor = Color.FromArgb(219, 219, 219),
                                            Text = sub[i.ToString()]["date"],
                                            TextAlign = ContentAlignment.TopRight,
                                        };
                                        pnl.Controls.Add(lbl_date);

                                        Label lbl = new Label()
                                        {
                                            AutoSize = true,
                                            Location = new Point(0, 50),
                                            MinimumSize = new Size(750, 10),
                                            MaximumSize = new Size(750, 0),
                                            Font = new Font("Segoe UI", 10),
                                            ForeColor = Color.White,
                                            Text = sub[i.ToString()]["text"],
                                            TextAlign = ContentAlignment.TopRight,
                                        };
                                        pnl.Controls.Add(lbl);

                                        if (Convert.ToString(sub[i.ToString()]["is_deleted"]) == "1")
                                            lbl.Font = new Font("Segoe UI", 10, FontStyle.Bold);

                                    }
                                    else if (sub[i.ToString()]["user"] == login_signin.login.accounts_user["username"] && sub[i.ToString()]["file"] != "")
                                    {
                                        Guna.UI2.WinForms.Guna2CirclePictureBox pct = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                                        {
                                            Size = new Size(40, 40),
                                            Location = new Point(760, 5),
                                            FillColor = Color.White,
                                        };
                                       
                                        pnl.Controls.Add(pct);

                                        Label lbl_username = new Label()
                                        {
                                            AutoSize = true,
                                            Location = new Point(0, 5),
                                            MinimumSize = new Size(750, 10),
                                            MaximumSize = new Size(750, 0),
                                            Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                            ForeColor = Color.White,
                                            Text = sub[i.ToString()]["user"],
                                            TextAlign = ContentAlignment.TopRight,
                                        };
                                        
                                        pnl.Controls.Add(lbl_username);

                                        Label lbl_date = new Label()
                                        {
                                            AutoSize = true,
                                            Location = new Point(0, 25),
                                            MinimumSize = new Size(750, 10),
                                            MaximumSize = new Size(750, 0),
                                            Font = new Font("Segoe UI", 9, FontStyle.Bold),
                                            ForeColor = Color.FromArgb(219, 219, 219),
                                            Text = sub[i.ToString()]["date"],
                                            TextAlign = ContentAlignment.TopRight,
                                        };
                                        pnl.Controls.Add(lbl_date);

                                        string file = sub[i.ToString()]["file"];
                                        string[] split = file.Split('/');

                                        Guna.UI2.WinForms.Guna2Chip chip = new Guna.UI2.WinForms.Guna2Chip()
                                        {
                                            Size = new Size(150, 70),
                                            Location = new Point(600, 50),
                                            IsClosable = false,
                                            Font = new Font("Segoe UI", 10),
                                            Text = split[2],
                                            Tag = file,
                                            TextAlign = HorizontalAlignment.Center,
                                            FillColor = Color.Transparent,
                                            BorderColor = Color.FromArgb(25, 133, 255),
                                            BorderThickness = 2,
                                            AutoRoundedCorners = false,
                                            BorderRadius = 5,
                                            Cursor = Cursors.Hand,
                                        };
                                        chip.Click += click_chip;
                                        pnl.Controls.Add(chip);
                                        
                                    }
                                    else if (sub[i.ToString()]["user"] != login_signin.login.accounts_user["username"] && sub[i.ToString()]["file"] == "")
                                    {
                                        Guna.UI2.WinForms.Guna2CirclePictureBox pct = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                                        {
                                            Size = new Size(40, 40),
                                            Location = new Point(0, 5),
                                            FillColor = Color.White,
                                            Tag = sub[i.ToString()]["user_token"],
                                            SizeMode = PictureBoxSizeMode.StretchImage,
                                            Cursor = Cursors.Hand,
                                        };
                                        pct.Click += redirect_profile;
                                        pct.Image = await _class.IncarcaAvatar(Convert.ToString(sub[i.ToString()]["user_token"]));
                                        pnl.Controls.Add(pct);

                                        Label lbl_username = new Label()
                                        {
                                            AutoSize = true,
                                            Location = new Point(45, 5),
                                            MinimumSize = new Size(750, 10),
                                            MaximumSize = new Size(750, 0),
                                            Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                            ForeColor = Color.White,
                                            Text = sub[i.ToString()]["user"],
                                            TextAlign = ContentAlignment.TopLeft,
                                            Tag = sub[i.ToString()]["user_token"],
                                            Cursor = Cursors.Hand,
                                        };
                                        lbl_username.Click += redirect_profile;
                                        pnl.Controls.Add(lbl_username);

                                        Label lbl_date = new Label()
                                        {
                                            AutoSize = true,
                                            Location = new Point(45, 25),
                                            MinimumSize = new Size(750, 10),
                                            MaximumSize = new Size(750, 0),
                                            Font = new Font("Segoe UI", 9, FontStyle.Bold),
                                            ForeColor = Color.FromArgb(219, 219, 219),
                                            Text = sub[i.ToString()]["date"],
                                            TextAlign = ContentAlignment.TopLeft,
                                        };
                                        pnl.Controls.Add(lbl_date);

                                        Label lbl = new Label()
                                        {
                                            AutoSize = true,
                                            Location = new Point(45, 50),
                                            MinimumSize = new Size(750, 10),
                                            MaximumSize = new Size(750, 0),
                                            Font = new Font("Segoe UI", 10),
                                            ForeColor = Color.White,
                                            Text = sub[i.ToString()]["text"],
                                            TextAlign = ContentAlignment.TopLeft,
                                        };
                                        pnl.Controls.Add(lbl);

                                        if (Convert.ToString(sub[i.ToString()]["is_deleted"]) == "1")
                                            lbl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                                    }
                                    else if (sub[i.ToString()]["user"] != login_signin.login.accounts_user["username"] && sub[i.ToString()]["file"] != "")
                                    {
                                        Guna.UI2.WinForms.Guna2CirclePictureBox pct = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                                        {
                                            Size = new Size(40, 40),
                                            Location = new Point(0, 5),
                                            FillColor = Color.White,
                                        };
                                        pnl.Controls.Add(pct);

                                        Label lbl_username = new Label()
                                        {
                                            AutoSize = true,
                                            Location = new Point(45, 5),
                                            MinimumSize = new Size(750, 10),
                                            MaximumSize = new Size(750, 0),
                                            Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                            ForeColor = Color.White,
                                            Text = sub[i.ToString()]["user"],
                                            TextAlign = ContentAlignment.TopLeft,
                                        };
                                        pnl.Controls.Add(lbl_username);

                                        Label lbl_date = new Label()
                                        {
                                            AutoSize = true,
                                            Location = new Point(45, 25),
                                            MinimumSize = new Size(750, 10),
                                            MaximumSize = new Size(750, 0),
                                            Font = new Font("Segoe UI", 9, FontStyle.Bold),
                                            ForeColor = Color.FromArgb(219, 219, 219),
                                            Text = sub[i.ToString()]["date"],
                                            TextAlign = ContentAlignment.TopLeft,
                                        };
                                        pnl.Controls.Add(lbl_date);

                                        string file = sub[i.ToString()]["file"];
                                        string[] split = file.Split('/');

                                        Guna.UI2.WinForms.Guna2Chip chip = new Guna.UI2.WinForms.Guna2Chip()
                                        {
                                            Size = new Size(150, 70),
                                            Location = new Point(50, 50),
                                            IsClosable = false,
                                            Font = new Font("Segoe UI", 10),
                                            Text = split[2],
                                            Tag = file,
                                            TextAlign = HorizontalAlignment.Center,
                                            FillColor = Color.Transparent,
                                            BorderColor = Color.FromArgb(25, 133, 255),
                                            BorderThickness = 2,
                                            AutoRoundedCorners = false,
                                            BorderRadius = 5,
                                            Cursor = Cursors.Hand,
                                        };
                                        chip.Click += click_chip;
                                        pnl.Controls.Add(chip);
                                    }

                                    if(sub[i.ToString()]["user"] == login_signin.login.accounts_user["username"] && Convert.ToString(sub[i.ToString()]["is_deleted"]) == "0")
                                    {
                                        Guna.UI2.WinForms.Guna2CircleButton gcp = new Guna.UI2.WinForms.Guna2CircleButton()
                                        {
                                            Size = new Size(32, 32),
                                            ImageSize = new Size(20, 20),
                                            Image = SchoolSync.Properties.Resources.delete_FILL1_wght700_GRAD0_opsz48,
                                            ImageAlign = HorizontalAlignment.Center,
                                            FillColor = Color.White,
                                            Animated = true,
                                            Location = new Point(0, 5),
                                            Cursor = Cursors.Hand,
                                            Tag = i,
                                        };
                                        gcp.Click += delete_bnt;
                                        pnl.Controls.Add(gcp);
                                        gcp.BringToFront();
                                        pnl.Tag = "1";
                                    }
                                    else if(Convert.ToString(sub[i.ToString()]["is_deleted"]) == "0")
                                    {
                                        Guna.UI2.WinForms.Guna2CircleButton gcp = new Guna.UI2.WinForms.Guna2CircleButton()
                                        {
                                            Size = new Size(32, 32),
                                            ImageSize = new Size(20, 20),
                                            Image = SchoolSync.Properties.Resources.delete_FILL1_wght700_GRAD0_opsz48,
                                            ImageAlign = HorizontalAlignment.Center,
                                            FillColor = Color.White,
                                            Animated = true,
                                            Location = new Point(750, 5),
                                            Cursor = Cursors.Hand,
                                            Tag = i,
                                            Name = "deletebtn"
                                        };
                                        gcp.Click += delete_bnt;
                                        pnl.Controls.Add(gcp);
                                        gcp.BringToFront();
                                        pnl.Tag = "-1";
                                    }

                                }
                                else if (sub[i.ToString()]["root"] == "1")
                                {
                                    Label lbl = new Label()
                                    {
                                        AutoSize = true,
                                        Location = new Point(55, 15),
                                        MinimumSize = new Size(750, 10),
                                        MaximumSize = new Size(750, 0),
                                        Padding = new Padding(100, 0, 100, 0),
                                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                        ForeColor = Color.FromArgb(219, 219, 219),
                                        Text = sub[i.ToString()]["text"],
                                        TextAlign = ContentAlignment.TopCenter
                                    };
                                    pnl.Controls.Add(lbl);
                                }
                                flowLayoutPanel2.Controls.Add(pnl);
                            }

                            if (isadmin == false)
                            {
                                foreach (Control ctrl in flowLayoutPanel2.Controls)
                                {
                                    try
                                    {
                                        if (ctrl.Tag != null && ctrl.Tag.ToString() == "-1")
                                        {
                                            try
                                            {
                                                ctrl.Controls["deletebtn"].Visible = false;
                                            }
                                            catch { };
                                        }
                                    }
                                    catch { };
                                }
                            }
                            flowLayoutPanel2.VerticalScroll.Value = flowLayoutPanel2.VerticalScroll.Maximum;
                        }
                    }
                }
            }
            catch { };
        }

        private async void load_tab_message_Tick(object sender, EventArgs e)
        {
            load_tab();
        }
    }
}
