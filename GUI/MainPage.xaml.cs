using System.IO;
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
            LocalDataFolder = ApplicationData.Current.LocalFolder.Path;
            ConfigDataPath = Path.Combine(LocalDataFolder, "config.ini");
            if (!File.Exists(ConfigDataPath))
            {
                backPage.Navigate(typeof(InitPage));
            }
            else
            {
                backPage.Navigate(typeof(UserMainPage));
            }
        }

        /// <summary>
        /// 本地数据文件夹路径。
        /// </summary>
        public static string LocalDataFolder;

        /// <summary>
        /// 配置数据文件路径。
        /// </summary>
        public static string ConfigDataPath;
    }
}
