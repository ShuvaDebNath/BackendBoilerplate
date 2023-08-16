using System;
using System.IO;

namespace Boilerplate.Entities.GlobalInfo
{
    public static class GlobalMethods
    {
        public static string UITODBDateFormat(string date)
        {
            try
            {
                if (date.Contains("T"))
                {
                    return DateTime.Parse(date.Split("T")[0]).AddDays(0).ToString(GlobalParams.DateFormateyyyyMMdd);
                }
                else
                {
                    string d = string.IsNullOrEmpty(date) ? DateTime.Now.ToString(GlobalParams.DateFormateyyyyMMdd) : (date.Split("/")[2] + "-" + date.Split("/")[1] + "-" + date.Split("/")[0]);
                    return d;
                }
            }
            catch (Exception ex)
            {
                return DateTime.Now.ToString(GlobalParams.DateFormateyyyyMMdd);
            }
        }

        public static string DBTOUIDateFormat(string date)
        {
            try
            {
                if (date.Contains("T"))
                {
                    return DateTime.Parse(date.Split("T")[0]).ToString(GlobalParams.DateFormateddMMyyyy);
                }
                else
                {
                    string d = string.IsNullOrEmpty(date) ? "01/01/1900" : DateTime.Parse(date).ToString(GlobalParams.DateFormateddMMyyyy);
                    return d;
                }
            }
            catch (Exception ex)
            {
                return "01/01/1900";
            }
        }

        public static string GetFormatedExceptionMsg(string userName,string controllerName,string actionName,string msg,string stackTrace,Exception? innerException)
        {
            return $"\nUser Name: {userName}\nController: {controllerName}\nActionName: {actionName}\nMsg: {msg}\nStack Trace: {stackTrace}\nInnerException: {innerException}\n\n";

        }

        public static string GetFormatedLogMsg(string userName, string controllerName, string actionName, string msg)
        {
            return $"\nUser Name: {userName}\nController: {controllerName}\nActionName: {actionName}\nMsg: {msg}\n\n";

        }

        public static byte[] GetImageByPath(string path, string rootPath)
        {
            try
            {
                //string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("CMS.Entities.dll", string.Empty);
                string FilePath = string.Format("{0}\\member_files\\img\\{1}", rootPath, path.Replace("/", @"\\"));
                return File.ReadAllBytes(FilePath);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
