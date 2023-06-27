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

namespace SchoolSync.pages.TimePlan_pages
{
    public partial class TimePlan_Calendar : UserControl
    {
        public TimePlan_Calendar()
        {
            InitializeComponent();
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        DateTime now;

        async void load_tab(DateTime time)
        {
            label1.Text = time.Day.ToString();
            if (time.DayOfWeek.ToString() == "Monday")
                label2.Text = "Luni";
            else if (time.DayOfWeek.ToString() == "Tuesday")
                label2.Text = "Marti";
            else if (time.DayOfWeek.ToString() == "Wednesday")
                label2.Text = "Miercuri";
            else if (time.DayOfWeek.ToString() == "Thursday")
                label2.Text = "Joi";
            else if (time.DayOfWeek.ToString() == "Friday")
                label2.Text = "Vineri";
            else if (time.DayOfWeek.ToString() == "Saturday")
                label2.Text = "Sambata";
            else if (time.DayOfWeek.ToString() == "Sunday")
                label2.Text = "Duminica";
            else
                label2.Text = time.DayOfWeek.ToString();
            label23.Text = time.Year.ToString();

            int luna = time.Month;
            int zi = time.Day;

            DateTime dotm = new DateTime(time.Year, time.Month, 1);
            
            int dayofweek = Convert.ToInt32(dotm.DayOfWeek.ToString("d"));
            int daymonth = DateTime.DaysInMonth(time.Year, time.Month);

            if (dayofweek == 0)
                dayofweek = 7;

            luna += 3;
            
            foreach (Control ctrl in guna2Panel1.Controls)
            {
                if (ctrl is Guna.UI2.WinForms.Guna2CircleButton)
                {
                    ((Guna.UI2.WinForms.Guna2CircleButton)ctrl).FillColor = Color.Gainsboro;
                    ((Guna.UI2.WinForms.Guna2CircleButton)ctrl).ForeColor = Color.Black;
                    ((Guna.UI2.WinForms.Guna2CircleButton)ctrl).BorderThickness = 0;
                }
            }

            foreach (Control ctrl in guna2Panel1.Controls)
            {
                if (ctrl is Label)
                {
                    ctrl.ForeColor = Color.FromArgb(32, 33, 36);
                }
            }

            foreach (Control ctrl in guna2Panel1.Controls)
            {
                if (ctrl is Label && ctrl.Name.ToString() == "label" + luna.ToString())
                {
                    ctrl.ForeColor = Color.FromArgb(46, 204, 113);
                }
            }
            foreach (Control ctrl in guna2Panel1.Controls)
            {
                if (ctrl is Guna.UI2.WinForms.Guna2CircleButton)
                {
                    ctrl.Visible = false;
                }
            }

            for(int i = 1; i <= daymonth; i++)
            {
                foreach (Control ctrl in guna2Panel1.Controls)
                {
                    if (ctrl is Guna.UI2.WinForms.Guna2CircleButton && ctrl.Name.ToString() == "guna2CircleButton" + (i + dayofweek).ToString())
                    {
                        ctrl.Visible = true;
                        ctrl.Text = i.ToString();
                    }
                }
            }
            
            foreach (Control ctrl in guna2Panel1.Controls)
            {
                if (ctrl is Guna.UI2.WinForms.Guna2CircleButton && ctrl.Name.ToString() == "guna2CircleButton" + (zi + dayofweek).ToString())
                {
                    ((Guna.UI2.WinForms.Guna2CircleButton)ctrl).FillColor = Color.FromArgb(46, 204, 113);
                    ctrl.ForeColor = Color.White;
                }
            }

            listBox1.Items.Clear();

            multiple_class _class = new multiple_class();
            string url = "https://schoolsync.nnmadalin.me/api/get.php";
            var data = new Dictionary<string, string>();
            data.Add("token", schoolsync.token);
            data.Add("command", "select * from timeplan where token_user = ?");

            var param = new Dictionary<string, string>()
            {
                {"token", Convert.ToString(login_signin.login.accounts_user["token"])}
            };

            data.Add("params", JsonConvert.SerializeObject(param));

            dynamic task = await _class.PostRequestAsync(url, data);
            if (task["message"] == "success")
            {
                dynamic calendar = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["calendar"]));
                JObject json = calendar;

                try
                {
                    JArray array = (JArray)json.SelectToken(time.ToString("MM/dd/yyyy"));
                    if (array != null && array.Type == JTokenType.Array)
                    {
                        foreach (var item in array)
                        {
                            listBox1.Items.Add(item.ToString());
                        }
                    }
                }
                catch
                {
                    try
                    {
                        listBox1.Items.Add(json[time.ToString("MM/dd/yyyy")]);
                    }
                    catch { };
                }
            }
        }

        private void TimePlan_Calendar_Load(object sender, EventArgs e)
        {
            load_tab(DateTime.Now);
            now = DateTime.Now;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in guna2Panel1.Controls)
            {
                if (ctrl is Label)
                {
                    ctrl.ForeColor = Color.FromArgb(32, 33, 36);
                }
            }

