using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Direct2D1;
using System.Drawing;
using SharpDX.DirectWrite;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;

namespace itslearning
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static Form1 form;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new Form1();
            while(true)
            {
                Application.DoEvents();
                if (form.hasClosed || form.IsDisposed)
                    break;
                form.CustomUpdate();
                System.Threading.Thread.Sleep(1);
            }
        }
    }
}
