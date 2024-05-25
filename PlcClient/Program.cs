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
            XTrace.UseConsole();
            XTrace.LogPath = Application.StartupPath + "\\Logs";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.Run(new Main());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString(), "软件未知异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            XTrace.WriteException((Exception)e.ExceptionObject);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "软件异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            XTrace.WriteException(e.Exception);
        }
    }
}
