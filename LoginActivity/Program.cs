using DeepNaiWorkshop_2796;
using DeepNaiWorkshop_2796.MyModel;
using DeepNaiWorkshop_2796.MyTool;
using DeepWorkshop.QQRot.FirstCity;
using DeepWorkshop.QQRot.FirstCity.MyTool;
using RegeditActivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace LoginActivity
{
    static class Program
    {
        private static bool IsSupportedRuntimeVersion = false;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(Program.Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.CurrentDomain_UnhandledException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Directory.SetCurrentDirectory(Application.StartupPath);
            /*
            if (!MyDotNetFrameworkUtil.IsSupportedRuntimeVersion())
            {
                IsSupportedRuntimeVersion = false;

                Application.Run(new RuntimeVerForm());

                
            }
            */
            Application.Run(new FrmLogin());

        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            string str = "出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";
            Exception exception = e.Exception;
            Program.writeLog(exception == null ? string.Format("应用程序线程错误:{0}", (object)e) : string.Format(str + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n", (object)exception.GetType().Name, (object)exception.Message, (object)exception.StackTrace));
            //注释掉，为了能自启动
            int num = (int)MessageBox.Show("软件发生未知错误,需要重启软件再操作！\r\n请将‘ErrLog.txt’发给程序员", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //强制退出
            //MySystemUtil.Exit("软件发生未知错误，重启软件尝试解决此问题");
            if(!String.IsNullOrWhiteSpace(CacheData.UserName)&& !String.IsNullOrWhiteSpace(CacheData.Ret))
            {
                YiYunUtil.Logout(CacheData.UserName, CacheData.Ret);
            }
            
            //强制退出
            System.Environment.Exit(0);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exceptionObject = e.ExceptionObject as Exception;
            string str = "出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";
            Program.writeLog(exceptionObject == null ? string.Format("Application UnhandledError:{0}", (object)e) : string.Format(str + "Application UnhandledException:{0};\n\r堆栈信息:{1}", (object)exceptionObject.Message, (object)exceptionObject.StackTrace));
            int num = (int)MessageBox.Show("软件发生未知错误,需要重启软件再操作！\r\n请将‘ErrLog.txt’发给程序员", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //强制退出
            //MySystemUtil.Exit("软件发生未知错误，重启软件尝试解决此问题");
            if(!String.IsNullOrWhiteSpace(CacheData.UserName) && !String.IsNullOrWhiteSpace(CacheData.Ret));
            {
                YiYunUtil.Logout(CacheData.UserName, CacheData.Ret);
            }
            //强制退出
            System.Environment.Exit(0);
        }


        private static void writeLog(string str)
        {
            StreamWriter streamWriter = new StreamWriter("ErrLog.txt", true);
            streamWriter.WriteLine(DateTime.Now.ToLongTimeString() + "=>" + str);
            streamWriter.WriteLine("---------------------------------------------------------");
            streamWriter.Close();

        }
    }
}
