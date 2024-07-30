using System.Runtime.InteropServices;
using System.Text;

namespace Ntruk.API
{
    internal static class IniHelper
    {
        // 声明INI文件的写操作函数
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // 声明INI文件的读操作函数
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        /// <summary>
        /// 写入Ini文件。
        /// </summary>
        /// <param name="section">节点名称。</param>
        /// <param name="key">键名。</param>
        /// <param name="value">对应的键值。</param>
        /// <param name="path">Ini文件路径。</param>
        public static void WriteIni(string section, string key, string value, string path)
        {
            WritePrivateProfileString(section, key, value, path);
        }

        /// <summary>
        /// 读取Ini文件。
        /// </summary>
        /// <param name="section">节点名称。</param>
        /// <param name="key">键名。</param>
        /// <param name="path">Ini文件路径。</param>
        /// <returns><paramref name="section"/>、<paramref name="key"/>对应的键值。</returns>
        public static string ReadIni(string section, string key, string path)
        {
            StringBuilder temp = new StringBuilder(255);// 每次从ini中读取多少字节
            GetPrivateProfileString(section, key, "", temp, 255, path);
            return temp.ToString();
        }
    }
}
