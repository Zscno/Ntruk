using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Ntruk.GUI
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class About : Page
    {
        public About()
        {
            this.InitializeComponent();
            ApplicationTheme theme = Application.Current.RequestedTheme;
            if (theme == ApplicationTheme.Light)
            {
                iconImage.Source = new BitmapImage(new Uri("ms-appx:///Images/Ntruk-Light-400x400.png"));
            }
            else if (theme == ApplicationTheme.Dark)
            {
                iconImage.Source = new BitmapImage(new Uri("ms-appx:///Images/Ntruk-Dark-400x400.png"));
            }
            else
            {
                iconImage.Source = new BitmapImage(new Uri("ms-appx:///Images/Ntruk-Dark-400x400.png"));
            }
            verisonText.Text = $"v.{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}";
        }
    }
}
