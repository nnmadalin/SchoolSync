using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace SchoolSync
{
    public partial class schoolsync : Form
    {
        public schoolsync()
        {
            InitializeComponent();
        }


        public static string token = "rKSW7LpXOM8n4ZEQc9FnQTLbpgtQ9qUY4HP3mQkGZJFHNEamt9yI?6Dy?5zjlvp0vSwAfq3E4BxAHB!SmnAggocCw181vqHnNBu1a8d9drAqzmcnGKYvI4?MIzPY8XJjGOywc=489KFcwgfMl2cWSQ5uboyRjrkrmryeq0DFcFByp0rKObROT1X!i-Nkb7Dgf/fOTiXEeJAWohAQVpi6IiqkG?NtkIc/wWjLv0MLYJ9c6kf7GWXbXjWNULdfoh/s8XCzI6HlT0fIo3G=q1kA3QuGt1QLIfMZxdAOebGji745HEee/rPes6J0?!TvvG-b4RKD11s?dbos4E7SQCOuNo1Q=!DemHroUhfAs7tV128SQIZY1bc3XYvjkKz8B6dk9v67vXJ3X7dylwp0jrchwSQH!Hs?Czg!DLYJnm7LkrcszDOUPNDIQ2u7UEA-Su7RvlgvGlnE79PulDoBq!JAda-YFVOnEABgY6sVWr29rG-OJsLfQ20mby0qaxCRPRj?sy!EqUMyBzmD86ILOow=LLfzc?4?E=YVd?dZCV9pQvOvZX!=DfY1JVhxtbQ37t8fsA5a6VKIov2RWGTKWu3COe07jUcN9v-FBbbSPT-6rCkl3IR7wgAJ3pTfI0/lAewpPe7PV/CXfqS0FbBVZmD/fQ7?oljo-tgpdCLfM/FOn!WgnyVrIybqt/JSl0psmrvN48HDmRoCpRtKH1xJlPzg21p-j5ehvGgJpBsgJC=Tw5ImKqHqgCSBvaqknSzmyk?akxt9NBXEHRAfhQjZFT/BEpFAvi5!GI7!G2E93dQ6UV0ueROYYx//sYUKkPPLs-EnG4y/!Bpk-6=Tz1LmOReUL8-MmOOC224gHNdr2bBNJt0/NXOtxFsRdhuDw!wzUzCY!jdxdqEkI8KwPbuZzHpe2Xtr6/e!8aCq?M9Wx/hzi-RmOAoDLmpBnPCXErm/vN4!DXgFGpCD98=IWWMdHk7xPSVkWHkvaTD6ao0DZcB5=An!M1p/F9a8N9/rUweb6yAc!d2p0h=?1LgxgAsZS/c8oD3fGPg8=AVDWR?tFta-Mh0jVff9PYSQ3ZBAJdHzs8nKCKaNtHZTZmVV6APXkOMJRFgl7?wiL3bjtbGGa8pJuMJGpTOQxt7p-8ZFM=j/y48zcdBY9PIGpz166oZFt=APcIcO-QnFm2EtbtYdHIlBUKPd?Ug2DocLiBJ5qL3ehlH87QbAe=zYrAwwaNAO-Zzn99!Z=Aw19EPT!JRwHCD6Gw4o7h7FsV2NotpVHrbcud6UQByQ8MfTh3nvRCwrBOt-aJE?zD7ivs6nK-9c=8Y3zC8TR9j3fR2!Ay?D-EaZQMRvEYtG5P8ZBVRWGfuvc5/ptK1AC3wipU/=mnK=b6vH8X3Gq5n0y6vcxn2s-IPL7YR3V-Q?9A7iyW7bYlp!cmkH4vZi=8JSXpk2Q9XFoOfXj1oc5jIiMYfKQ12lV=fTWRHGzKFqRcadzg-QUrXjcLiQky//?GFagZ5/ih26EiTtrWF!grcbv79XoG7UVN6ZEmbfdPXxwSKyFs80M10yWBsBkI2phsR72HwCw7RecLNIvK7lI-=l9IJGv1=TMOZpse9qTGO?XSE!tuGa3jOA1xR=uR5bm8KuHG1USnt?yTOjV4NMpz-NBr/mFHIhHfBW/3p198i=9PD5IzCevXpHyOVeDZPlYbo7lIzleVNX3tufY9WeUV-JC/GHAfSVgtpPgfHYVKFD9bJmkbQsj10B2l6pk!5Ta5cXNBhsSuVFllyzn86qPrf9sR66lAPHC1uBEt/DiuJ0yV01cKqFCWgSVK-8v6FzTAGiRY2cUgv=yfRLIt0P!crunlsHMvS3vAx11OATDkyuLIEyGt7fxe7h!mWAvd0jsQxCPRSt/?wLl9/W3-LXuca3yc7nPojTYe?W?TL!QHfFv42ADQ9S1hud1i1BPDaVCYqUOrxkaSsw=OG75W3dNYdwdBHMZMsGBVwc=4DeItW0P8Ve6TYo1y!I04r6eVlRlptt3!EXcYqOoWKPrl0xznLEehnh7Kiho-64qTwe";
        public static bool is_loading = false;
        public static string version = "v8.9-18/7/2023";

        private async void Form1_Load(object sender, EventArgs e)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            var frm = new login_signin.login();
            guna2Panel2.Controls.Add(frm);
            frm.Show();
            frm.Location = new Point(0, 0);
            GC.Collect();
                
        }

        public static void show_loading()
        {
            if (is_loading == false)
            {
                is_loading = true;
                schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
                var pnl = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
                pnl.Enabled = false;

                Guna.UI2.WinForms.Guna2WinProgressIndicator loading = new Guna.UI2.WinForms.Guna2WinProgressIndicator()
                {
                    Size = new Size(90, 90),
                    BackColor = Color.Transparent,
                    UseTransparentBackground = true,
                    AutoStart = true,
                    Location = new Point((1340 - 90) / 2, (690 - 90) / 2),
                    Name = "panel_loading"
                };
                pnl.Controls.Add(loading);
                loading.Show();
                loading.BringToFront();
            }
        }

        public static void hide_loading()
        {
            schoolsync schoolsync = (schoolsync)System.Windows.Forms.Application.OpenForms["schoolsync"];
            var pnl = (Guna.UI2.WinForms.Guna2Panel)schoolsync.Controls["guna2Panel2"];
            pnl.Controls.Remove(pnl.Controls["panel_loading"]);
            pnl.Enabled = true;
            is_loading = false;
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void schoolsync_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
