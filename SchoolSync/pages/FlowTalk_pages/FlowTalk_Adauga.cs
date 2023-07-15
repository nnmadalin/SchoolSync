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
    public partial class FlowTalk_Adauga : UserControl
    {
        public FlowTalk_Adauga()
        {
            InitializeComponent();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            guna2MessageDialog1.Caption = "Inchide";
            guna2MessageDialog1.Text = "Esti sigur ca vrei sa inchizi pagina?";

            if (guna2MessageDialog1.Show() == DialogResult.Yes)
            {
                if (navbar_home.page == "FlowTalk_editare")
                {
                    this.Dispose();
                }
                else
                {
                    navbar_home.page = "FlowTalk";
                    navbar_home.use = false;
                }
            }
        }

        private async void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            string nume_cautat = guna2TextBox2.Text;      
        }

        async Task<string> get_token_name(string token)
        {
            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from accounts where token = ?");
            dynamic task = await _class.PostRequestAsync(url, data);

            var param = new Dictionary<string, string>()
            {
                {"token", token}
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            task = await _class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                return task["0"]["username"];
            }
            return "";
        }

        private async  void FlowTalk_Adauga_Load(object sender, EventArgs e)
        {

            schoolsync.show_loading();

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from accounts");
            dynamic task = await _class.PostRequestAsync(url, data);

            if (task["message"] == "success")
            {
                AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
                JObject jb = task;
                for (int i = 0; i < jb.Count - 1; i++)
                {
                    string username = Convert.ToString(task[i.ToString()]["username"]);
                    collection.Add(username);

                }
                guna2TextBox2.AutoCompleteCustomSource = collection;
                guna2TextBox3.AutoCompleteCustomSource = collection;
            }


            string[] split2 = FlowTalk.admini_mess.Split(';');

            bool admin = false;

            for(int i = 0; i < split2.Length - 1; i++)
            {
                if (split2[i] == Convert.ToString(login_signin.login.accounts_user["token"]))
                    admin = true;
            }

            if(navbar_home.page == "FlowTalk_editare")
            {
                if (admin == true)
                {
                    label1.Text = "Detalii grup";
                    guna2Button2.Text = "Modifica grupul";                    
                }
                else
                {
                    label1.Text = "Detalii grup";

                    guna2Button1.Visible = false;
                    guna2Button3.Visible = false;
                    guna2Button2.Visible = false;
                    guna2TextBox1.Enabled = false;
                    guna2TextBox2.Visible = false;
                    guna2TextBox3.Visible = false;
                    label3.Text = "Persoane: ";
                    label4.Text = "Admini: ";
                }
                guna2TextBox1.Text = FlowTalk.name_mess;
                //load db

                url = "https://schoolsync.nnmadalin.me/api/get.php";
                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from flowtalk where token = ?");

                var param = new Dictionary<string, string>()
                {
                    {"token", FlowTalk.token_mess}
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                task = await _class.PostRequestAsync(url, data);
                if (task["message"] == "success")
                {
                    string persoane_grup = task["0"]["people"];
                    string admin_grup = task["0"]["admins"];
                    string[] split = persoane_grup.Split(';');
                    for(int i = 0; i < split.Length - 1; i++)
                    {
                        string name = await get_token_name(split[i]);
                        Guna.UI2.WinForms.Guna2Chip chip = new Guna.UI2.WinForms.Guna2Chip()
                        {
                            Size = new Size(200, 50),
                            IsClosable = true,
                            AutoSize = true,
                            TextAlign = HorizontalAlignment.Left,
                            FillColor = Color.Transparent,
                            BorderColor = Color.FromArgb(25, 133, 255),
                            ForeColor = Color.Black,
                            BorderThickness = 2,
                            AutoRoundedCorners = false,
                            BorderRadius = 5,
                        };
                        chip.Tag = split[i];
                        chip.Text = name;
                        flowLayoutPanel1.Controls.Add(chip);
                        if (split[i] == Convert.ToString(login_signin.login.accounts_user["token"]))
                        {
                            chip.IsClosable = false;
                        }
                        if(admin == false)
                            chip.IsClosable = false;
                    }

                    split = admin_grup.Split(';');
                    for (int i = 0; i < split.Length - 1; i++)
                    {                        
                        string name = await get_token_name(split[i]);
                        Guna.UI2.WinForms.Guna2Chip chip = new Guna.UI2.WinForms.Guna2Chip()
                        {
                            Size = new Size(200, 50),
                            IsClosable = true,
                            AutoSize = true,
                            TextAlign = HorizontalAlignment.Left,
                            FillColor = Color.Transparent,
                            BorderColor = Color.FromArgb(25, 133, 255),
                            ForeColor = Color.Black,
                            BorderThickness = 2,
                            AutoRoundedCorners = false,
                            BorderRadius = 5,
                        };
                        chip.Tag = split[i];
                        chip.Text = name;
                        flowLayoutPanel2.Controls.Add(chip);
                        if (split[i] == Convert.ToString(login_signin.login.accounts_user["token"]))
                        {
                            chip.IsClosable = false;
                        }
                        if (admin == false)
                            chip.IsClosable = false;
                    }

                }                
            }
            else
            {
                Guna.UI2.WinForms.Guna2Chip chip2 = new Guna.UI2.WinForms.Guna2Chip()
                {
                    Size = new Size(200, 50),
                    IsClosable = false,
                    AutoSize = true,
                    TextAlign = HorizontalAlignment.Left,
                    FillColor = Color.Transparent,
                    BorderColor = Color.FromArgb(25, 133, 255),
                    ForeColor = Color.Black,
                    BorderThickness = 2,
                    AutoRoundedCorners = false,
                    BorderRadius = 5,
                };
                chip2.Tag = login_signin.login.accounts_user["token"];
                chip2.Text = login_signin.login.accounts_user["username"];
                flowLayoutPanel2.Controls.Add(chip2);
            }
            schoolsync.hide_loading();
        }

        async void add_user()
        {
            if (guna2TextBox2.Text.Trim() != "")
            {
                Guna.UI2.WinForms.Guna2Chip chip = new Guna.UI2.WinForms.Guna2Chip()
                {
                    Size = new Size(350, 90),
                    Font = new Font("Segoe UI", 15),
                    IsClosable = true,
                    AutoSize = true,
                    TextAlign = HorizontalAlignment.Left,
                    FillColor = Color.Transparent,
                    BorderColor = Color.FromArgb(25, 133, 255),
                    ForeColor = Color.Black,
                    BorderThickness = 2,
                    AutoRoundedCorners = false,
                    BorderRadius = 5,
                };

                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                var data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from accounts where username like ?");

                var param = new Dictionary<string, string>()
                {
                    {"username", "%" + guna2TextBox2.Text +"%"}
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                dynamic task = await _class.PostRequestAsync(url, data);
                if (task["message"] == "success")
                {
                    chip.Tag = task["0"]["token"];
                    chip.Text = task["0"]["username"];

                    if (task["0"]["token"] == login_signin.login.accounts_user["token"])
                    {
                        var frm = new notification.error();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.error.message = "Nu poti sa te adaugi pe tine :) !";
                        frm.BringToFront();
                    }
                    else
                    {
                        bool ok = false;
                        foreach (Control ctrl in flowLayoutPanel1.Controls)
                        {
                            if (ctrl.Tag.ToString() == Convert.ToString(task["0"]["token"]))
                            {
                                var frm = new notification.error();
                                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                                panel.Controls.Add(frm);
                                notification.error.message = "Ai adaugat deja aceasta persoana!";
                                frm.BringToFront();
                                ok = true;
                                break;
                            }
                        }
                        if (ok == false)
                        {
                            flowLayoutPanel1.Controls.Add(chip);
                            guna2TextBox2.Clear();
                        }
                    }
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            add_user();      
        }

        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            if (navbar_home.page != "FlowTalk_editare")
            {
                schoolsync.show_loading();
                string name_group = guna2TextBox1.Text;
                string persoane_token = Convert.ToString(login_signin.login.accounts_user["token"]) + ";";
                string persoane_admin_token = "";
                if (guna2TextBox1.Text.Trim() == "")
                {
                    name_group = login_signin.login.accounts_user["username"];
                    foreach (Control ctrl in flowLayoutPanel1.Controls)
                    {
                        name_group += (", " + ctrl.Text.ToString());
                    }
                }
                foreach (Control ctrl in flowLayoutPanel1.Controls)
                {
                    persoane_token += (ctrl.Tag.ToString() + ";");
                }
                foreach (Control ctrl in flowLayoutPanel2.Controls)
                {
                    persoane_admin_token += (ctrl.Tag.ToString() + ";");
                }                

                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/post.php";
                var data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "insert into flowtalk(token, token_user, created, name, color, people, messages, seen, admins) values (?, ?, ?, ?, ?, ? ,?, ?, ?)");

                string token = _class.generate_token();

                JObject sub_json = new JObject();
                JObject jbo = new JObject();

                sub_json.Add("root", "1");
                sub_json.Add("user", "");
                sub_json.Add("user_token", "");
                sub_json.Add("date", DateTime.Now.ToString());
                sub_json.Add("file", "");
                sub_json.Add("text", Convert.ToString(login_signin.login.accounts_user["username"]) + " a creat grupul: '" + name_group + "'");
                sub_json.Add("is_deleted", "0");
                jbo.Add("0", sub_json);

                Random random = new Random();
                string randomColor = random.Next(256).ToString() + ", " + random.Next(256).ToString() + ", " + random.Next(256).ToString();

                var param = new Dictionary<string, string>()
                {
                    {"token", token},
                    {"token_user", Convert.ToString(login_signin.login.accounts_user["token"])},
                    {"created", Convert.ToString(login_signin.login.accounts_user["username"])},
                    {"name", name_group},
                    {"color", randomColor.ToString()},
                    {"people", persoane_token},
                    {"messages", JsonConvert.SerializeObject(jbo)},
                    {"seen", ""},
                    {"admins", persoane_admin_token},
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                dynamic task = await _class.PostRequestAsync(url, data);
                schoolsync.hide_loading();
                if (task["message"] == "insert success")
                {
                    var frm = new notification.success();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.success.message = "Conversatie creata cu succes!";
                    frm.BringToFront();
                    navbar_home.page = "FlowTalk";
                    navbar_home.use = false;
                }
                else
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Ceva nu mers bine, mai incearca!";
                    frm.BringToFront();
                }
            }
            else 
            {
                //editare

                schoolsync.show_loading();
                string name_group = guna2TextBox1.Text;
                string persoane_token = "";
                string persoane_admin_token = "";
                
                foreach (Control ctrl in flowLayoutPanel1.Controls)
                {
                    persoane_token += (ctrl.Tag.ToString() + ";");
                }
                foreach (Control ctrl in flowLayoutPanel2.Controls)
                {
                    persoane_admin_token += (ctrl.Tag.ToString() + ";");
                }

                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                var data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from flowtalk where token = ?");

                var param = new Dictionary<string, string>()
                {
                    {"token", FlowTalk.token_mess},
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                dynamic task = await _class.PostRequestAsync(url, data);
                if (task["message"] == "success")
                {
                    JObject json = new JObject();
                    json.Add("date", DateTime.Now.ToString());
                    json.Add("file", "");
                    json.Add("root", "1");
                    json.Add("text",Convert.ToString(login_signin.login.accounts_user["username"]) + " a facut niste modificari!");
                    json.Add("user", "");
                    json.Add("user_token", "");
                    json.Add("is_deleted", "0");

                    dynamic x = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["messages"]));
                    JObject jbo = x;
                    jbo.Add(jbo.Count.ToString(), json);

                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "update flowtalk set name = ?, people = ?, admins = ?, messages = ?, seen = ? where token = ?");
                    param = new Dictionary<string, string>()
                    {
                        {"name", name_group},
                        {"people", persoane_token},
                        {"admins", persoane_admin_token},
                        {"messages", JsonConvert.SerializeObject(jbo)},
                        {"seen", ""},
                        {"token",  FlowTalk.token_mess}
                    };

                    data.Add("params", JsonConvert.SerializeObject(param));

                    task = await _class.PostRequestAsync(url, data);
                    schoolsync.hide_loading();
                    if (task["message"] != "update success")
                    {
                        var frm = new notification.error();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.error.message = "Ceva nu a mers bine :(!";
                        frm.BringToFront();
                    }
                    else
                    {
                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);
                        notification.success.message = "Modificarile au fost facute cu succes!";
                        frm.BringToFront();
                        this.Dispose();
                    }

                }
                else
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Ceva nu mers bine, mai incearca!";
                    frm.BringToFront();
                }
                schoolsync.hide_loading();
            }
        }

        string token_app = schoolsync.token;

        private async void guna2Button3_Click(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text.Trim() != "")
            {
                Guna.UI2.WinForms.Guna2Chip chip = new Guna.UI2.WinForms.Guna2Chip()
                {
                    Size = new Size(350, 90),
                    Font = new Font("Segoe UI", 15),
                    IsClosable = true,
                    AutoSize = true,
                    TextAlign = HorizontalAlignment.Left,
                    FillColor = Color.Transparent,
                    BorderColor = Color.FromArgb(25, 133, 255),
                    ForeColor = Color.Black,
                    BorderThickness = 2,
                    AutoRoundedCorners = false,
                    BorderRadius = 5,
                };

                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                var data = new Dictionary<string, string>();
                data.Add("token", token_app);
                data.Add("command", "select * from accounts where username like ?");

                var param = new Dictionary<string, string>()
                {
                    {"username", "%" + guna2TextBox3.Text + "%"}
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                dynamic task = await _class.PostRequestAsync(url, data);

                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];

                if (task["message"] == "success")
                {
                    chip.Tag = task["0"]["token"];
                    chip.Text = task["0"]["username"];

                    foreach (Control ctrl in flowLayoutPanel2.Controls)
                    {
                        if (ctrl.Tag.ToString() == Convert.ToString(task["0"]["token"]))
                        {
                            frm = new notification.error();
                            schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                            panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                            panel.Controls.Add(frm);
                            notification.error.message = "Ai adaugat deja aceasta persoana!";
                            frm.BringToFront();
                            return ;
                        }
                    }

                    foreach (Control ctrl in flowLayoutPanel1.Controls)
                    {
                        if (ctrl.Tag.ToString() == Convert.ToString(task["0"]["token"]))
                        {
                            flowLayoutPanel2.Controls.Add(chip);
                            guna2TextBox3.Clear();                            
                            return;
                        }
                    }

                    frm = new notification.error();
                    schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Aceasta persoana nu este in grup!";
                    frm.BringToFront();

                }
            }
        }
    }
}
