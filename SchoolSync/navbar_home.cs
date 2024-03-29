﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace SchoolSync
{
    public partial class navbar_home : UserControl
    {
        public navbar_home()
        {
            InitializeComponent();
        }

        private void home_Load(object sender, EventArgs e)
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
            var label = panel.Controls["label1"];
            label.Text = "SchoolSync | Acasa";
            GC.Collect();

            use = true;
            guna2Panel2.Controls.Clear();

            var frm = new pages.Home();
            guna2Panel2.Controls.Add(frm);

            background_color_btn();
            guna2Button1.FillColor = Color.FromArgb(66, 66, 66);
        }

        void background_color_btn()
        {
            guna2Button1.FillColor = Color.Transparent;
            guna2Button2.FillColor = Color.Transparent;
            guna2Button3.FillColor = Color.Transparent;
            guna2Button4.FillColor = Color.Transparent;
            guna2Button5.FillColor = Color.Transparent;
            guna2Button6.FillColor = Color.Transparent;
            guna2Button7.FillColor = Color.Transparent;
            guna2Button8.FillColor = Color.Transparent;
        }        

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
            navbar_home.use = false;
            navbar_home.page = "Home";
        }       

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            navbar_home.use = false;
            navbar_home.page = "EduMentor";

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            navbar_home.use = false;
            navbar_home.page = "InvataUnit";
        }


        private void guna2Button5_Click(object sender, EventArgs e)
        {
            navbar_home.token_page = login_signin.login.accounts_user["token"];
            navbar_home.use = false;
            navbar_home.page = "Profil";
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
            Properties.Settings.Default.Data_account = "";
            Properties.Settings.Default.Save();

            page = "";
            timer1.Dispose();

            var frm = new login_signin.login();
            panel.Controls.Clear();
            panel.Controls.Add(frm);
        }

        public static bool use = true;
        public static string page = "", token_page = "", token_page_2 = "";

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            navbar_home.use = false;
            navbar_home.page = "FlowTalk";
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            navbar_home.use = false;
            navbar_home.page = "TimePlan_calendar";
        }

        private async void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Form>().ToList().Count == 1)
            {
                var frm = new openai_chat();
                frm.Show();
            }
            else
            {
                Application.OpenForms.OfType<Form>().ToList()[1].WindowState = FormWindowState.Normal;
            }
        }

        public static bool IsFormOpen(string formName)
        {
            return Application.OpenForms.OfType<Form>().Any(form => form.Name == formName);
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            navbar_home.use = false;
            navbar_home.page = "EduClass";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (page == "Home" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | Acasa";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.Home();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();

                background_color_btn();
                guna2Button1.FillColor = Color.FromArgb(66, 66, 66);
            }

            else if (page == "EduMentor" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduMentor";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.EduMentor();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();

                background_color_btn();
                guna2Button2.FillColor = Color.FromArgb(66, 66, 66);
            }
            else if (page == "EduMentor_adauga" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduMentor - Adauga Material";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.EduMentor_pages.EduMentor_Adauga();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
                background_color_btn();
                guna2Button2.FillColor = Color.FromArgb(66, 66, 66);
            }
            else if (page == "EduMentor_cod" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduMentor - Vizualizare cod";
                GC.Collect();

                use = true;

                var frm = new pages.EduMentor_pages.EduMentor_Cod();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "EduMentor_vizualizare" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduMentor - Vizualizare Material";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.EduMentor_pages.EduMentor_Vizualizare();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "EduMentor_vizualizare_->_home" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduMentor - Vizualizare Material";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();
                var frm = new pages.EduMentor_pages.EduMentor_Vizualizare();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "EduMentor_editare" && use == false)
            {

                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduMentor - Editare Material";
                GC.Collect();

                use = true;

                guna2Panel2.Controls.Clear();

                var frm = new pages.EduMentor_pages.EduMentor_Adauga();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "EduMentor_editare_->_home" && use == false)
            {

                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduMentor - Editare Material";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();
                var frm = new pages.EduMentor_pages.EduMentor_Adauga();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }

            else if (page == "InvataUnit" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | InvataUnit";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.InvataUnit();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
                background_color_btn();
                guna2Button3.FillColor = Color.FromArgb(66, 66, 66);
            }
            else if (page == "InvataUnit_adauga" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | InvataUnit - Adauga Intrebare";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.InvataUnit_pages.InvataUnit_Adauga();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "InvataUnit_editare" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | InvataUnit - Editare Intrebare";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();
                var frm = new pages.InvataUnit_pages.InvataUnit_Adauga();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "InvataUnit_vizualizare" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | InvataUnit - Vizualizare intrebare";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.InvataUnit_pages.InvataUnit_Vizualizare();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }

            else if (page == "FlowTalk" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | FlowTalk";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.FlowTalk_pages.FlowTalk();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
                background_color_btn();
                guna2Button4.FillColor = Color.FromArgb(66, 66, 66);
            }
            else if (page == "FlowTalk_adauga" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | FlowTalk";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.FlowTalk_pages.FlowTalk_Adauga();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "FlowTalk_home" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | FlowTalk";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.FlowTalk_pages.FlowTalk();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
                background_color_btn();
                guna2Button4.FillColor = Color.FromArgb(66, 66, 66);
            }
            else if (page == "FlowTalk_editare" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | FlowTalk";
                GC.Collect();

                use = true;

                var frm = new pages.FlowTalk_pages.FlowTalk_Adauga();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }

            else if (page == "TimePlan_calendar" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | TimePlan";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.TimePlan_pages.TimePlan_Calendar();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
                background_color_btn();
                guna2Button7.FillColor = Color.FromArgb(66, 66, 66);
            }

            else if (page == "EduClass" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduClass";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.EduClass();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
                background_color_btn();
                guna2Button8.FillColor = Color.FromArgb(66, 66, 66);
            }
            else if (page == "EduClass_creaza" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduClass";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.EduClass_pages.EduClass_Adauga();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "EduClass_vizualizare" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduClass";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.EduClass_pages.EduClass_Vizualizare();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "EduClass_adauga_curs" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduClass";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.EduClass_pages.EduClass_Adauga_Curs();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "EduClass_vizualizare_persoane" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduClass";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.EduClass_pages.EduClass_Vizualizare_Persoane();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "EduClass_editare" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduClass";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.EduClass_pages.EduClass_Adauga();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "EduClass_creaza_lectie" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduClass";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.EduClass_pages.EduClass_Adauga_Lectie();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "EduClass_editare_lectie" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduClass";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.EduClass_pages.EduClass_Adauga_Lectie();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "EduClass_vizualizare_lectie" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduClass";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.EduClass_pages.EduClass_Vizualizare_lectie();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }
            else if (page == "EduClass_vizualizare_teme" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | EduClass";
                GC.Collect();

                use = true;

                var frm = new pages.EduClass_pages.EduClass_Vizualizare_Teme();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }

            else if (page == "Profil" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | Profil";
                GC.Collect();

                use = true;
                guna2Panel2.Controls.Clear();

                var frm = new pages.Profil();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
                background_color_btn();
                guna2Button5.FillColor = Color.FromArgb(66, 66, 66);
            }
            else if (page == "Profil_person" && use == false)
            {
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var panel = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel1"];
                var label = panel.Controls["label1"];
                label.Text = "SchoolSync | Profil";
                GC.Collect();

                use = true;

                var frm = new pages.Profil();
                guna2Panel2.Controls.Add(frm);
                frm.BringToFront();
            }

        }
    }
}
