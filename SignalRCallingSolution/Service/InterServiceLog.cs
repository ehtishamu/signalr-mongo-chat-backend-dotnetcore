using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SignalRCallingSolution.Service
{
    public static class InterServiceLog
    {

        //public static void sendEmail(ExceptionsTbl execptionsTbl)
        //{
        //    string directoryOther = Path.GetPathRoot(Environment.CurrentDirectory) + "\\OtherErrors";
        //    if (!Directory.Exists(directoryOther))
        //    {
        //        Directory.CreateDirectory(directoryOther);
        //    }
        //    string filePathOther = directoryOther + "\\errors_" + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".txt";
        //    using (StreamWriter writer = new StreamWriter(filePathOther, true))
        //    {
        //        writer.WriteLine("User_Name :" + execptionsTbl.Requester + Environment.NewLine + /*"Practice_Code :" + User_Profile.PracticeCode*/"" + Environment.NewLine + "<br/> <br/> Message :" + execptionsTbl.ExecptionMessage + Environment.NewLine + Environment.NewLine + "URI :" + execptionsTbl.ReqeustURL + Environment.NewLine + Environment.NewLine + "Request parameters :" + execptionsTbl.RequestPrams + Environment.NewLine + Environment.NewLine + "StackTrace :" + execptionsTbl.Exceptionstacktrace + Environment.NewLine + Environment.NewLine +
        //           "///------------------Inner Exception------------------///" + Environment.NewLine + execptionsTbl.InnerExection + "" + Environment.NewLine +
        //           "Date :" + DateTime.Now.ToString() + Environment.NewLine + Environment.NewLine + "-------------------------------------------------------||||||||||||---End Current Exception---||||||||||||||||-------------------------------------------------------" + Environment.NewLine);
        //    }
        //    try
        //    {
        //        var emailHTML = "User_Name :" + execptionsTbl.Requester + Environment.NewLine + /*"Practice_Code :" + User_Profile.PracticeCode*/"" + Environment.NewLine + Environment.NewLine + "<br/> <br/> Message :" + execptionsTbl.ExecptionMessage + "<br/> <br/> URI :" + execptionsTbl.ReqeustURL + "<br/> <br/> Request parameters :" + execptionsTbl.RequestPrams + "<br/> <br/> StackTrace :" + execptionsTbl.Exceptionstacktrace + "<br/> <br/> ///------------------Inner Exception------------------///  <br/>" + execptionsTbl.InnerExection + " <br/>";
        //        emailHTML += "Date :" + DateTime.Now.ToString() + "<br/> <br/> -------------------------------------------------------||||||||||||---End Current Exception---||||||||||||||||-------------------------------------------------------";
        //        //Helper.SendEmail("talkehrexception@mtbc.com", "talkEHR Exception " + DateTime.Now.ToString("MM/dd/yyyy"), emailHTML);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Helper.SendEmail("talkehrexception@mtbc.com", "talkEHR Exception on Email Exception " + DateTime.Now.ToString("MM/dd/yyyy"), ex.Message);
        //    }
        //}
        public static void WriteLog(string type, string jsonData)
        {
            try
            {
                string directoryOther = Path.GetPathRoot(Environment.CurrentDirectory) + "TelehealthLogs";
                if (!Directory.Exists(directoryOther))
                {
                    Directory.CreateDirectory(directoryOther);
                }
                string filePathOther = directoryOther + "\\RequestLog" + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".txt";
                using (StreamWriter writer = new StreamWriter(filePathOther, true))
                {
                    writer.WriteLine("Date" + DateTime.Now + "==" + type + ":" + jsonData + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Wrting Logs", ex.Message,
                      EventLogEntryType.Error);
            }


        }
        public static void WriteExceptionLog(string exception, string innerException)
        {
            try
            {
                string directoryOther = AppDomain.CurrentDomain.BaseDirectory + "/ExceptionLogs";
                if (!Directory.Exists(directoryOther))
                {
                    Directory.CreateDirectory(directoryOther);
                }
                string filePathOther = directoryOther + "\\ExceptionLogs" + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".txt";
                using (StreamWriter writer = new StreamWriter(filePathOther, true))
                {
                    writer.WriteLine("Date" + DateTime.Now + "==" + exception + ":" + innerException + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Wrting Logs", ex.Message,
                      EventLogEntryType.Error);
            }


        }
        public static void WriteLogSignalR(string type, string jsonData)
        {
            try
            {
                string directoryOther = AppDomain.CurrentDomain.BaseDirectory + "/Logs";
                if (!Directory.Exists(directoryOther))
                {
                    Directory.CreateDirectory(directoryOther);
                }
                string filePathOther = directoryOther + "\\SignalR_RequestLog" + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".txt";
                using (StreamWriter writer = new StreamWriter(filePathOther, true))
                {
                    writer.WriteLine("Date" + DateTime.Now + "==" + type + ":" + jsonData + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Wrting Logs", ex.Message,
                      EventLogEntryType.Error);
            }


        }
    }
}

