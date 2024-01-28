using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Ntruk
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
                content.Navigate(typeof(Settings));
            }
            else if (args.SelectedItem == home)
            {
                content.Navigate(typeof(Home));
            }
            else if (args.SelectedItem == mCRE)
            {
                content.Navigate(typeof(MCRE));
            }
            else if (args.SelectedItem == about)
            {
                content.Navigate(typeof(About));
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
