using Windows.UI.Xaml.Media;
using Windows.UI;
using System.IO;
using System.Collections.Generic;
using System;
using Windows.Storage;
using System.Threading.Tasks;

namespace Ntruk.API
{
    /// <summary>
    /// 关于Minecraft与程序的转换操作。
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
        /// <param name="extensionName">一个正确的Minecraft资源文件扩展名。（最前面有“.”的）</param>
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

        /// <summary>
        /// 获取从<paramref name="icon"/>和<paramref name="name"/>解析出来的资源文件扩展名（最前面有“.”的）。
        /// </summary>
        /// <param name="icon">一个正确的图标资源代码。</param>
        /// <param name="name">一个正确的资源文件的相对路径。</param>
        /// <returns>一个正确的资源文件扩展名（最前面有“.”的）。</returns>
        [Obsolete("此方法已被弃用。")]
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

        /// <summary>
        /// 获取指定文件夹中所有Minecraft的版本号。
        /// </summary>
        /// <param name="folder">一个正确的Minecraft文件夹。</param>
        /// <returns>指定文件夹中所有Minecraft的版本号。</returns>
        public async static Task<string[]> GetAllVersions(StorageFolder folder)
        {
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            string[] versions = new string[files.Count];
            for (int i = 0; i < files.Count; i++)
            {
                versions[i] = GetVersion(files[i].Path);
            }
            Array.Sort(versions);
            return versions;
        }

        /// <summary>
        /// 获取从<paramref name="hash"/>解析出来的标题。
        /// </summary>
        /// <param name="hash">一个正确的哈希值。</param>
        /// <param name="fullName">一个正确的全称。</param>
        /// <returns>一个正确的标题。
        /// <para>*如果未解析成功将返回<paramref name="fullName"/> </para>
        /// </returns>
        public static string GetTitle(string hash, string fullName)
        {//  为图标图片添加缩略图。。。
            string title;
            switch (hash)
            {
                case "b62ca8ec10d07e6bf5ac8dae0c8c1d2e6a1e3356":
                    title = "图标-正式版128x128.png";
                    break;
                case "5ff04807c356f1beed0b86ccf659b44b9983e3fa":
                    title = "图标-正式版16x16.png";
                    break;
                case "8030dd9dc315c0381d52c4782ea36c6baf6e8135":
                    title = "图标-正式版256x256.png";
                    break;
                case "af96f55a90eaf11b327f1b5f8834a051027dc506":
                    title = "图标-正式版32x32.png";
                    break;
                case "b80b6e9ff01c78c624df5429e1d3dcd3d5130834":
                    title = "图标-正式版48x48.png";
                    break;
                case "f00657542252858a721e715a2e888a9226404e35":
                    title = "图标-正式版.icns";
                    break;
                case "958d57021d8009de55d6e9e19957a72545e3c30c":
                    title = "图标-快照版128x128.png";
                    break;
                case "949afe72d4d3d785dab52d8baaefeb0e38b3c067":
                    title = "图标-快照版16x16.png";
                    break;
                case "9f84f917a09facacf1235eed3fa77789e4554afb":
                    title = "图标-快照版256x256.png";
                    break;
                case "26ad18d9f4ef0a71255459b5f01b738b81dbc7dc":
                    title = "图标-快照版32x32.png";
                    break;
                case "df274fe57c49ef1af6d218703d805db76a5c8af9":
                    title = "图标-快照版48x48.png";
                    break;
                case "65ebca3306ccd6d7f9d5de8f1cc7a1216d80246d":
                    title = "图标-快照版.icns";
                    break;
                case "f8d4768707b20359f2f7660346bd3a84b6ee27b1":
                    title = "字体-unifont.json";
                    break;
                case "109663114d0099c48a703626c8462e07d802e08b":
                    title = "字体-unifont.zip";
                    break;
                case "0c4a7ce69ee03d15b6ff2706eca246ab4234c9d1":
                    title = "语言-af_za.json";
                    break;
                case "74c3cb140919b70e64484a9040d490a2b31977ba":
                    title = "语言-ar_sa.json";
                    break;
                case "d8a6a2065e06049db7b9d82a6eab97276b953dca":
                    title = "语言-ast_es.json";
                    break;
                case "78689dcb47f101e56db5386dded196927c8efa43":
                    title = "语言-az_az.json";
                    break;
                case "ac692046a2c0e5414aaeafe76200a67b9f252aff":
                    title = "语言-ba_ru.json";
                    break;
                case "e7672a4de5a730a1f92eb804df3d8e4146fdafce":
                    title = "语言-bar.json";
                    break;
                case "2112ad08ccde94cdbc690a7f5c443df58d52d2e0":
                    title = "语言-be_by.json";
                    break;
                case "fbdd0c830632939ea1cd20a331799086e0b5076a":
                    title = "语言-bg_bg.json";
                    break;
                case "f6dd7169bb09dd53497cc9a561ad9ab87ce4c5af":
                    title = "语言-br_fr.json";
                    break;
                case "ff77572950f2d837493d8e495191372646f1a3fc":
                    title = "语言-brb.json";
                    break;
                case "ff3a25bea2b0c235a91b7bfdec9900c7107dbd43":
                    title = "语言-bs_ba.json";
                    break;
                // TODO
                default:
                    title = fullName;
                    break;
            }
            return title;
        }
    }
}
