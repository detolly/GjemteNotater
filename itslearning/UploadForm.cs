using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace itslearning
{
    public partial class UploadForm : Form
    {
        private NotatForm form;
        private static readonly Encoding encoding = Encoding.UTF8;

        public UploadForm(NotatForm form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //logic here
            Close();
        }
    }
}
