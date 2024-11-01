using Ntruk.API;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Ntruk.GUI
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Home : Page
    {
        public Home()
        {
            this.InitializeComponent();
            List<Function> homeObjs = new List<Function>();
            Function obj1 = new Function()
            {
                IconLocation = "/Images/MCRE.png",
                Title = "Minecraft资源提取器",
                Description = "一种更快、更准、更直观地获取Minecraft资源地方法。",
            };
            homeObjs.Add(obj1);
            contentView.ItemsSource = homeObjs;
        }

        private void ContentView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame parent = Parent as Frame;
            parent.Navigate(typeof(MCRE));
        }
    }
}
