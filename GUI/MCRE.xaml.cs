using Ntruk.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public MCRE()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadingAnimation.IsActive = true;
            await GetMCREData();
            if (Objs == null)
            {
                ContentDialogHelper.ShowErrorDialog("我们这里出了些问题：" + ExMessage + "\n请根据以上信息尝试解决问题。\n如还是有问题可以反馈到Github（在关于界面中）。");
            }
            contentView.ItemsSource = Objs;
            loadingAnimation.IsActive = false;
        }

        List<MCREObj> Objs;
        string ExMessage = string.Empty;

        private async Task GetMCREData()
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

                Objs = new List<MCREObj>();

                #region 反序列化Minecraft资源索引文件内容，并将提取内容到列表。
                // 将文本转换为Json文档。
                JsonDocument document = JsonDocument.Parse(fileText);
                //解析Json文档中根对象objects中的所有名称。
                document.RootElement.TryGetProperty("objects", out JsonElement objElement);
                //遍历解析的所有名称。
                foreach (JsonProperty objProperty in objElement.EnumerateObject())
                {
                    (string, SolidColorBrush) tuple = MinecraftHelper.GetIcon(Path.GetExtension(objProperty.Name));
                    MCREObj obj = new MCREObj()
                    {
                        Hash = objProperty.Value.GetProperty("hash").ToString(),
                        Title = Path.GetFileName(objProperty.Name),
                        Icon = tuple.Item1,
                        IconColor = tuple.Item2,
                        Name = objProperty.Name,
                    };
                    Objs.Add(obj);
                }
                #endregion
            }
            catch (Exception ex)
            {
                ExMessage = ex.Message;
                Objs = null;
            }
        }

        private async void DetermineButton_Click(object sender, RoutedEventArgs e)
        {
            loadingAnimation.IsActive = true;
            determineButton.IsEnabled = false;
            List<MCREObj> targetObjs = contentView.SelectedItems.Cast<MCREObj>().ToList();
            if (targetObjs.Count == 0)
            {
                ContentDialogHelper.ShowTipDialog("请选择要提取的资源。");
            }
            await CopyFile(targetObjs);
            loadingAnimation.IsActive = false;
            determineButton.IsEnabled = true;
            ContentDialogHelper.ShowTipDialog("提取完毕。");
        }

        // 在下一次更新中将此方法移动至PageHelper类型中。
        private static RelativePanel FindChild<RelativePanel>(DependencyObject depObj) where RelativePanel : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child != null && child is RelativePanel panel)
                {
                    return panel;
                }
                else
                {
                    RelativePanel childItem = FindChild<RelativePanel>(child);
                    if (childItem != null)
                    {
                        return childItem;
                    }
                }
            }
            return null;
        }

        private async static Task CopyFile(List<MCREObj> targetObjs)
        {

            StorageFolder mcFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken");
            StorageFolder objsFolder = await StorageFolder.GetFolderFromPathAsync(Path.Combine(mcFolder.Path, "assets", "objects"));
            StorageFolder targetFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("TargetFolderToken");
            foreach (MCREObj objItem in targetObjs)
            {
                StorageFolder subFolder = await StorageFolder.GetFolderFromPathAsync(Path.Combine(objsFolder.Path, objItem.Hash.Substring(0, 2)));
                foreach (StorageFile objFile in await subFolder.GetFilesAsync())
                {
                    if (objFile.Name == objItem.Hash)
                    {
                        string[] temp = objItem.Name.Split("/");
                        string extension = Path.GetExtension(objItem.Name);
                        string fileName = string.Join('-', temp) + extension;
                        await objFile.CopyAsync(targetFolder, fileName);
                    }
                }
            }
        }
    }
}