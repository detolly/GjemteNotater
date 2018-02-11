using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;

namespace itslearning
{
    public partial class CloudForm : Form
    {
        private Form1 form;
        private NotatForm notatForm;

        public CloudForm(Form1 form, NotatForm notatForm)
        {
            this.form = form;
            this.notatForm = notatForm;
            InitializeComponent();
            GetNewThings();
            label3.MaximumSize = new Size(Width - label3.Location.X, 0);
        }

        public Dictionary<string, string[]> json;

        void GetNewThings()
        {
            HttpWebRequest request = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create("http://server.detolly.no:5000/api/notat");
            }
            catch
            {
                var l = new Label();
                l.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                l.Text = "Could not connect to server. Close window";
                l.AutoSize = true;
                l.Location = new Point((Width / 2) - l.Size.Width / 2, (Height / 2) - l.Size.Height / 2);
                Controls.Add(l);
                return;
            }
            string html;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            json = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(html);

            var currentHeight = 60;
            var currentWidth = 10;

            foreach (var key in json.Keys)
            {
                Button b = new Button();
                Controls.Add(b);

                b.Click += (o, e) =>
                {
                    var result = MessageBox.Show("This will remvoe your current notes. There are no ways of getting them back except for copy and pasting config or uploading to cloud.", "Are you sure?", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        form.answers = json[key];
                        notatForm.Close();
                        Close();
                    }
                };
                b.Location = new Point(currentWidth, currentHeight);
                b.Size = new Size(20, 20);
                b.Text = ">";
                currentWidth += b.Size.Width;
                Label l = new Label();
                l.Cursor = Cursors.Hand;
                l.Location = new Point(currentWidth, currentHeight);
                var currentArr = json[key];
                l.Text = key;
                l.Click += (o,e) =>
                {
                    label3.Text = currentArr.Concatenate(Environment.NewLine);
                };
                Controls.Add(l);

                currentHeight += 30;

                currentWidth = 10;
            }
        }
    }
}
