using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Ntruk.GUI
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class UserMainPage : Page
    {
        public UserMainPage()
        {
            this.InitializeComponent();
        }

        private void MainPanel_SelectionChanged(muxc.NavigationView sender, muxc.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem == home)
            {
                contentFrame.Navigate(typeof(Home));
            }
            else if (args.SelectedItem == mCRE)
            {
                contentFrame.Navigate(typeof(MCRE));
            }
            else if (args.SelectedItem == about)
            {
                contentFrame.Navigate(typeof(About));
            }
            else if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(Settings));
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            switch (e.Content.ToString())
            {
                case "Ntruk.GUI.Home":
                    mainPanel.SelectedItem = home;
                    break;
                case "Ntruk.GUI.MCRE":
                    mainPanel.SelectedItem = mCRE;
                    break;
                case "Ntruk.GUI.About":
                    mainPanel.SelectedItem = about;
                    break;
                case "Ntruk.GUI.Settings":
                    mainPanel.SelectedItem = mainPanel.SettingsItem;
                    break;
                default:
                    break;
            }
        }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            mainPanel.SelectedItem = home;
            await LogSystem.WriteLog(LogLevel.Info, this, "“用户主界面”加载完成。");
        }
    }
}
