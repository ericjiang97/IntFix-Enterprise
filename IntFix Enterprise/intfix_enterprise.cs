/*
IntFix Enterprise Version 1.2.11-alpha1
Based off Core Code of IntFix v3.4.1.0
by Eric JIANG
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
using System.IO;
using System.Diagnostics;
using System.Management;
using System.Threading;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.ServiceProcess;

namespace IntFix_Enterprise
{
    public partial class intfix_enterprise : Form
    {
        public string showme { get; set; }
        public intfix_enterprise()
        {
            InitializeComponent();
            string license = appset.Default.Licensed;
            label2.Text = "Licensed to: " + license;

        }

        private void mainfixbtn_Click(object sender, EventArgs e)
        {
            

            textBox1.Text += "\r\n";
            textBox1.Text += "========================================================= \r\n MainFix Cycle Version 1.1 \r\n IntFix Enterprise \r\n ========================================================= \r\n";
            textBox1.Text += "00% Completed:Updating Group Policy \r\n";
            progressBar1.Value = 10;
            
            try
            {
                FileInfo execFile = new FileInfo("gpupdate.exe");
                Process proc = new Process();
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc.StartInfo.FileName = execFile.Name;
                proc.StartInfo.Arguments = "/force";
                proc.Start();
                proc.WaitForExit();//Wait for GPUpdate to finish
                Application.DoEvents();
                Thread.Sleep(100);
            }
            catch (Exception exception)
            {
                //Exception
                MessageBox.Show("An Error has Occured. Error:" + exception);
            }

            textBox1.Text += "10% Completed: Running IPFix \r\n";
            progressBar1.Value = 30;
            #region ipAdress Fix
            #region ipconfig /release
            FileInfo execFile2 = new FileInfo("ipconfig.exe");
            Process proc2 = new Process();
            proc2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc2.StartInfo.FileName = execFile2.Name;
            proc2.StartInfo.Arguments = "/release";
            proc2.Start(); //Start Process (cmd line): ipconfig /release
            proc2.WaitForExit();
            progressBar1.Value = 35;
            textBox1.Text += "35% Completed: Releasing IP Address \r\n";
            #endregion
            #region ipconfig /renew
            FileInfo execFile3 = new FileInfo("ipconfig.exe");
            progressBar1.Value = 40;
            Process proc3 = new Process();
            proc3.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc3.StartInfo.FileName = execFile3.Name;
            proc3.StartInfo.Arguments = "/renew";
            proc3.Start();
            proc3.WaitForExit();
            progressBar1.Value = 50;
            textBox1.Text += "50% Completed: Renewing IP Address \r\n";
            #endregion
            #endregion
            textBox1.Text += "50% Completed: Running DNSFix \r\n";
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();

            #region FlushDNS
            FileInfo execFile4 = new FileInfo("ipconfig.exe");
            Process proc4 = new Process();
            proc4.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc4.StartInfo.FileName = execFile4.Name;
            proc4.StartInfo.Arguments = "/flushdns";
            proc4.Start();
            proc4.WaitForExit();
            #endregion
            progressBar1.Value = 75;
            textBox1.Text += "75% Completed: Flushed DNS Cache \r\n";
            textBox1.Text += "75% Completed: Running NetshFix \r\n";
            #region Rst_network_stats
            FileInfo execFile5 = new FileInfo("netsh.exe");
            Process proc5 = new Process();
            proc5.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc5.StartInfo.FileName = execFile5.Name;
            proc5.StartInfo.Arguments = "int ip reset c:resetlog.txt";
            proc5.Start();
            proc5.WaitForExit();
            progressBar1.Value = 80;
            textBox1.Text += "80% Completed: Reset \r\n";
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();

            FileInfo execFile6 = new FileInfo("netsh.exe");
            Process proc6 = new Process();
            proc6.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc6.StartInfo.FileName = execFile6.Name;
            proc6.StartInfo.Arguments = "winsock reset";
            proc6.Start();
            proc6.WaitForExit();
            progressBar1.Value = 100;
            textBox1.Text += "100% Completed MainFix Cycle \r\n";
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
            #endregion
        }

        private void GPO_Update()
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string name = saveFileDialog1.FileName;
                File.WriteAllText(name, textBox1.Text);
            }
        }

        private void intfix_enterprise_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Text = Application.ProductName + " | Version: " + Application.ProductVersion;
            this.progressBar1.BackColor = Color.White;
            #region startup_setup
            string date = DateTime.Now.ToString("MM\\-dd\\-yyyy");
            saveFileDialog1.FileName = "log-" + date +"-" + Environment.MachineName;
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
            PowerStatus p = SystemInformation.PowerStatus;
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            if (IsAdministrator() == true)
            {
                this.Text += " (Admin)";
                button3.Enabled = false;
                runAsAdminToolStripMenuItem.Enabled = false;
            }
                #endregion

                #region computerinfo
                string machinename = Environment.MachineName;
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            textBox1.Text += "Computer Name: " + machinename + "\r\n";
            textBox1.Text += "Domain Name: " + domainName +"\r\n";
            textBox1.Text += "Current User: " + userName + "\r\n";
            textBox1.Text += "\r\n";
            textBox1.Text += "Battery Charge: " + string.Format("{0}%", (p.BatteryLifePercent * 100));
            #endregion

        }

        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            textBox1.Text += "Exiting IntFix Enterprise";
            Application.Exit(); //Exits program, if form is closed.
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (IsAdministrator() == false)
            {
                DialogResult error = MessageBox.Show("Error #00002. This command requires admin previllages. Please visit https://www.ejiang.co/support/knowledgebase.php?article=2 for more information. Do you want to restart as admin?", "IntFix Enterprise - Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (error == DialogResult.Yes)
                {
                    // Restart program and run as admin
                    var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                    ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                    startInfo.Verb = "runas";
                    System.Diagnostics.Process.Start(startInfo);
                    this.Close();
                    return;
                }
                else
                {
                    //ignore
                }
            }
            else
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Error. No KMS Server Address Enterred. Please type one in,", "IntFix - KMSFix", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    textBox1.Text += "\r\n Running KMS Windows Activation Method \r\n";
                    string kmsserver = textBox2.Text;
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C slmgr.vbs /skms " + kmsserver;
                    process.StartInfo = startInfo;
                    process.Start();
                    textBox1.Text += "Finished Setting KMS Server. Running Activation Services \r\n";
                    System.Diagnostics.Process process2 = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo2 = new System.Diagnostics.ProcessStartInfo();
                    startInfo2.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo2.FileName = "cmd.exe";
                    startInfo2.Arguments = "/C slmgr.vbs /ato";
                    process2.StartInfo = startInfo2;
                    process2.Start();
                }
            }
            
        }

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (IsAdministrator() == false)
            {
                // Restart program and run as admin
                var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                startInfo.Verb = "runas";
                System.Diagnostics.Process.Start(startInfo);
                this.Close();
                return;
            }

    }

        private void button4_Click(object sender, EventArgs e)
        {
            new AboutBox1().Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ServiceController service = new ServiceController("spooler");
            if (IsAdministrator() == true)
            {
                try
                {
                    textBox1.Text += "\r\nStopiing Print Spooler \r\n";
                    progressBar1.Value = 33;
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                    textBox1.Text += "Stopped Print Spooler. Starting Print Spooler \r\n";
                    progressBar1.Value = 67;
                    service.Start();
                    progressBar1.Value = 100;
                    textBox1.Text += "Finished! \r\n";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                }
            }
            else
            {
                DialogResult error = MessageBox.Show("Error #00002. This command requires admin previllages. Please visit https://www.ejiang.co/support/knowledgebase.php?article=2 for more information. Do you want to restart as admin?", "IntFix Enterprise - Error", MessageBoxButtons.YesNo,MessageBoxIcon.Error);
                if (error == DialogResult.Yes)
                {
                    // Restart program and run as admin
                    var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                    ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                    startInfo.Verb = "runas";
                    System.Diagnostics.Process.Start(startInfo);
                    this.Close();
                    return;
                }
                else
                {
                    //ignore
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Open Button
            this.WindowState = FormWindowState.Normal; //Open Form
            this.ShowInTaskbar = true; //Show icon in taskbar
        }
        protected override void OnResize(EventArgs e)
        {
            if(WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false; //Hide icon in taskbar
            }
        }

        private void hibernateComputerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process hibcomp = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo2 = new System.Diagnostics.ProcessStartInfo();
            startInfo2.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo2.FileName = "shutdown";
            startInfo2.Arguments = "/h";
            hibcomp.StartInfo = startInfo2;
            hibcomp.Start();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().Show();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal; //Open Form
            this.ShowInTaskbar = true; //Show icon in taskbar
        }

        private void runAsAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsAdministrator() == false)
            {
                // Restart program and run as admin
                var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                startInfo.Verb = "runas";
                System.Diagnostics.Process.Start(startInfo);
                this.Close();
                return;
            }
        }
    }
}
