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
            if(progressBar1.Value == 10)
            {
                this.Hide();
                new intfix_enterprise().Show();
            }
            timer1.Start();
        }
        public int timeleft;

        private void startup_Load(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.date < DateTime.Now.Date)
            {
                Properties.Settings.Default.license = "activated";
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.license = "unactivated";
                Properties.Settings.Default.Save();
            }
            progressBar1.Value += 10;
        }
        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            if (progressBar1.Value == 10)
            {
                this.Hide();
                new intfix_enterprise().Show();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value == 10)
            {
                timer1.Stop();
                this.Hide();
                new intfix_enterprise().Show();
            }
        }
    }
}
