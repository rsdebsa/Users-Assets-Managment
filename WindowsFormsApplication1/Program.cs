using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ریست مسیر دیتابیس
            //Properties.Settings.Default.DatabasePath = "";
            //Properties.Settings.Default.Save();

            Application.Run(new Form3()); // یا هر فرم شروع
       
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new Form3());
        }
    }
}
