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
            mainPanel.SelectedItem = home;
        }

        private void NavigationView_SelectionChanged(muxc.NavigationView sender, muxc.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(Settings));
            }
            else if (args.SelectedItem == home)
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
        }

        private void content_Navigated(object sender, NavigationEventArgs e)
        {
            switch (e.Content.ToString())
            {
                case "Ntruk.Home":
                    mainPanel.SelectedItem = home;
                    break;
                case "Ntruk.MCRE":
                    mainPanel.SelectedItem = mCRE;
                    break;
                case "Ntruk.About":
                    mainPanel.SelectedItem = about;
                    break;
                default:
                    mainPanel.SelectedItem = mainPanel.SettingsItem;
                    break;
            }
        }
    }
}
