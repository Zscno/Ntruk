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
                PageHelper.NavigateOneselfTo(this, typeof(InitPage));
                ContentDialogHelper.ShowTipDialog("请重新选择一个Minecraft文件夹。\n目前选择的文件夹不起作用。");
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            currentFolder.IsReadOnly = true;
            currentFolder.Text = (await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken")).Path;
            currentVersion.PlaceholderText = IniHelper.ReadIni("Minecraft", "Version", MainPage.ConfigDataPath);
            try
            {
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(Path.Combine((await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken")).Path, "assets", "indexes"));
                string[] versions = await MinecraftHelper.GetAllVersions(folder);
                currentVersion.ItemsSource = versions;
            }
            catch (Exception)
            {
                PageHelper.NavigateOneselfTo(this, typeof(InitPage));
                ContentDialogHelper.ShowTipDialog("请重新选择一个Minecraft文件夹。\n目前选择的文件夹不起作用。");
            }
        }

        private void CurrentVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IniHelper.WriteIni("Minecraft", "Version", currentVersion.SelectionBoxItem.ToString(), MainPage.ConfigDataPath);
        }
    }
}
