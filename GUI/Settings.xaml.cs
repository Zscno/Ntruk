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

        private bool useEvent;

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LogSystem.WriteLog(LogLevel.Info, this, "加载“Settings”界面...");
            useEvent = false;
            StorageFolder minecraftFolder;
            StorageFolder indexesFolder;
            minecraftFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken");
            try
            {
                indexesFolder = await StorageFolder.GetFolderFromPathAsync(Path.Combine(minecraftFolder.Path, "assets", "indexes"));
            }
            catch (Exception)
            {
                await LogSystem.WriteLog(LogLevel.Warning, this, $"{Path.Combine(minecraftFolder.Path, "assets", "indexes")}不存在或程序权限不足无法访问。");
                await ContentDialogHelper.ShowTipDialog("我们无法访问所选Minecraft文件夹。它可能不存在或我们无权访问。\n请您更换文件夹再次尝试。");
                StorageApplicationPermissions.FutureAccessList.Remove("MinecraftFolderToken");
                useEvent = true;
                return;
            }
            string[] versions = await MinecraftHelper.GetAllVersions(indexesFolder);
            if (versions.Length == 0)
            {
                await LogSystem.WriteLog(LogLevel.Warning, this, $"{indexesFolder.Path}中没有资源索引文件（.json）或无权访问它们。");
                await ContentDialogHelper.ShowTipDialog("我们没有在所选Minecraft文件夹中找到索引文件。它可能不存在或我们无权访问。\n请您更换文件夹或在其中下载一个Minecraft版本以再次尝试。");
                useEvent = true;
                return;
            }
            currentVersion.ItemsSource = versions;
            int count = 0;
            foreach (var item in versions)
            {
                if (item == IniHelper.ReadIni("Minecraft", "Version", MainPage.ConfigDataPath))
                {
                    break;
                }
                else
                {
                    count++;
                }
            }
            if (count >= versions.Length)
            {
                await LogSystem.WriteLog(LogLevel.Warning, this, $"{minecraftFolder.Path}中没有用户上次选择的版本{IniHelper.ReadIni("Minecraft", "Version", MainPage.ConfigDataPath)}（数组索引超出范围）。");
                await ContentDialogHelper.ShowTipDialog("在Minecraft文件夹中我们找不到您选择的版本。\n请您重新选择版本或者文件夹。");
                currentVersion.PlaceholderText = "重新选择版本...";
                IniHelper.WriteIni("Minecraft", "Version", string.Empty, MainPage.ConfigDataPath);
                useEvent = true;
                goto GetTargetFolder;
            }
            currentVersion.SelectedItem = versions[count];
            currentFolder.Text = minecraftFolder.Path;

        GetTargetFolder:
            StorageFolder targetFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("TargetFolderToken");
            if (!Directory.Exists(targetFolder.Path))
            {
                await LogSystem.WriteLog(LogLevel.Warning, this, $"{targetFolder.Path}不存在或程序权限不足无法访问。");
                await ContentDialogHelper.ShowTipDialog("我们无法访问所选目标文件夹。它可能不存在或我们无权访问。\n请您更换文件夹再次尝试。");
                StorageApplicationPermissions.FutureAccessList.Remove("TargetFolderToken");
                currentTarget.PlaceholderText = "重新选择文件夹...";
                useEvent = true;
                return;
            }
            currentTarget.Text = targetFolder.Path;
            useEvent = true;

            await LogSystem.WriteLog(LogLevel.Info, this, "“Settings”界面加载完成。");
        }

        private async void ChangeFolder_Click(object sender, RoutedEventArgs e)
        {
            await LogSystem.WriteLog(LogLevel.Info, this, "尝试修改Minecraft文件夹...");
            useEvent = false;
            changeFolder.IsEnabled = false;
            StorageFolder minecraftFolder = await PickerHelper.GetSingleFolderByPicker();
            if (minecraftFolder != null)
            {
                StorageFolder indexesFolder;
                try
                {
                    indexesFolder = await StorageFolder.GetFolderFromPathAsync(Path.Combine(minecraftFolder.Path, "assets", "indexes"));
                }
                catch (Exception)
                {
                    await LogSystem.WriteLog(LogLevel.Warning, this, $"{Path.Combine(minecraftFolder.Path, "assets", "indexes")}不存在或程序权限不足无法访问。");
                    await ContentDialogHelper.ShowTipDialog("我们无法访问所选Minecraft文件夹。它可能不存在或我们无权访问。\n请您更换文件夹再次尝试。");
                    changeFolder.IsEnabled = true;
                    useEvent = true;
                    return;
                }
                string[] versions = await MinecraftHelper.GetAllVersions(indexesFolder);
                if (versions.Length == 0)
                {
                    await LogSystem.WriteLog(LogLevel.Warning, this, $"{indexesFolder.Path}中没有资源索引文件（.json）或无权访问它们。");
                    await ContentDialogHelper.ShowTipDialog("我们没有在所选Minecraft文件夹中找到索引文件。它可能不存在或我们无权访问。\n请您更换文件夹或在其中下载一个Minecraft版本以再次尝试。");
                    changeFolder.IsEnabled = true;
                    useEvent = true;
                    return;
                }
                IniHelper.WriteIni("Minecraft", "Folder", minecraftFolder.Path, MainPage.ConfigDataPath);
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("MinecraftFolderToken", minecraftFolder);
                currentFolder.Text = minecraftFolder.Path;
                currentVersion.PlaceholderText = "请重新选择版本...";
                currentVersion.ItemsSource = versions;
                changeFolder.IsEnabled = true;
                useEvent = true;
                await LogSystem.WriteLog(LogLevel.Info, this, "修改Minecraft文件夹成功。");
            }
            else
            {
                changeFolder.IsEnabled = true;
                useEvent = true;
                await LogSystem.WriteLog(LogLevel.Info, this, "用户未选择文件夹，修改Minecraft文件夹失败。");
            }
        }

        private async void CurrentVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!useEvent)
            {
                return;
            }
            await LogSystem.WriteLog(LogLevel.Info, this, "尝试修改Minecraft版本...");
            if (currentVersion.SelectedItem != null)
            {
                IniHelper.WriteIni("Minecraft", "Version", currentVersion.SelectedItem.ToString(), MainPage.ConfigDataPath);
                await LogSystem.WriteLog(LogLevel.Info, this, "修改Minecraft版本成功。");
            }
            else
            {
                await LogSystem.WriteLog(LogLevel.Info, this, "用户未选择Minecraft版本，修改Minecraft文件夹失败。");
            }
        }

        private async void ChangeTarget_Click(object sender, RoutedEventArgs e)
        {
            await LogSystem.WriteLog(LogLevel.Info, this, "尝试修改目标文件夹...");
            changeTarget.IsEnabled = false;
            StorageFolder targetFolder = await PickerHelper.GetSingleFolderByPicker();
            if (targetFolder != null)
            {
                changeTarget.IsEnabled = true;
                currentTarget.Text = targetFolder.Path;
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("TargetFolderToken", targetFolder);
                IniHelper.WriteIni("Minecraft", "Target", targetFolder.Path, MainPage.ConfigDataPath);
                await LogSystem.WriteLog(LogLevel.Info, this, "修改目标文件夹成功。");
            }
            else
            {
                changeTarget.IsEnabled = true;
                await LogSystem.WriteLog(LogLevel.Info, this, "用户未选择文件夹，修改目标文件夹失败。");
            }
        }

    }
}