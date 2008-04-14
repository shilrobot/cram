using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Cram
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CramForm());
            return 0;
        }
    }
}