using Ntruk.API;
using System;
using System.IO;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
                await PickerHelper.UsePickerGetSingleFolder("MinecraftFolderToken");
                currentFolder.Text = (await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken")).Path;
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(Path.Combine((await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken")).Path, "assets", "indexes"));
                string[] versions = await MinecraftHelper.GetAllVersions(folder);
                currentVersion.ItemsSource = versions;
                currentVersion.PlaceholderText = "请重新选择版本...";
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
                await PickerHelper.UsePickerGetSingleFolder("TargetFolderToken");
                currentTarget.Text = (await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("TargetFolderToken")).Path;
            }
            catch (Exception)
            {
                currentTarget.PlaceholderText = "请重新选择文件夹...";
                ContentDialogHelper.ShowTipDialog("目前选择的目标文件夹可能无法使用，请再次尝试。");
            }
        }
    }
}
