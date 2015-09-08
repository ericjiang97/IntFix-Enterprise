/*
* IntFix Enterprise 
* Splash Screen Code.
* Version 0.1
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace IntFix_Enterprise
{
    public partial class startup : Form
    {
        #region public_strings
        public string ping_status;
        public string license;
        #endregion

        public startup()
        {
            InitializeComponent();
            label2.Text = "Version " + this.ProductVersion;
        }
        public int timeleft;

        private void startup_Load(object sender, EventArgs e)
        {
            timeleft = 20;
            this.timer1.Start();
        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timeleft > 0)
            {
                timeleft = timeleft - 1;
                progressBar1.Value = (20 - timeleft)*5;
            }
            else
            {
                timer1.Stop();
                new intfix_enterprise().Show();
                this.Hide();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if(timeleft == 10)
            {
                label3.Text = "Loading Workers";
            }
        }
    }
}
