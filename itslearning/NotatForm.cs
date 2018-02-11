using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace itslearning
{
    public partial class NotatForm : Form
    {

        List<string> notater;
        public NotatForm()
        {
            InitializeComponent();
            notater = new List<string>();
            string[] currentNotater = Form1.instance.jsonObject.answers.ToObject<string[]>();
            for (int i = 0; i < currentNotater.Length; i++)
            {
                string notat = currentNotater[i];
                CreateNotat(i, notat);
            }
            Sort();
        }

        private void CreateNotat(int i, string notat)
        {
            notater.Add(notat);
            Button b = new Button();
            TextBoxAndButton box = new TextBoxAndButton(b);
            b.TabIndex = 100;
            box.TabIndex = i;
            box.MaximumSize = new Size(Width - 80, 60);
            box.Size = new Size(Width - 80, box.Height);
            box.Location = new Point(20, i * box.Height + 10);
            box.Text = notat;
            box.AutoSize = true;
            Controls.Add(box);
            Controls.Add(b);
            b.Size = new Size(box.Height, box.Height);
            b.Text = "X";
            b.FlatStyle = FlatStyle.Flat;
            b.Location = new Point(box.Location.X - b.Size.Width, box.Location.Y);
            b.Click += (o, e) =>
            {
                notater.Remove(box.Text);
                Controls.Remove(box);
                Controls.Remove(b);
                Sort();
                box.Dispose();
                b.Dispose();
            };
            Size = new Size(Width, notater.Count * box.Height + 120);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //logic get a new string[]
            List<string> theseAnswers = new List<string>();
            foreach(Control c in Controls)
            {
                if (c is TextBoxAndButton t)
                {
                    theseAnswers.Add(t.Text);
                }
            }
            Form1.instance.answers = theseAnswers.ToArray();
            Close();
            Dispose();
        }

        private void button2_Click(object sender, EventArgs e2)
        {
            CreateNotat(notater.Count, "");
        }

        private void Sort()
        {
            int index = 0;
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] is TextBoxAndButton t)
                {
                    t.Location = new Point(t.Location.X, index * t.Height + 10);
                    t.correspondingButton.Location = new Point(t.Location.X-t.correspondingButton.Size.Width, t.Location.Y);
                    index++;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string execute = System.IO.Directory.GetCurrentDirectory() + "\\answers.json";
            System.Diagnostics.Process.Start(execute);
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new CloudForm(Program.form, this).Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new UploadForm(this).Show();
        }
    }

    public class TextBoxAndButton : TextBox
    {
        public Button correspondingButton;

        public TextBoxAndButton(Button x)
        {
            correspondingButton = x;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}
