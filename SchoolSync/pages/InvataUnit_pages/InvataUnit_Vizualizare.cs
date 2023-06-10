using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace SchoolSync.pages.InvataUnit_pages
{
    public partial class InvataUnit_Vizualizare : UserControl
    {
        public InvataUnit_Vizualizare()
        {
            InitializeComponent();
        }

        private void deschide_fisier_buton(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            System.Diagnostics.Process.Start(@"https://schoolsync.nnmadalin.me/attachments/" + btn.Tag.ToString());
        }

        private void deschide_fisier_chip(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Chip;
            System.Diagnostics.Process.Start(@"https://schoolsync.nnmadalin.me/attachments/" + btn.Tag.ToString());
        }

        private async void InvataUnit_Vizualizare_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.HorizontalScroll.Visible = false;
            schoolsync.show_loading();
            multiple_class _Class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from invataunit where token = ?");

            var param = new Dictionary<string, string>()
            {
                {"token", navbar_home.token_page},
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _Class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                string str = task["0"]["favourites"];
                string[] split = str.Split(';');
                for (int i = 0; i < split.Length - 1; i++)
                {
                    if (split[i] == Convert.ToString(login_signin.login.accounts_user["token"]))
                    {
                        guna2Button1.Image = SchoolSync.Properties.Resources.favorite_FILL1_wght700_GRAD0_opsz48;
                        guna2Button1.Tag = "1";
                        break;
                    }
                }

                label1.Text = task["0"]["created"];
                label2.Text = task["0"]["category"] + " • " + task["0"]["data"];
                richTextBox1.Rtf = task["0"]["question"];
                if (task["0"]["token_user"] == login_signin.login.accounts_user["token"])
                {
                    guna2CircleButton2.Visible = true;
                    guna2CircleButton3.Visible = true;
                }

                if (task["0"]["files"] == "")
                {
                    label4.Visible = true;
                }
                else
                {
                    string row = task["0"]["files"];
                    string[] file_split = row.Split(';');
                    for (int i = 0; i < file_split.Length; i++)
                    {
                       
                        url = "https://schoolsync.nnmadalin.me/api/get.php";
                        data = new Dictionary<string, string>();
                        data.Add("token", schoolsync.token);
                        data.Add("command", "select * from files where token_user = ? and token = ?");
                        param = new Dictionary<string, string>()
                        {
                            {"token_user", Convert.ToString(login_signin.login.accounts_user["token"])},
                            {"token", file_split[i]}
                        };
                        data.Add("params", JsonConvert.SerializeObject(param));

                        task = await _Class.PostRequestAsync(url, data);
                        if (task["message"] == "success")
                        {
                            try
                            {
                                string[] splitplit = Convert.ToString(task["0"]["name"]).Split('.');

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

                                flowLayoutPanel5.Controls.Add(flp_files_panel);
                                flp_files_panel.Controls.Add(gcp);
                                flp_files_panel.Controls.Add(lbl_panel_file);
                                
                                if (splitplit[0].Length >= 10)
                                {
                                    lbl_panel_file.Text = splitplit[0].Substring(0, 10) + "." + splitplit[1];
                                }
                                else
                                    lbl_panel_file.Text = task["0"]["name"];
                                if (splitplit[1] == "jpg" || splitplit[1] == "jpeg" || splitplit[1] == "png" || splitplit[1] == "svg" || splitplit[1] == "webp" || splitplit[1] == "bmp")
                                {
                                    gcp.Image = SchoolSync.Properties.Resources.image_FILL1_wght700_GRAD0_opsz48;
                                }
                                else
                                    gcp.Image = SchoolSync.Properties.Resources.description_FILL1_wght700_GRAD0_opsz48;
                                
                                flp_files_panel.Controls.Add(panel_file_btn);

                                panel_file_btn.Tag = Convert.ToString(login_signin.login.accounts_user["token"]) + "/" + task["0"]["token"] + "/" + task["0"]["name"];
                                panel_file_btn.Click += deschide_fisier_buton;
                            }
                            catch (Exception ee) { };
                            
                        }
                    }
                }

                
            }
            
            load_answers();
        }

        private void richTextBox1_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            ((RichTextBox)sender).Height = e.NewRectangle.Height + 5;
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            navbar_home.page = "InvataUnit";
            navbar_home.use = false;
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            navbar_home.page = "InvataUnit_editare";
            navbar_home.use = false;
        }

        private async void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            string token = navbar_home.token_page;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
            guna2MessageDialog1.Caption = "Sterge intrebare!";
            guna2MessageDialog1.Text = "Esti sigur ca vrei sa stergi intrebarea?";
            DialogResult dr = guna2MessageDialog1.Show();
            if (dr == DialogResult.Yes)
            {
                schoolsync.show_loading();

                multiple_class _Class = new multiple_class();

                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from invataunit where token = ?");
                //navbar_home.token_page
                var param = new Dictionary<string, string>()
                {
                    {"token", navbar_home.token_page},
                };
                data.Add("params", JsonConvert.SerializeObject(param));

                dynamic task = await _Class.PostRequestAsync(url, data);

                if (task["message"] == "success")
                {
                    string fisiere_value = task["0"]["files"];


                    url = "https://schoolsync.nnmadalin.me/api/delete.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "delete from invataunit where token = ?");

                    param = new Dictionary<string, string>()
                    {
                        {"token", token}
                    };
                    data.Add("params", JsonConvert.SerializeObject(param));

                    task = await _Class.PostRequestAsync(url, data);

                    string token_app = schoolsync.token;

                    if (task["message"] == "delete success")
                    {
                        string[] split_1 = fisiere_value.Split(';');
                        for (int i = 0; i < split_1.Length - 1; i++)
                        {
                            url = "https://schoolsync.nnmadalin.me/api/delete_file.php";
                            data = new Dictionary<string, string>();
                            data.Add("token", token_app);
                            data.Add("token_user", Convert.ToString(login_signin.login.accounts_user["token"]));
                            data.Add("file", split_1[i]);
                            task = await _Class.PostRequestAsync(url, data);

                            url = "https://schoolsync.nnmadalin.me/api/delete.php";
                            data = new Dictionary<string, string>();
                            data.Add("token", token_app);
                            data.Add("command", "delete from files where token = ?");

                            param = new Dictionary<string, string>()
                            {
                                {"token", split_1[i]}
                            };
                            data.Add("params", JsonConvert.SerializeObject(param));
                            task = await _Class.PostRequestAsync(url, data);
                        }

                        var frm = new notification.success();
                        schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                        var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                        panel.Controls.Add(frm);

                        notification.success.message = "Intrebare stersa cu succes!";
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
                    schoolsync.hide_loading();
                    navbar_home.page = "InvataUnit";
                    navbar_home.use = false;
                }
            }
        }

        string number_answer = "0";

        async void load_design_answers(dynamic task)
        {            
            try
            {
                schoolsync.show_loading();
                multiple_class _Class = new multiple_class();

                dynamic sub_task = task;
                
                JObject jb = JObject.Parse(sub_task);
                
                if (number_answer != jb.Count.ToString())
                {
                    flowLayoutPanel3.Controls.Clear();
                    number_answer = jb.Count.ToString();
                    for (int i = jb.Count - 1; i >= 0; i--)
                    {
                        Guna.UI2.WinForms.Guna2Panel pnl_answer = new Guna.UI2.WinForms.Guna2Panel()
                        {
                            MaximumSize = new Size(1130, 0),
                            MinimumSize = new Size(1130, 0),
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
                        //cpb_answer.Image = await _Class.IncarcaImagineAsync("https://schoolsync.nnmadalin.me/api/getfile.php?token=userfoto_" + login_signin.login.accounts_user["token"] + ".png");

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
                                multiple_class _class = new multiple_class();

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
                                    IsClosable = false,
                                    Cursor = Cursors.Hand
                                };

                                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                                var data = new Dictionary<string, string>();
                                data.Add("token", schoolsync.token);
                                data.Add("command", "select * from files where token = ? and token_user = ?");
                                var param = new Dictionary<string, string>()
                                {
                                    {"token", split[j]},
                                    {"token_user", Convert.ToString(login_signin.login.accounts_user["token"])}
                                };

                                data.Add("params", JsonConvert.SerializeObject(param));
                                task = await _class.PostRequestAsync(url, data);
                                if (task["message"] == "success")
                                {
                                    
                                    string[] splitsplit = Convert.ToString(task["0"]["name"]).Split('.');
                                    guna2Chip.Tag = login_signin.login.accounts_user["token"] + "/" + split[j].ToString() + "/" + task["0"]["name"];
                                    if (splitsplit[0].Length > 10)
                                        guna2Chip.Text = splitsplit[0].Substring(0, 10) + "___." + splitsplit[1];
                                    else
                                        guna2Chip.Text = task["0"]["name"];
                                    guna2Chip.Click += deschide_fisier_chip;
                                    flp_answer.Controls.Add(guna2Chip);
                                }
                            }
                        }


                        pnl_answer.Controls.Add(cpb_answer);
                        pnl_answer.Controls.Add(lbl_name_answer);
                        pnl_answer.Controls.Add(lbl_time_answer);
                        pnl_answer.Controls.Add(lbl_question_answer);
                        pnl_answer.Controls.Add(flp_answer);

                        flowLayoutPanel3.Controls.Add(pnl_answer);
                        flowLayoutPanel3.Controls.Add(flp_answer);
                    }
                }
                schoolsync.hide_loading();
            }
            catch(Exception e)
            {
                navbar_home.page = "InvataUnit";
                navbar_home.use = false;                

                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);
                notification.error.message = "Conexiunea oprita!";
                schoolsync.hide_loading();

            }
        }
        public static TcpClient client = null;
        async void load_answers()
        {
            string serverIP = "86.122.1.39";
            int serverPort = 3381;

            while (navbar_home.page == "InvataUnit_vizualizare")
            {
                try
                {
                    client = new TcpClient(serverIP, serverPort);

                    NetworkStream stream = client.GetStream();
                    string message = "Nj1BcXraVt0zyY47GsgZY9Ct=RUDpZDq0ZWfPb9KNBLX2hgckeWDSlqG4TEskDKviEfF2uf6S0jFE0-Nf!1B!BAakCDyHVnULK2Bgx-/37g-qrjc6!v5=3dBm2!WX=1AzVD6uIgu7nQ50s27zk6=cqTOxbpZV=vM9DpRJv6yME5-e/jE?WV95FEsKbs2qEzmo6Rz6hyQF3-oqNMOyVR5IwF-lWXT-K?0C/r/bPeSgNnJUDR?/-dLUm/34FKC20485Wxz!v6p0GOD=f8DPCDwXCMb7HpVafEEdmw!o??heIw!eYtVh7RuwlR-jKdULWwZLGn6Xx/O175kjFkGtaIvCkX8PNs3V7qMAfWXksRxaiPM!LBdL4RNTdOePZrDFmpy8v0n3z-te/f!3FN6glDfbhvgGMN=lMa0ZRp0?legyBuRlBH=tYiNo40B4t-FjBzuvzU4-ns?HunlP02Xdh7MFiHwldNTwMKVrVDnpirLY6IFL6Wx=PqyO-X?OkE?aZWJf25SbqTNm7?YcMkC75pzb-xzOM4UEAGp61OgbVr4UR549-/6CU-tC/zvwcHG5fx!9KrSB=C7CcLq!bMjTgni=7kyagAjM-eDRCrCV--Ah!qlkQcwZ1mms!Ulh7vKeE-qIUPPnGvj2hSEhxI2kmyMCTy3XJPh3lWeyr3kTyp6aNx97AXcAYfY1Xp/sqIHG3HU2kpu4Vytx-2bLl/uhdRbTXgo/P8pKh/zPU!iluLE3JTcj!XWZGhofkEy/VJ?sF4JQ665NeFuNq=Ak7wX-cTpBbi3/F4GxerghCUPAD9da9mG!oHnvdfb68WWrlXXhd=BBSWJoul=u8?4CGDB5z/TjDzemA3HZK8UotQeZnX2jB4xbiEhnD/-nb9H4mdsf8zx?EQi1D44IMC8Y7y7-DuTKUixx0Y/N=4vv4awi2Qv/tHgKODkc5=kH/zcrZvKeBDQTijuLpk-GZy?xp?WEjNYnjJwN5k32=UxqEJ9F?XlSOZ?vxhZ5G4WYXP77j2Cbb5thsFRyVNyKXiCux-ZQ/!D=KKYSSCXpaEzjAPUwMHXsI0yPC2?J5N1TRHvQncbnFI9YcOF77Bqw-yCIKjgiFjb6HyEx3d?9WgEkjRo2cHnWyYaEUDkrY!1v53cJ2FODIAR1ThA-W85fslk/SQMFMZg=m4EIKCrtlPilZmpSIXq-kKGZSg6qp1UpNthZIxBDA2q2ZEWVCcvKRlIsnAsgcy22wsVu01A3GJ1KBWVgEkZDzP3Sv0wHLAZDMKZHENlbLEaBHofrdOdTgDJP/dZY5-Fyi8kvG=VgfK-MPDcfuJfY3ydmQ2yiHIVS1dXCijZ0lKg/RE4ny7CiTA-mkPl4rQFUt/W8EQu3EGBccyuu3f2iCS//8BodFsU0Aq7u7haCHNlXn1-iKoA4TK?k1?G2Z/yGYfO2ri7JLFWlt2DrtfAi6fSPBLIRgx3rhUWgSP!YypgB65vBln64vjGQun9zbIEE8i3psEoXJ39YM!1KFGk0OTh7?NKjUKsxsVh6GVYmHDMGnYFMiefX4r0frYDJQqkIAo6??U3eNgjGhgg1blrLRjoPGD9iJiSToIRZlexKJZoU8ry-d4lSAIa?CS6wSW23IAIWh0jK?uhbbbj?LxDqw?gx-3Gx?Cjsd-5/Zu-Lbb8rUTqzxXWFU!Pg7PPR-x/jLNIV1!CgT5sG-mYPCXwoEzF?RA7xqwN0He5a?x=3ZVrdZ7jxBoj8-rY5fUHKYyYXC7qJ2VUtgEmWOb3cL8NGHh3s92OjqFGfPrfDHrz2aRsueua5!K=jsQZjLsdRfiDzJoj9I?SNPwNiXn-gk=qTfC9eyFVyPn/mYGQ5-pT54hQbkvl6YjP7W4-FtLa8JgEthw8UugZRguWF1EKe?aul!Pw4dfp62cjWJo9K-XmXf=gOqprRnKwhExQJSkuI2=Gai7JI?aEgQJnyu?LhTFuVCBL8RfjZSbvkFhx4eAMknQke5LmKn8dvYs0B5J/y!wkCyc!?mTuR6uqdvffyX!Zx2pGJozuPeJGt9jlsUNv-h2?";
                    byte[] data;
                    message += (";" + Convert.ToString(login_signin.login.accounts_user["token"]));
                    message += (";" + "InvataUnit");
                    message += (";" + navbar_home.token_page);
                    data = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(data, 0, data.Length);

                    while (navbar_home.page == "InvataUnit_vizualizare")
                    {
                        byte[] buffer = new byte[999999];
                        int byteRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        string Message = Encoding.ASCII.GetString(buffer, 0, byteRead);
                        
                        if (byteRead > 0)
                        {
                            if (Message.Trim() == "-1")
                            {
                                var frm = new notification.error();
                                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                                panel.Controls.Add(frm);
                                notification.error.message = "Ceva nu a mers bine, mai incearc!";
                                frm.BringToFront();
                                client.Close();
                                stream.Close();
                                stream.Flush();

                                await Task.Delay(1000);
                                break;
                            }
                            else if (Message.Trim() == "1")
                            {
                                while (navbar_home.page == "InvataUnit_vizualizare")
                                {
                                    byte[] buffer2 = new byte[999999];
                                    int byteRead2 = await stream.ReadAsync(buffer2, 0, buffer2.Length);
                                    string Message2 = Encoding.ASCII.GetString(buffer2, 0, byteRead2);
                                    
                                    if(byteRead2 > 0)
                                        load_design_answers(Message2);
                                }
                            }
                        }
                    }                    
                }
                catch(Exception e)
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Conexiunea oprita!";
                    frm.BringToFront();
                    await Task.Delay(200);
                    navbar_home.page = "InvataUnit";
                    navbar_home.use = false;
                }
            }
            schoolsync.hide_loading();
        }

        private void adauga_fisier_Click(object sender, EventArgs e)
        {
            try
            {
                if (flowLayoutPanel2.Controls.Count < 3)
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
                            guna2Chip.Tag = opf.FileName;
                            flowLayoutPanel2.Controls.Add(guna2Chip);
                        }
                    }
                }
                else
                {
                    var frm = new notification.error();
                    schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                    var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                    panel.Controls.Add(frm);
                    notification.error.message = "Poti adauga maxim 3 fisiere!";
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

        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            multiple_class _class = new multiple_class();
            if (guna2TextBox1.Text.Trim() == "")
            {
                var frm = new notification.error();
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                panel.Controls.Add(frm);

                notification.error.message = "Completati cu raspunsul!";
                frm.BringToFront();
            }
            else
            {
                NetworkStream stream = client.GetStream();

                string files = "";
                foreach (Control control in flowLayoutPanel2.Controls)
                {
                    FileInfo inf = new FileInfo(control.Tag.ToString());

                    string token_file = _class.generate_token_250();

                    var param = new Dictionary<string, string>();
                    param.Add("token", schoolsync.token);
                    param.Add("token_user", Convert.ToString(login_signin.login.accounts_user["token"]));
                    param.Add("token_file", token_file);
                    param.Add("filename", inf.Name);

                    _ = await _class.new_UploadFileAsync(param, control.Tag.ToString());
                    files += (token_file + ";");
                }

                var param2 = new Dictionary<string, string>()
                {
                    {"answer", guna2TextBox1.Text},
                    {"files", files}
                };

                string message = JsonConvert.SerializeObject(param2);
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
                guna2TextBox1.Clear();
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;

            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command","select * from invataunit where token = ?");

            var param = new Dictionary<string, string>()
            {
                {"token", navbar_home.token_page}
            };
            data.Add("params", JsonConvert.SerializeObject(param));

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

            if (task["message"] == "success")
            {
                string str = task["0"]["favourites"];
                if (ok == false)
                {
                    str += (login_signin.login.accounts_user["token"] + ";");
                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "update invataunit set favourites = ? where token = ?");
                    param = new Dictionary<string, string>()
                    {
                        {"favourites", str},
                        {"token", navbar_home.token_page}
                    };
                    data.Add("params", JsonConvert.SerializeObject(param));

                    task = await _Class.PostRequestAsync(url, data);
                    if (task["message"] == "update success")
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
                    for (int i = 0; i < split.Length - 1; i++)
                    {
                        if (split[i] != Convert.ToString(login_signin.login.accounts_user["token"]))
                        {
                            fns += (split[i] + ";");
                        }
                    }
                    url = "https://schoolsync.nnmadalin.me/api/put.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "update invataunit set favourites = ? where token = ?");
                    param = new Dictionary<string, string>()
                    {
                        {"favourites", fns},
                        {"token", navbar_home.token_page}
                    };
                    data.Add("params", JsonConvert.SerializeObject(param));
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
    }
}
