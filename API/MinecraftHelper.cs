using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Ntruk.API
{
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
                    icon = "\uEB9F";
                    iconColor = new SolidColorBrush(Color.FromArgb(255, 169, 106, 242));
                    break;
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
                    icon = "\uE8A5";
                    iconColor = new SolidColorBrush(Color.FromArgb(255, 99, 127, 150));
                    break;
                case ".json":
                    icon = "\uE8A5";
                    iconColor = new SolidColorBrush(Color.FromArgb(255, 183, 183, 59));
                    break;
                default:
                    icon = "\uE8A5";
                    iconColor = new SolidColorBrush(Color.FromArgb(255, 126, 155, 183));
                    break;
            }

            return (icon,  iconColor);
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
        {
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
                case "740df156fd6bcd90452c4a4ddcae22dc3ad9bfed":
                    title = "语言-ca_es.json";
                    break;
                case "7779bdb781fb210176c653b1f8dc45c3f0850947":
                    title = "语言-cs_cz.json";
                    break;
                case "56d2e24c814facfa4b5eec059eba3c9620b66732":
                    title = "语言-cy_gb.json";
                    break;
                case "7c50a40a1e4a2d147a78ef980fb647de2cabbbd8":
                    title = "语言-da_dk.json";
                    break;
                case "0531cd05d88f629218a8b18abc521dc580508dd9":
                    title = "语言-de_at.json";
                    break;
                case "40d67111dc6d737bd223c1e7782ac114cd21cf83":
                    title = "语言-de_ch.json";
                    break;
                case "80a753919b5f1d6838d02b482277f2ba6e94d7e6":
                    title = "语言-de_de.json";
                    break;
                case "4b4543bb7cd5c1c5e7de821070623eb4b7d17531":
                    title = "语言-el_gr.json";
                    break;
                case "872b3e7829aee47bea16c5032f82a74dc34b6385":
                    title = "语言-en_au.json";
                    break;
                case "9654e8295217f8b2313d5c9520a5ed8f2ce122b7":
                    title = "语言-en_ca.json";
                    break;
                case "7c056bd9eac2d0a34254f634ac48855ad9446f0a":
                    title = "语言-en_gb.json";
                    break;
                case "55786e57bf574dd22a7dfda951b2f4fc0c0de5d9":
                    title = "语言-en_nz.json";
                    break;
                case "a93d6fbeba4a849ea425c6f57ce6f7156630eec4":
                    title = "语言-en_pt.json";
                    break;
                case "e19794de9a0b737e722a665f5ba2f8f3be3892fd":
                    title = "语言-en_ud.json";
                    break;
                case "2aaeeb666763243a172b1ac0e3bb77e6b3748610":
                    title = "语言-enp.json";
                    break;
                case "d1c202b358bb62bf6cc93c588952f28cf5912ae8":
                    title = "语言-enws.json";
                    break;
                case "3aff86973d5c45ae80034df9cf7eeacb7c01f74d":
                    title = "语言-eo_uy.json";
                    break;
                case "dbc39da314be2ddb8d6835a0a981b3748ced2c59":
                    title = "语言-es_ar.json";
                    break;
                case "69f822503c04094121dbf7f20faabf4f8feee375":
                    title = "语言-es_cl.json";
                    break;
                case "7d6aa9105ae19e56d2be5339e73cf29a64a8ebcc":
                    title = "语言-es_ec.json";
                    break;
                case "42288c6d2c3623124a8d07379e291dee30a04d1a":
                    title = "语言-es_es.json";
                    break;
                case "3449b4a5611d203d4edc7e1dc916e25ae0d66242":
                    title = "语言-es_mx.json";
                    break;
                case "b1ba29cebad8161027a06836491ead66354b49e9":
                    title = "语言-es_uy.json";
                    break;
                case "15eb232a624ff387868426148c380622a58db2b9":
                    title = "语言-es_ve.json";
                    break;
                case "165568d8e18cf8cdfc3c6af5fc2c2217b6d3ef7b":
                    title = "语言-esan.json";
                    break;
                case "982d6f89dd1e42f1ee82ee0efa1c431df035e903":
                    title = "语言-et_ee.json";
                    break;
                case "5a674ccfa6f333fc5a84062acfc2cc141155da62":
                    title = "语言-eu_es.json";
                    break;
                case "dcd738c37aedf7b465047e941191d4c582b062cf":
                    title = "语言-fa_ir.json";
                    break;
                case "e3077d9f2d088304f57cebae6e808e802205ec63":
                    title = "语言-fi_fi.json";
                    break;
                case "9350d63a942fc3f6abe83bb5b105b7047a39b7a8":
                    title = "语言-fil_ph.json";
                    break;
                case "00007386081b4c8554508987d95c484c57569d2e":
                    title = "语言-fo_fo.json";
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
