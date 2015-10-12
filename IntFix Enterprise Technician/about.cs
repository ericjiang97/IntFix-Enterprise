using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntFix_Enterprise
{
    public partial class about : Form
    {
        public about()
        {
            InitializeComponent();
        }

        private void about_Load(object sender, EventArgs e)
        {
            label2.Text = "Version: " + ProductVersion;
            label5.Text = AssemblyDescription;
            liclabel.Text = Properties.Settings.Default.licensee;
            string date = Properties.Settings.Default.date.ToString();
            datelbl.Text = date;
            prodlbl.Text = Properties.Settings.Default.produID;
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
                new activate().Show();
        }
    }
}
