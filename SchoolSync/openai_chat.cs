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
using System.Net.Http;

namespace SchoolSync
{
    public partial class openai_chat : Form
    {
        public openai_chat()
        {
            InitializeComponent();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        async void show_loading()
        {
            Guna.UI2.WinForms.Guna2WinProgressIndicator loading = new Guna.UI2.WinForms.Guna2WinProgressIndicator()
            {
                Size = new Size(90, 90),
                BackColor = Color.Transparent,
                UseTransparentBackground = true,
                AutoStart = true,
                Location = new Point((594 - 90) / 2, (632 - 90) / 2),
                Name = "panel_loading"
            };
            this.Enabled = false;
            this.Controls.Add(loading);
            loading.Show();
            loading.BringToFront();
        }

        async void hide_loading()
        {
            this.Controls.Remove(this.Controls["panel_loading"]);
            this.Enabled = true;
        }

        async Task send_api_openai(string message)
        {
            try
            {
                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                var data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from chatgpt");

                dynamic task = await _class.PostRequestAsync(url, data);
                if (task["message"] == "success")
                {
                    string tokenSecret = Convert.ToString(task["0"]["token_api"]);

                    if (tokenSecret == "")
                        return ;

                    JObject subjson = new JObject();
                    subjson.Add("role", "user");
                    subjson.Add("content", message);

                    JObject json = new JObject();
                    json.Add("model", "gpt-3.5-turbo");
                    json.Add("messages", new JArray(subjson));
                    json.Add("max_tokens", 500);

                    show_loading();

                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSecret);
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        var content = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");

                        var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                        var responseContent = await response.Content.ReadAsStringAsync();
                        dynamic jsoncontent = JsonConvert.DeserializeObject(responseContent);
                        if (response.IsSuccessStatusCode)
                        {
                            try
                            {
                                message = jsoncontent["choices"][0]["message"]["content"];

                                JObject jsonnew = new JObject();
                                jsonnew.Add("date", DateTime.Now.ToString());
                                jsonnew.Add("text", message);
                                jsonnew.Add("is_ai", "1");

                                url = "https://schoolsync.nnmadalin.me/api/get.php";
                                data = new Dictionary<string, string>();
                                data.Add("token", schoolsync.token);
                                data.Add("command", "select * from accounts where token = ?");

                                var param = new Dictionary<string, string>()
                                {
                                    {"token", Convert.ToString(login_signin.login.accounts_user["token"])}
                                };

                                data.Add("params", JsonConvert.SerializeObject(param));

                                task = await _class.PostRequestAsync(url, data);

                                hide_loading();

                                if (task["message"] == "success")
                                {
                                    dynamic x = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["chatgpt_message"]));
                                    JObject jbo = x;
                                    jbo.Add(jbo.Count.ToString(), jsonnew);

                                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                                    data = new Dictionary<string, string>();
                                    data.Add("token", schoolsync.token);
                                    data.Add("command", "update accounts set chatgpt_message = ? where token = ?");
                                    param = new Dictionary<string, string>()
                                    {
                                        {"chatgpt_message", JsonConvert.SerializeObject(jbo)},
                                        {"token", Convert.ToString(login_signin.login.accounts_user["token"])}
                                    };

                                    data.Add("params", JsonConvert.SerializeObject(param));

                                    task = await _class.PostRequestAsync(url, data);
                                    if (task["message"] != "update success")
                                    {
                                        guna2MessageDialog1.Caption = "Eroare";
                                        guna2MessageDialog1.Text = "Ceva nu a mers bine cu API!";
                                    }
                                }
                            }
                            catch
                            {
                                guna2MessageDialog1.Caption = "Eroare";
                                guna2MessageDialog1.Text = "Ceva nu a mers bine cu API!";
                                guna2MessageDialog1.Show();
                                hide_loading();
                            }
                        }
                        else
                        {
                            guna2MessageDialog1.Caption = "Eroare";
                            guna2MessageDialog1.Text = "Ceva nu a mers bine cu API. Token invalid?";
                            guna2MessageDialog1.Show();
                            hide_loading();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Caption = "Eroare";
                guna2MessageDialog1.Text = "Ceva nu a mers bine!";
                guna2MessageDialog1.Show();
                hide_loading();
            }
            hide_loading();
        }

