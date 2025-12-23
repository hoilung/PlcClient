using NewLife.Log;
using System;
using System.Windows.Forms;

namespace PlcClient
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //XTrace.LogPath = Application.StartupPath + "\\Logs";
            //XTrace.UseConsole();
          
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.Run(new Main());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            XTrace.Log.Error("CurrentDomain_UnhandledException {0}", (Exception)e.ExceptionObject);
            MessageBox.Show(e.ExceptionObject.ToString(), "软件未知异常", MessageBoxButtons.OK, MessageBoxIcon.Error);            
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            XTrace.Log.Error("Application_ThreadException {0}", e.Exception);
            MessageBox.Show(e.Exception.Message, "软件异常", MessageBoxButtons.OK, MessageBoxIcon.Error);            
        }
    }
}
