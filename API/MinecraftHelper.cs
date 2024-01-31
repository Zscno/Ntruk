using Windows.UI.Xaml.Media;
using Windows.UI;
using System.IO;

namespace Ntruk.API
{
    /// <summary>
    /// 关于Minecraft的转换操作。
    /// </summary>
    internal static class MinecraftHelper
    {
        /// <summary>
        /// 获取从<paramref name="fileName"/>解析出的Minecraft版本号。
        /// </summary>
        /// <param name="fileName">一个正确的Minecraft资源索引文件名。</param>
        /// <returns>一个正确的Minecraft版本号。</returns>
        public static string GetVersion(string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            string version;

            switch (name)
            {
                case "2":
                    version = "1.19.3";
                    break;
                case "3":
                    version = "1.19.4";
                    break;
                case "5":
                    version = "1.20";
                    break;
                case "8":
                    version = "1.20.2";
                    break;
                case "12":
                    version = "1.20.3";
                    break;
                default:
                    version = name;
                    break;
            }

            return version;
        }

        /// <summary>
        /// 获取从<paramref name="version"/>解析出的Minecraft资源索引文件名。
        /// </summary>
        /// <param name="version">一个正确的Minecraft版本号。</param>
        /// <returns>一个正确的Minecraft资源索引文件名。</returns>
        public static string GetFileName(string version)
        {
            string fileName;

            switch (version)
            {
                case "1.19.3":
                    fileName = "2.json";
                    break;
                case "1.19.4":
                    fileName = "3.json";
                    break;
                case "1.20":
                    fileName = "5.json";
                    break;
                case "1.20.2":
                    fileName = "8.json";
                    break;
                case "1.20.3":
                    fileName = "12.json";
                    break;
                default:
                    fileName = version + ".json";
                    break;
            }

            return fileName;
        }

        /// <summary>
        /// 获取从<paramref name="extensionName"/>解析出来的图标资源。
        /// </summary>
        /// <param name="extensionName">一个正确的Minecraft资源文件扩展名。</param>
        /// <returns>一组正确的图标资源。</returns>
        public static (string, SolidColorBrush) GetIcon(string extensionName)
        {
            string icon;
            SolidColorBrush iconColor;

            switch (extensionName)
            {
                case ".png":
                case ".icns":
                    icon = "\uEB9F";
                    iconColor = new SolidColorBrush(Color.FromArgb(255, 21, 135, 208));
                    break;
                case ".ogg":
                    icon = "\uE8D6";
                    iconColor = new SolidColorBrush(Color.FromArgb(255, 222, 126, 112));
                    break;
                case ".zip":
                    icon = "\uF012";
                    iconColor = new SolidColorBrush(Color.FromArgb(255, 18, 170, 159));
                    break;
                case ".mcmeta":
                case ".json":
                default:
                    icon = "\uE8A5";
                    iconColor = new SolidColorBrush(Color.FromArgb(255, 126, 155, 183));
                    break;
            }

            return (icon,  iconColor);
        }

        public static string GetExtensionName(string icon, string name)
        {
            string extensionName = string.Empty;
            switch (icon)
            {
                case "\uEB9F":
                    if (name == "icons/minecraft" || name == "icons/snapshot/minecraft")
                    {
                        extensionName = ".icns";
                    }
                    else
                    {
                        extensionName = ".png";
                    }
                    break;
                case "\uE8D6":
                    extensionName = ".ogg";
                    break;
                case "\uF012":
                    extensionName = ".zip";
                    break;
                case "\uE8A5":
                    if (name == "pack")
                    {
                        extensionName = ".mcmeta";
                    }
                    else if (name == "minecraft/font/include/unifont" || name.Split('/')[1] == "lang" || name == "minecraft/sounds")
                    {
                        extensionName = ".json";
                    }
                    break;
                default:
                    extensionName = string.Empty;
                    break;
            }

            return extensionName;
        }
    }
}