        async void send_message()
        {
            string message = guna2TextBox1.Text;

            JObject json = new JObject();
            json.Add("date", DateTime.Now.ToString());
            json.Add("text", message);
            json.Add("is_ai", "0");

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from accounts where token = ?");

            var param = new Dictionary<string, string>()
            {
                {"token", Convert.ToString(login_signin.login.accounts_user["token"])}
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                string time = Convert.ToString(task["0"]["chatgpt_time"]);
                DateTime dt;
                if (time != "")
                {
                    dt = Convert.ToDateTime(time);
                    if (dt.AddMinutes(2) > DateTime.Now)
                    {
                        guna2MessageDialog1.Show();
                        return;
                    }
                }

                dynamic x = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["chatgpt_message"]));
                JObject jbo = x;
                if (jbo == null)
                    jbo = new JObject();
                jbo.Add(jbo.Count.ToString(), json);

                url = "https://schoolsync.nnmadalin.me/api/put.php";
                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "update accounts set chatgpt_message = ?, chatgpt_time = ? where token = ?");
                param = new Dictionary<string, string>()
                {
                    {"chatgpt_message", JsonConvert.SerializeObject(jbo)},
                    {"chatgpt_time", Convert.ToString(DateTime.Now)},
                    {"token", Convert.ToString(login_signin.login.accounts_user["token"])}
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                task = await _class.PostRequestAsync(url, data);
                if (task["message"] != "update success")
                {
                    guna2MessageDialog1.Caption = "Eroare";
                    guna2MessageDialog1.Text = "Ceva nu a mers bine cu API!";
                    guna2MessageDialog1.Show();
                }
                else
                {
                    send_api_openai(message);
                }
            }

