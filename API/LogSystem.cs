using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace Ntruk.GUI
{
    internal static class LogSystem
    {
        /// <summary>
        /// 日志文件。
        /// </summary>
        public static StorageFile LogFile { get; set; }

        /// <summary>
        /// 写入日志。
        /// </summary>
        /// <param name="level">日志等级。</param>
        /// <param name="page">日志所属界面</param>
        /// <param name="message">日志内容。</param>
        public async static Task WriteLog(LogLevel level, Page page, string message)
        {
            string[] pageNameParts = page.ToString().Split('.');
            string pageName = "[" + pageNameParts[pageNameParts.Length - 1] + "]";
            string levelString = string.Empty;
            switch (level)
            {
                case LogLevel.Info:
                    levelString = "[Info]";
                    break;
                case LogLevel.Warning:
                    levelString = "[Warning]";
                    break;
                case LogLevel.Error:
                    levelString = "[Error]";
                    break;
            }
            await FileIO.AppendTextAsync(LogFile, DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff]") + pageName + levelString + message + "\n", UnicodeEncoding.Utf8);
        }
    }

    /// <summary>
    /// 日志等级标识。
    /// </summary>
    internal enum LogLevel { Info, Warning, Error }
}
