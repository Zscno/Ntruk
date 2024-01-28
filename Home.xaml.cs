using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Ntruk
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Home : Page
    {
        public Home()
        {
            this.InitializeComponent();
            List<HomeObj> homeObjs = new List<HomeObj>();
            HomeObj obj1 = new HomeObj()
            {
                IconLocation = "/Images/MCRE.png",
                Title = "Minecraft资源提取器",
                Description = "一种更快、更准、更直观地获取Minecraft资源地方法。",
            };
            homeObjs.Add(obj1);
            content.ItemsSource = homeObjs;
        }

        private void content_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PageHelper.NavigateOneselfTo(this, typeof(MCRE));
        }
    }
}
