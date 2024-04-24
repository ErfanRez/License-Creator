using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoxLearn.License;

namespace LicenseCreator
{
    public partial class CreateLicense : Form
    {


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        public CreateLicense()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        const int ProductCode = 1;

        private void CreateLicense_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            textBoxX1.Text = ComputerInfo.GetComputerId();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            KeyManager km = new KeyManager(textBoxX1.Text);
            KeyValuesClass kv;
            string productkey = String.Empty;

            if (comboBox1.SelectedIndex == 0)
            {
                kv = new KeyValuesClass()
                {
                    Type = LicenseType.FULL,
                    Header = Convert.ToByte(9),
                    Footer = Convert.ToByte(6),
                    ProductCode = (byte)ProductCode,
                    Edition = Edition.ENTERPRISE,
                    Version = 1
                };
                if (km.GenerateKey(kv, ref productkey))
                    textBoxX3.Text = "ERROR";
            }
            else
            {
                kv = new KeyValuesClass()
                {
                    Type = LicenseType.TRIAL,
                    Header = Convert.ToByte(9),
                    Footer = Convert.ToByte(6),
                    ProductCode = (byte)ProductCode,
                    Edition = Edition.ENTERPRISE,
                    Version = 1,
                    Expiration = DateTime.Now.Date.AddDays(Convert.ToInt32(textBoxX2.Text))
                };
                if (km.GenerateKey(kv, ref productkey))
                    textBoxX3.Text = "ERROR";
            }
            textBoxX3.Text = productkey;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Application.OpenForms["MainForm"].Show();
            this.Close();
        }

        private void symbolBox1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBoxX3.Text);
        }
    }
}