            guna2TextBox1.Clear();
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            if(guna2TextBox1.Text.Trim()  != "")
            {
                send_message();
            }
        }

        private void guna2TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                send_message();
            }
        }

        private void openai_chat_Load(object sender, EventArgs e)
        {
            check_code();
        }

        string count_message = "";

        private async void timer1_Tick(object sender, EventArgs e)
        {
            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from accounts where token = ?");

            var param = new Dictionary<string, string>()
            {
                {"token", Convert.ToString(login_signin.login.accounts_user["token"])}
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);

            if (task["message"] == "success")
            {

                dynamic sub = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["chatgpt_message"]));

                JObject jb = sub;
                if (count_message == "")
                {
                    flowLayoutPanel1.Controls.Clear();
                }

                if (jb == null)
                    jb = new JObject();

                if (count_message != jb.Count.ToString())
                {
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
                            MinimumSize = new Size(500, 30),
                            MaximumSize = new Size(500, 0),
                            AutoSize = true,
                            Margin = new Padding(0, 0, 0, 15),
                        };

                        if (sub[i.ToString()]["is_ai"] == "0")
                        {
                            Guna.UI2.WinForms.Guna2CirclePictureBox pct = new Guna.UI2.WinForms.Guna2CirclePictureBox() 
                            {
                                Size = new Size(40, 40),
                                Location = new Point(460, 5),
                                FillColor = Color.Black,
                                SizeMode = PictureBoxSizeMode.StretchImage,
                            };
                            pnl.Controls.Add(pct);

                            pct.Image = await _class.IncarcaAvatar(Convert.ToString(login_signin.login.accounts_user["token"]));

                            Label lbl_username = new Label()
                            {
                                AutoSize = true,
                                Location = new Point(0, 5),
                                MinimumSize = new Size(450, 10),
                                MaximumSize = new Size(450, 0),
                                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                ForeColor = Color.Black,
                                Text = Convert.ToString(login_signin.login.accounts_user["username"]),
                                TextAlign = ContentAlignment.TopRight,
                            };
                            pnl.Controls.Add(lbl_username);

                            Label lbl_date = new Label()
                            {
                                AutoSize = true,
                                Location = new Point(0, 25),
                                MinimumSize = new Size(450, 10),
                                MaximumSize = new Size(450, 0),
                                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                                ForeColor = Color.FromArgb(74, 74, 74),
                                Text = sub[i.ToString()]["date"],
                                TextAlign = ContentAlignment.TopRight,
                            };
                            pnl.Controls.Add(lbl_date);

                            Label lbl = new Label()
                            {
                                AutoSize = true,
                                Location = new Point(0, 50),
                                MinimumSize = new Size(450, 10),
                                MaximumSize = new Size(450, 0),
                                Font = new Font("Segoe UI", 10),
                                ForeColor = Color.Black,
                                Text = sub[i.ToString()]["text"],
                                TextAlign = ContentAlignment.TopRight,
                            };
                            pnl.Controls.Add(lbl);
                        }
                        else
                        {
                            Guna.UI2.WinForms.Guna2CirclePictureBox pct = new Guna.UI2.WinForms.Guna2CirclePictureBox()
                            {
                                Size = new Size(40, 40),
                                Location = new Point(0, 5),
                                FillColor = Color.White,
                                SizeMode = PictureBoxSizeMode.StretchImage,
                            };
                            pnl.Controls.Add(pct);

                            pct.Image = SchoolSync.Properties.Resources.ChatGPT_logo_svg;

                            Label lbl_username = new Label()
                            {
                                AutoSize = true,
                                Location = new Point(45, 5),
                                MinimumSize = new Size(450, 10),
                                MaximumSize = new Size(450, 0),
                                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                ForeColor = Color.Black,
                                Text = "AI (gpt-3.5-turbo)",
                                TextAlign = ContentAlignment.TopLeft,
                            };
                            pnl.Controls.Add(lbl_username);

                            Label lbl_date = new Label()
                            {
                                AutoSize = true,
                                Location = new Point(45, 25),
                                MinimumSize = new Size(450, 10),
                                MaximumSize = new Size(450, 0),
                                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                                ForeColor = Color.FromArgb(74, 74, 74),
                                Text = sub[i.ToString()]["date"],
                                TextAlign = ContentAlignment.TopLeft,
                            };
                            pnl.Controls.Add(lbl_date);

                            Label lbl = new Label()
                            {
                                AutoSize = true,
                                Location = new Point(45, 50),
                                MinimumSize = new Size(450, 10),
                                MaximumSize = new Size(450, 0),
                                Font = new Font("Segoe UI", 10),
                                ForeColor = Color.Black,
                                Text = sub[i.ToString()]["text"],
                                TextAlign = ContentAlignment.TopLeft,
                            };
                            pnl.Controls.Add(lbl);
                        }

                        flowLayoutPanel1.Controls.Add(pnl);
                    }


                    flowLayoutPanel1.VerticalScroll.Value = flowLayoutPanel1.VerticalScroll.Maximum;
                    flowLayoutPanel1.VerticalScroll.Value = flowLayoutPanel1.VerticalScroll.Maximum;
                }
            }
        }

        private async void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            if (guna2MessageDialog2.Show() == DialogResult.Yes)
            {
                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/put.php";
                var data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "update accounts set chatgpt_message = ? where token = ?");

                var param = new Dictionary<string, string>()
                {
                    {"chatgpt_message", "{}"},
                    {"token", Convert.ToString(login_signin.login.accounts_user["token"])}
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                dynamic task = await _class.PostRequestAsync(url, data);

                if (task["message"] != "update success")
                {
                    guna2MessageDialog1.Caption = "Eroare";
                    guna2MessageDialog1.Text = "Ceva nu a mers bine!";
                    guna2MessageDialog1.Show();
                }
                else
                    count_message = "";
            }
        }

        async void check_code()
        {
            multiple_class _class = new multiple_class();

            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from chatgpt");

            dynamic task = await _class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                if (Convert.ToString(task["0"]["token_api"]).Trim() == "")
                {
                    guna2Button1.Visible = guna2CircleButton2.Visible = guna2TextBox1.Visible = false;
                }
                else
                    guna2Button1.Visible = guna2CircleButton2.Visible = guna2TextBox1.Visible = true;
            }
            else
                guna2Button1.Visible = guna2CircleButton2.Visible = guna2TextBox1.Visible = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            check_code();
        }
    }
}
