using Ntruk.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI;
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
            loading.IsActive = true;
            await GetMCREData();
            if (Objs == null)
            {
                ContentDialogHelper.ShowErrorDialog("我们这里出了些问题：" + ExMessage + "\n请根据以上信息尝试解决问题。\n如还是有问题可以反馈到Github。");
            }
            content.ItemsSource = Objs;
            loading.IsActive = false;
        }

        List<MCREObj> Objs;
        string ExMessage = string.Empty;

        private async Task GetMCREData()
        {
            try
            {
                Objs = new List<MCREObj>();
                StorageFolder folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken");
                StorageFolder mcFolder = await StorageFolder.GetFolderFromPathAsync(Path.Combine(folder.Path, "assets", "indexes"));
                //string filePath = Path.Combine(IniHelper.ReadIni("Minecraft", "Folder", MainPage.ConfigDataPath), "assets", "indexes");
                string version = IniHelper.ReadIni("Minecraft", "Version", MainPage.ConfigDataPath);
                switch (version)
                {
                    case "1.19.3":
                        version = "2.json";
                        break;
                    case "1.19.4":
                        version = "3.json";
                        break;
                    case "1.20":
                        version = "5.json";
                        break;
                    case "1.20.2":
                        version = "8.json";
                        break;
                    case "1.20.3":
                        version = "12.json";
                        break;
                    default:
                        version += ".json";
                        break;
                }
                // 获取索引文件内容。
                StorageFile file = await StorageFile.GetFileFromPathAsync(Path.Combine(mcFolder.Path, version));
                string fileText = await FileIO.ReadTextAsync(file); //File.ReadAllText(Path.Combine(filePath, version));
                // 反序列化索引文件内容。
                JsonDocument document = JsonDocument.Parse(fileText);
                document.RootElement.TryGetProperty("objects", out JsonElement objElement);
                foreach (JsonProperty objProperty in objElement.EnumerateObject())
                {
                    string[] title = objProperty.Name.Split('.');
                    string icon;
                    SolidColorBrush iconColor;
                    switch (title[1])
                    {
                        case "png":
                        case "icns":
                            icon = "\uEB9F";
                            iconColor = new SolidColorBrush(Color.FromArgb(255, 21, 135, 208));
                            break;
                        case "ogg":
                            icon = "\uE8D6";
                            iconColor = new SolidColorBrush(Color.FromArgb(255, 222, 126, 112));
                            break;
                        case "zip":
                            icon = "\uF012";
                            iconColor = new SolidColorBrush(Color.FromArgb(255, 18, 170, 159));
                            break;
                        case "mcmeta":
                        case "json":
                        default:
                            icon = "\uE8A5";
                            iconColor = new SolidColorBrush(Color.FromArgb(255, 126, 155, 183));
                            break;
                    }
                    MCREObj obj = new MCREObj()
                    {
                        Hash = objProperty.Value.GetProperty("hash").ToString(),
                        Title = title[0],
                        Icon = icon,
                        IconColor = iconColor,
                        Tag = Objs.Count
                    };
                    Objs.Add(obj);
                }
            }
            catch (Exception ex)
            {
                ExMessage = ex.Message;
                Objs = null;
                throw;
            }
        }

        private async void oK_Click(object sender, RoutedEventArgs e)
        {
            loading.IsActive = true;
            oK.IsEnabled = false;
            int[] ints = new int[content.SelectedItems.Count];
            int count = -1;
            if (content.SelectedItems.Count == 0)
            {
                ContentDialogHelper.ShowTipDialog("请选择要提取的资源。");
            }
            else
            {
                foreach (var item in content.SelectedItems)
                {
                    if (item is GridViewItem gridViewItem)
                    {
                        var panel = FindChild<RelativePanel>(gridViewItem);
                        if (panel != null)
                        {
                            count += 1;
                            ints[count] = (int)panel.Tag;
                        }
                    }
                }
            }
            List<MCREObj> targetObjs = new List<MCREObj>();
            foreach (var item in Objs)
            {
                foreach (var item1 in ints)
                {
                    if (item.Tag == item1)
                    {
                        targetObjs.Add(item);
                    }
                }
            }
            StorageFolder folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken");
            StorageFolder objsFolder = await StorageFolder.GetFolderFromPathAsync(Path.Combine(folder.Path, "assets", "objects"));
            StorageFolder targetFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("TargetFolderToken");
            foreach (var item in targetObjs)
            {
                foreach (var item1 in await objsFolder.GetFoldersAsync())
                {
                    foreach (var item2 in await item1.GetFilesAsync())
                    {
                        if (item2.Name == item.Hash)
                        {
                            string extension = string.Empty;
                            switch (item.Icon)
                            {
                                case "\uEB9F":
                                    if (item.Title == "icons/minecraft" || item.Title == "icons/snapshot/minecraft")
                                    {
                                        extension = ".icns";
                                    }
                                    else
                                    {
                                        extension = ".png";
                                    }
                                    break;
                                case "\uE8D6":
                                    extension = ".ogg";
                                    break;
                                case "\uF012":
                                    extension = ".zip";
                                    break;
                                case "\uE8A5":
                                    if (item.Title == "pack")
                                    {
                                        extension = ".mcmeta";
                                    }
                                    else if (item.Title == "minecraft/font/include/unifont" || item.Title.Split('/')[1] == "lang" || item.Title == "minecraft/sounds")
                                    {
                                        extension = ".json";
                                    }
                                    break;
                                default:
                                    break;
                            }
                            string[] temp = item.Title.Split("/");
                            string fileName = string.Join('-', temp) + extension;
                            await item2.CopyAsync(targetFolder, fileName);

                        }
                    }
                }
            }
            loading.IsActive = false;
            oK.IsEnabled = true;
            ContentDialogHelper.ShowTipDialog("提取完毕。");
        }

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
    }
}
