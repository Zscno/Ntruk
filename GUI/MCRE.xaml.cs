using Ntruk.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Ntruk.GUI
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MCRE : Page
    {
        /// <summary>
        /// MCRE界面中的文件对象数组。
        /// </summary>
        internal static List<MCREObj> MCFileObjs;

        /// <summary>
        /// MCRE界面中的文件对象数组树形逻辑结构根节点。
        /// </summary>
        internal static StringTreeNode Root;

        /// <summary>
        /// MCRE界面中将要导出的文件集合。
        /// </summary>
        internal static List<MCREObj> ReadyData;

        public MCRE()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //loadingAnimation.IsActive = true;
            //MainPage.MCFileObjs = await GetMCREData();
            mainFrame.Navigate(typeof(MCREFilePage));
            ReadyData = new List<MCREObj>();
            //if (Objs == null)
            //{
            //    ContentDialogHelper.ShowErrorDialog("我们这里出了些问题：" + ExMessage + "\n请根据以上信息尝试解决问题。\n如还是有问题可以反馈到Github（在关于界面中）。");
            //}
            //contentView.ItemsSource = Objs;
            //loadingAnimation.IsActive = false;
        }

        private async Task<List<MCREObj>> GetMCREData()
        {
            try
            {
                #region 获取Minecraft资源索引文件内容。
                StorageFolder minecraftFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken");
                StorageFolder indexesFolder = await StorageFolder.GetFolderFromPathAsync(Path.Combine(minecraftFolder.Path, "assets", "indexes"));
                string version = IniHelper.ReadIni("Minecraft", "Version", MainPage.ConfigDataPath);
                string fileName = MinecraftHelper.GetFileName(version);
                StorageFile indexeFile = await StorageFile.GetFileFromPathAsync(Path.Combine(indexesFolder.Path, fileName));
                string fileText = await FileIO.ReadTextAsync(indexeFile);
                #endregion

                List<MCREObj> objs = new List<MCREObj>();

                #region 反序列化Minecraft资源索引文件内容，并将提取内容到列表。
                // 将文本转换为Json文档。
                JsonDocument document = JsonDocument.Parse(fileText);
                //解析Json文档中根对象objects中的所有名称。
                document.RootElement.TryGetProperty("objects", out JsonElement objElement);
                //遍历解析的所有名称。
                foreach (JsonProperty objProperty in objElement.EnumerateObject())
                {
                    string[] pathParts = objProperty.Name.Split('/');
                    (string, SolidColorBrush) tuple = MinecraftHelper.GetIcon(Path.GetExtension(objProperty.Name));
                    MCREObj obj = new MCREObj()
                    {
                        Hash = objProperty.Value.GetProperty("hash").ToString(),
                        Title = pathParts[pathParts.Length - 1],//MinecraftHelper.GetTitle(objProperty.Value.GetProperty("hash").ToString(), objProperty.Name),
                        Path = objProperty.Name,
                        Icon = tuple.Item1,
                        IconColor = tuple.Item2,
                    };
                    objs.Add(obj);
                    Root = new StringTreeNode("root");
                    Root.Add(pathParts, 0);
                }
                #endregion

                return objs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DetermineButton_Click(object sender, RoutedEventArgs e)
        {
            //loadingAnimation.IsActive = true;
            //determineButton.IsEnabled = false;
            //List<MCREObj> targetObjs = contentView.SelectedItems.Cast<MCREObj>().ToList();
            //if (targetObjs.Count == 0)
            //{
            //    ContentDialogHelper.ShowTipDialog("请选择要提取的资源。");
            //}
            //await CopyFile(targetObjs);
            //loadingAnimation.IsActive = false;
            //determineButton.IsEnabled = true;
            //ContentDialogHelper.ShowTipDialog("提取完毕。");
        }

        private async static Task CopyFile(List<MCREObj> targetObjs)
        {
            StorageFolder mcFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken");
            StorageFolder objsFolder = await StorageFolder.GetFolderFromPathAsync(Path.Combine(mcFolder.Path, "assets", "objects"));
            StorageFolder targetFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("TargetFolderToken");
            foreach (MCREObj objItem in targetObjs)
            {
                StorageFile objFile = await StorageFile.GetFileFromPathAsync(Path.Combine(objsFolder.Path, objItem.Hash.Substring(0, 2), objItem.Hash));
                await objFile.CopyAsync(targetFolder, objItem.Title);
            }
        }
    }
}