            ((Label)sender).ForeColor = Color.FromArgb(46, 204, 113);
            try
            {
                now = new DateTime(now.Year, Convert.ToInt32(((Label)sender).Tag.ToString()), now.Day);
            }
            catch
            {
                now = new DateTime(now.Year, Convert.ToInt32(((Label)sender).Tag.ToString()), DateTime.DaysInMonth(now.Year, Convert.ToInt32(((Label)sender).Tag.ToString())));
            }
            load_tab(now);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            now = now.AddYears(1);
            load_tab(now);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            now = now.AddYears(-1);
            load_tab(now);
        }

        private void guna2CircleButton43_Click(object sender, EventArgs e)
        {
            now = new DateTime(now.Year, now.Month, Convert.ToInt32(((Guna.UI2.WinForms.Guna2CircleButton)sender).Text));
            load_tab(now);
        }

        private async void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            if(guna2TextBox1.Text.Trim() != "")
            {
                string eveniment = guna2TextBox1.Text.Trim();

                multiple_class _class = new multiple_class();
                string url = "https://schoolsync.nnmadalin.me/api/get.php";
                var data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "select * from timeplan where token_user = ?");

                var param = new Dictionary<string, string>()
                {
                    {"token", Convert.ToString(login_signin.login.accounts_user["token"])}
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                dynamic task = await _class.PostRequestAsync(url, data);

                dynamic calendar = null;

                if(task["message"] == "success")
                {
                    calendar = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["calendar"]));
                }
                else
                {
                    url = "https://schoolsync.nnmadalin.me/api/post.php";
                    data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "insert into timeplan (token_user, calendar, timetable) values (?, ?, ?)");

                    param = new Dictionary<string, string>()
                    {
                        {"token_user", Convert.ToString(login_signin.login.accounts_user["token"])},
                        {"calendar", "{}"},
                        {"timetable", "{}"}
                    };

                    data.Add("params", JsonConvert.SerializeObject(param));

                    task = await _class.PostRequestAsync(url, data);
                }

                JObject json = calendar;

                bool keyExists = false;
                foreach (JProperty property in json.Properties())
                {
                    if (property.Name == now.ToString("MM/dd/yyyy"))
                    {
                        keyExists = true;
                        if (property.Value.Type == JTokenType.Array)
                        {
                            JArray existingArray = (JArray)property.Value;
                            existingArray.Add(eveniment);
                        }
                        else
                        {
                            JArray newArray = new JArray(property.Value, eveniment);
                            json[now.ToString("MM/dd/yyyy")] = newArray;
                        }
                        break;
                    }
                }

                if (!keyExists)
                {
                    json.Add(now.ToString("MM/dd/yyyy"), eveniment);
                }



                url = "https://schoolsync.nnmadalin.me/api/put.php";
                data = new Dictionary<string, string>();
                data.Add("token", schoolsync.token);
                data.Add("command", "update timeplan set calendar = ? where token_user = ?");

                param = new Dictionary<string, string>()
                {
                    {"calendar", JsonConvert.SerializeObject(json)},
                    {"token_user", Convert.ToString(login_signin.login.accounts_user["token"])}
                };

                data.Add("params", JsonConvert.SerializeObject(param));

                task = await _class.PostRequestAsync(url, data);
                guna2TextBox1.Clear();
                load_tab(now);
            }
        }

        private async void listBox1_DoubleClick(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;

            if (index >= 0)
            {

                guna2MessageDialog1.Caption = "Sterge eveniment";
                guna2MessageDialog1.Text = "Esti sigur ca vrei sa stergi evenimentul?";

                DialogResult dr = guna2MessageDialog1.Show();

                JObject json = new JObject();

                if (dr == DialogResult.Yes)
                {
                    multiple_class _class = new multiple_class();
                    string url = "https://schoolsync.nnmadalin.me/api/get.php";
                    var data = new Dictionary<string, string>();
                    data.Add("token", schoolsync.token);
                    data.Add("command", "select * from timeplan where token_user = ?");

                    var param = new Dictionary<string, string>()
                    {
                        {"token", Convert.ToString(login_signin.login.accounts_user["token"])}
                    };

                    data.Add("params", JsonConvert.SerializeObject(param));

                    dynamic task = await _class.PostRequestAsync(url, data);

                    if (task["message"] == "success")
                    {
                        dynamic calendar = JsonConvert.DeserializeObject(Convert.ToString(task["0"]["calendar"]));

                        json = calendar;

                        try
                        {
                            JArray array = (JArray)json[now.ToString("MM/dd/yyyy")];
                           
                            array.RemoveAt(index);
                        }
                        catch 
                        {
                            try
                            {
                                json.Remove(now.ToString("MM/dd/yyyy"));
                            }
                            catch { };
                        };

                        url = "https://schoolsync.nnmadalin.me/api/put.php";
                        data = new Dictionary<string, string>();
                        data.Add("token", schoolsync.token);
                        data.Add("command", "update timeplan set calendar = ? where token_user = ?");

                        param = new Dictionary<string, string>()
                        {
                            {"calendar", JsonConvert.SerializeObject(json)},
                            {"token_user", Convert.ToString(login_signin.login.accounts_user["token"])}
                        };

                        data.Add("params", JsonConvert.SerializeObject(param));

                        task = await _class.PostRequestAsync(url, data);

                        load_tab(now);
                    }
                }
            }
        }
    }
}
