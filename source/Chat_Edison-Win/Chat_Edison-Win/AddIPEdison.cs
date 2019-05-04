using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Edison_Win
{
    public partial class AddIPEdison : Form
    {
        fClient clientForm;
        public AddIPEdison(fClient form)
        {
            InitializeComponent();
            this.clientForm = form;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            if (ip.Equals(""))
            {
                
            }else
            {
                this.clientForm.AddIPIntoCb(ip);
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
