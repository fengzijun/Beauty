using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Beauty.App
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
       

        [STAThread]
        static void Main()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(Form1_UIThreadException);

            // Set the unhandled exception mode to force all Windows Forms errors to go through     
            // our handler.     
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event.      
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException); 
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Main(""));
            Application.Run(new Login());
        }

        private static void Form1_UIThreadException(object sender, ThreadExceptionEventArgs t)
        {
            Exception ex = (Exception)t.Exception;
            string errorMsg = "An application error occurred. Please contact the adminstrator " +
                "with the following information:/n/n";
            errorMsg += ex.Message + "/n/nStack Trace:/n" + ex.StackTrace + "//" + ex.Source;
            BeautyService.BeautyServiceClient client = new BeautyService.BeautyServiceClient();
            client.Log(errorMsg);
            MessageBox.Show("程序发生异常，请重新运行");
        }

        // Handle the UI exceptions by showing a dialog box, and asking the user whether     
        // or not they wish to abort execution.     
        // NOTE: This exception cannot be kept from terminating the application - it can only      
        // log the event, and inform the user about it.      
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            string errorMsg = "An application error occurred. Please contact the adminstrator " +
                  "with the following information:/n/n";
            errorMsg += ex.Message + "/n/nStack Trace:/n" + ex.StackTrace + "//" + ex.Source;

            //MessageBox.Show("2:" + errorMsg);

            BeautyService.BeautyServiceClient client = new BeautyService.BeautyServiceClient();
            client.Log(errorMsg);
            MessageBox.Show("程序发生异常，请重新运行");
        }     
    }
}
