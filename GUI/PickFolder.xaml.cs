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
    public sealed partial class PickFolder : Page
    {
        public PickFolder()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 指示该界面获取的文件夹是否存在并可访问。
        /// </summary>
        public static bool IsFolderOperable { get; private set; } = false;

        /// <summary>
        /// 该界面获取的文件夹中的所有Minecraft版本。
        /// </summary>
        public static string[] Versions { get; private set; }

        /// <summary>
        /// 该界面获取的文件夹。
        /// </summary>
        public static string Folder { get; private set; }

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            openButton.IsEnabled = false;
            await LogSystem.WriteLog(LogLevel.Info, this, "用户尝试选择一个Minecraft文件夹...");
            StorageFolder minecraftFolder = await PickerHelper.GetSingleFolderByPicker();
            if (minecraftFolder != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("MinecraftFolderToken", minecraftFolder);

                StorageFolder indexesFolder;
                try
                {
                    indexesFolder = await StorageFolder.GetFolderFromPathAsync(Path.Combine(minecraftFolder.Path, "assets", "indexes"));
                }
                catch (Exception ex)
                {
                    await LogSystem.WriteLog(LogLevel.Warning, this, $"获取文件夹{Path.Combine(minecraftFolder.Path, "assets", "indexes")}时触发异常：{ex.Message}。");
                    await ContentDialogHelper.ShowTipDialog("我们无法访问所选Minecraft文件夹。\n请您更换文件夹再次尝试。");
                    StorageApplicationPermissions.FutureAccessList.Remove("MinecraftFolderToken");
                    openButton.IsEnabled = true;
                    IsFolderOperable = false;
                    return;
                }

                Versions = await MinecraftHelper.GetAllVersions(indexesFolder);
                if (Versions.Length == 0)
                {
                    await LogSystem.WriteLog(LogLevel.Warning, this, $"{indexesFolder.Path}中没有资源索引文件（.json）。");
                    await ContentDialogHelper.ShowTipDialog("我们没有在所选Minecraft文件夹中找到索引文件。它们可能不存在。\n请您更换文件夹或在其中下载一个Minecraft版本以再次尝试。");
                    StorageApplicationPermissions.FutureAccessList.Remove("MinecraftFolderToken");
                    openButton.IsEnabled = true;
                    IsFolderOperable = false;
                    return;
                }

                inputBox.Text = minecraftFolder.Path;
                IsFolderOperable = true;
                IniHelper.WriteIni("Minecraft", "Folder", minecraftFolder.Path, MainPage.ConfigDataPath);
            }
            else
            {
                await LogSystem.WriteLog(LogLevel.Warning, this, "用户没有选择文件夹。");
                IsFolderOperable = false;
            }
            openButton.IsEnabled = true;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StorageFolder minecraftFolder = null;
            string folderPath = IniHelper.ReadIni("Minecraft", "Folder", MainPage.ConfigDataPath);
            if (folderPath != string.Empty)
            {
                minecraftFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken");
                if (folderPath !=  minecraftFolder.Path)
                {
                    await ContentDialogHelper.ShowErrorDialog("我们在访问Minecraft文件夹时出现了一些问题。");
                    await LogSystem.WriteLog(LogLevel.Warning, this, $"配置文件的值{folderPath}与程序可访问列表（Token = MinecraftFolderToken）的值{minecraftFolder.Path}不匹配。");
                }
            }
            if (minecraftFolder != null)
            {
                inputBox.Text = minecraftFolder.Path;
                IsFolderOperable = true;
            }
            else
            {
                IsFolderOperable = false;
            }
        }
    }
}
