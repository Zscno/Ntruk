using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Ntruk.API;

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
            currentFolder.Text += IniHelper.ReadIni("minecraft", "Folder", MainPage.ConfigDataPath);
            currentVersion.Text += IniHelper.ReadIni("minecraft", "Version", MainPage.ConfigDataPath);
        }

        private void changeFolder_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            frame.Navigate(typeof(PickFolder));
        }

        private void changeVersion_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            frame.Navigate(typeof(PickVersion));
        }
    }
}
