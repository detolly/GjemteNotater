using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace itslearning
{
    public partial class Form1 : Form
    {
        public bool hasClosed = false;
        int currentIndex = 0;
        Label current;
        string[] _answers;

        public string[] answers
        {
            get
            {
                return _answers;
            }
            set
            {
                _answers = value;
                jsonObject.answers = new JArray(_answers);
                System.IO.File.WriteAllText("answers.json", JsonConvert.SerializeObject(jsonObject, Formatting.Indented));
            }
        }
        TextBox searchBox = new TextBox();
        int placementOfLabel = Screen.PrimaryScreen.WorkingArea.Width / 3;
        public static Form1 instance;
        public string config;
        private dynamic _jsonObject;
        public dynamic jsonObject
        {
            get
            {
                return _jsonObject;
            }
            set
            {
                _jsonObject = value;
                System.IO.File.WriteAllText("answers.json", JsonConvert.SerializeObject(_jsonObject, Formatting.Indented));
            }
        }

        public Form1()
        {
            InitializeComponent();
            StartApp();
            Show();
            //if (CheckNewVersion())
            //{
            //    hasClosed = true; return;
            //}
            CheckSettings();
            //new NotatForm().Show();
        }

        private bool CheckNewVersion()
        {
            /*using (System.Net.WebClient client = new System.Net.WebClient())
            {
                if (client.DownloadString("http://www.detolly.no/itslearning/version.txt") != "1.0.2")
                {
                    var g = MessageBox.Show("New Update Available. Should Download?", "Update", MessageBoxButtons.YesNo);
                    if (g == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("setup.exe");
                        return true;
                    }
                }
            }
            */return false;
        }

        private void StartApp()
        {
            if (!System.IO.File.Exists("answers.json"))
                System.IO.File.AppendAllText("answers.json", "{\"answers\":[\"CTRL+Q for å endre notater.\"],\"settings\":{}}");
            instance = this;
            Width = Screen.PrimaryScreen.Bounds.Width;
            Height = Screen.PrimaryScreen.Bounds.Height;
            TopMost = true;
            config = System.IO.File.ReadAllText("answers.json");
            current = new Label();
            current.AutoSize = true;
            current.ForeColor = Color.White;
            current.ForeColor = Color.FromArgb(255, 255, 255);
            current.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width / 2, 0);
            Controls.Add(current);
            searchBox.Visible = false;
            Controls.Add(searchBox);
            searchBox.BackColor = Color.Black;
            searchBox.ForeColor = Color.White;
            searchBox.BorderStyle = BorderStyle.FixedSingle;
            searchBox.MaximumSize = new Size(0, 0);
            searchBox.AutoSize = true;
            searchBox.TextChanged += textChangedBox;
            current.Location = new Point(placementOfLabel + searchBox.Size.Width, Screen.PrimaryScreen.Bounds.Height - current.Height);
            searchBox.Location = new Point(placementOfLabel, Screen.PrimaryScreen.Bounds.Height - searchBox.Size.Height);
            jsonObject = JsonConvert.DeserializeObject(config);
            answers = jsonObject.answers.ToObject<string[]>();

            currentIndex = -1;
            shiftCurrentText(true);
        }

        private void CheckSettings()
        {
            if (!((bool)(jsonObject.settings.pn102 ?? false)))
            {
                MessageBox.Show(@"Ny i version 1.0.2:

    +Custom notater: Ctrl+Q
    +Bedre søking.
    +Nytt navn: Gjemte Notater (istedenfor Itslearning).

    -Fjernet Herobrine.");
                jsonObject.settings.pn102 = "true";
                jsonObject = jsonObject;
            }
        }

        private void textChangedBox(object sender, System.EventArgs e)
        {
            for (int i = 0; i < answers.Length; i++)
            {
                if (answers[i].ToLower().Contains(searchBox.Text))
                {
                    currentIndex = i;
                }
            }
            current.Text = currentIndex + 1 + " " + answers[currentIndex];
        }

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        bool isDown = false;
        bool isSearching = false;

        public void CustomUpdate()
        {
            if (GetAsyncKeyState(Keys.Left) < 0 && !isDown)
            {
                shiftCurrentText(false);
                isDown = true;
            }
            else if (GetAsyncKeyState(Keys.Right) < 0 && !isDown)
            {
                shiftCurrentText(true);
                isDown = true;
            }
            else if (GetAsyncKeyState(Keys.Down) < 0 && !isDown)
            {
                current.Visible = false;
                searchBox.Visible = false;
                searchBox.Enabled = false;
                isDown = true;
            }
            else if (GetAsyncKeyState(Keys.Up) < 0 && !isDown)
            {
                current.Visible = true;
                searchBox.Visible = isSearching;
                searchBox.Enabled = isSearching;
                isDown = true;
            }
            else if (GetAsyncKeyState(Keys.ControlKey) < 0 && GetAsyncKeyState(Keys.F) < 0 && !isDown)
            {
                isDown = true;
                isSearching = !isSearching;
                searchBox.Visible = isSearching;
                searchBox.Enabled = isSearching;
                searchBox.Focus();
            }
            else if (GetAsyncKeyState(Keys.ControlKey) < 0 && GetAsyncKeyState(Keys.Q) < 0 && !isDown)
            {
                isDown = true;
                new NotatForm().Show();
            }
            else if (GetAsyncKeyState(Keys.ControlKey) >= 0 && GetAsyncKeyState(Keys.Q) >= 0 && GetAsyncKeyState(Keys.F) >= 0 && GetAsyncKeyState(Keys.Up) >= 0 && GetAsyncKeyState(Keys.Down) >= 0 && GetAsyncKeyState(Keys.Right) >= 0 && GetAsyncKeyState(Keys.Left) >= 0)
            {
                isDown = false;
            }
        }

        private void shiftCurrentText(bool left)
        {
            if (left) currentIndex++; else currentIndex--;
            if (currentIndex < 0) currentIndex = answers.Length - 1;
            if (currentIndex > answers.Length - 1) currentIndex = 0;
            current.Text = currentIndex + 1 + " " + answers[currentIndex];
            current.Location = new Point(placementOfLabel + searchBox.Size.Width, Screen.PrimaryScreen.Bounds.Height - current.Height);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            hasClosed = true;
        }
    }
}
