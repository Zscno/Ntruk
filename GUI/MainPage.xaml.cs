using Ntruk.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Ntruk.GUI
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 配置数据文件路径。
        /// </summary>
        public static string ConfigDataPath { get; set; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (LogSystem.LogFile == null)
            {
                LogSystem.LogFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            }
            await LogSystem.WriteLog(LogLevel.Info, this, "初始化应用...");
            await LogSystem.WriteLog(LogLevel.Info, this, "加载配置文件...");
            ConfigDataPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "config.ini");
            await LogSystem.WriteLog(LogLevel.Info, this, "加载初始界面...");
            if (!File.Exists(ConfigDataPath))
            {
                await ApplicationData.Current.LocalFolder.CreateFileAsync("config.ini");
                contentFrame.Navigate(typeof(InitPage));
            }
            else if (new List<string>() { "Folder", "Version", "Target" }.Any(key => IniHelper.ReadIni("Minecraft", key, ConfigDataPath) == string.Empty))
            {
                await FileIO.WriteTextAsync(await StorageFile.GetFileFromPathAsync(ConfigDataPath), "");
                contentFrame.Navigate(typeof(InitPage));
            }
            else
            {
                contentFrame.Navigate(typeof(UserMainPage));
            }
        }
    }
}
