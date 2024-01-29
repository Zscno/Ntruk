using System.Text;

namespace Ntruk.API
{
    /// <summary>
    /// 关于Ini文件的IO操作。
    /// </summary>
    internal static class IniHelper
    {
        // 声明INI文件的写操作函数
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // 声明INI文件的读操作函数
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);


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
        /// <returns>对应的键值。</returns>
        public static string ReadIni(string section, string key, string path)
        {
            // 每次从ini中读取多少字节
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", temp, 255, path);
            return temp.ToString();
        }
    }
}
