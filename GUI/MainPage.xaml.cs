using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

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
        /// 本地数据文件夹路径。
        /// </summary>
        public static string LocalDataFolder { get; set; }

        /// <summary>
        /// 配置数据文件路径。
        /// </summary>
        public static string ConfigDataPath { get; set; }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (LogSystem.LogFile == null)
            {
                LogSystem.LogFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            }
            await LogSystem.WriteLog(LogLevel.Info, this, "初始化应用...");
            await LogSystem.WriteLog(LogLevel.Info, this, "加载MCRE配置文件...");
            LocalDataFolder = ApplicationData.Current.LocalFolder.Path;
            ConfigDataPath = Path.Combine(LocalDataFolder, "config.ini");
            await LogSystem.WriteLog(LogLevel.Info, this, "加载初始界面...");
            if (!File.Exists(ConfigDataPath))
            {
                contentFrame.Navigate(typeof(InitPage));
            }
            else
            {
                contentFrame.Navigate(typeof(UserMainPage));
            }
        }
    }
}
