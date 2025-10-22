using System;
using System.Windows.Forms;

namespace ChatServer
{
    static class Program
    {
        /// <summary>
        /// Uygulamanin ana giris noktasi
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Windows Forms ayarlarini yapilandir
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Server formunu baslat
            Application.Run(new ServerForm());
        }
    }
}

