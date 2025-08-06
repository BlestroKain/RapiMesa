using SQLitePCL;
using System;
using System.Windows.Forms;

namespace Rapimesa
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Batteries.Init();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}
