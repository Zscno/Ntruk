using Ntruk.API;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Ntruk.GUI
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MCRE : Page
    {
        /// <summary>
        /// MCRE界面中的文件对象数组。
        /// </summary>
        internal static List<MinecraftResourceFile> MCFileObjs;

        /// <summary>
        /// MCRE界面中的文件对象数组树形逻辑结构根节点。
        /// </summary>
        internal static StringTreeNode Root;

        /// <summary>
        /// MCRE界面中将要导出的文件集合。
        /// </summary>
        internal static List<MinecraftResourceFile> ReadyData;

        /// <summary>
        /// MCRE界面中返回键所使用。
        /// </summary>
        internal static List<List<MinecraftResourceFile>> backPageFiles;

        /// <summary>
        /// MCRE的初始界面的参数。
        /// </summary>
        internal static List<MinecraftResourceFile> homePage;

        public MCRE()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(typeof(MCREFilePage));
            backPageFiles = new List<List<MinecraftResourceFile>>();
            ReadyData = new List<MinecraftResourceFile>();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (backPageFiles.Count == 0)
            {
                return;
            }
            mainFrame.Navigate(typeof(MCREFilePage), backPageFiles[backPageFiles.Count - 1]);
            backPageFiles.Remove(backPageFiles[backPageFiles.Count - 1]);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(typeof(MCREFilePage), homePage);
        }
    }
}