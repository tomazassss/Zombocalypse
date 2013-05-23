using System;
using System.Windows.Forms;
using MapEditor.Forms;

namespace MapEditor
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (FormMain mainForm = new FormMain())
            {
                Application.Run(mainForm);
            }
        }
    }
}

