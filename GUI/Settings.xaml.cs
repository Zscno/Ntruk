using Ntruk.API;
using System;
using System.IO;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Ntruk.GUI
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
        }

        private async void ChangeFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFolder formerFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken");
                if (await PickerHelper.UsePickerGetSingleFolder("MinecraftFolderToken") != null)
                {
                    StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(Path.Combine((await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken")).Path, "assets", "indexes"));
                    string[] versionsFile = await MinecraftHelper.GetAllVersions(folder);
                    StorageFolder versionsFolder = await StorageFolder.GetFolderFromPathAsync(Path.Combine((await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken")).Path, "versions"));
                    IReadOnlyList<StorageFolder> folders = await versionsFolder.GetFoldersAsync();
                    string[] versions = new string[folders.Count];
                    int count = 0;
                    foreach (var item in folders)
                    {
                        foreach (var item1 in versionsFile)
                        {
                            if (item.Name.Split('-')[0] == item1)
                            {
                                versions[count] = item1;
                                count++;//没用
                            }
                        }
                    }
                    currentVersion.PlaceholderText = "请重新选择版本...";
                    currentFolder.Text = (await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken")).Path;
                    currentVersion.ItemsSource = versions;
                }
                else
                {
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("MinecraftFolderToken", formerFolder);
                    currentVersion.PlaceholderText = "请重新选择Minecraft文件夹...";
                }
            }
            catch (Exception)
            {
                currentTarget.PlaceholderText = "请重新选择文件夹...";
                ContentDialogHelper.ShowTipDialog("目前选择的Minecarft文件夹不起作用，请再次尝试。");
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                currentFolder.IsReadOnly = true;
                currentTarget.IsReadOnly = true;
                currentFolder.Text = (await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken")).Path;
                currentTarget.Text = (await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("TargetFolderToken")).Path;
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(Path.Combine((await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken")).Path, "assets", "indexes"));
                string[] versions = await MinecraftHelper.GetAllVersions(folder);
                string version = string.Empty;
                foreach (var item in versions)
                {
                    if (item == IniHelper.ReadIni("Minecraft", "Version", MainPage.ConfigDataPath))
                    {
                        version = item;
                        break;
                    }
                }
                currentVersion.ItemsSource = versions;
                currentVersion.SelectedItem = version;
            }
            catch (Exception)
            {
                currentFolder.PlaceholderText = "请重新选择文件夹...";
                ContentDialogHelper.ShowTipDialog("目前选择的Minecarft文件夹不起作用，请再次尝试。");
            }
        }

        private void CurrentVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IniHelper.WriteIni("Minecraft", "Version", currentVersion.SelectionBoxItem.ToString(), MainPage.ConfigDataPath);
        }

        private async void ChangeTarget_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (await PickerHelper.UsePickerGetSingleFolder("TargetFolderToken") != null)
                {
                    currentTarget.Text = (await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("TargetFolderToken")).Path;
                }
            }
            catch (Exception)
            {
                currentTarget.PlaceholderText = "请重新选择文件夹...";
                ContentDialogHelper.ShowTipDialog("目前选择的目标文件夹可能无法使用，请再次尝试。");
            }
        }
    }
}
