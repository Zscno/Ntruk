using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Ntruk
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class InitPage : Page
    {
        public InitPage()
        {
            this.InitializeComponent();
            step.Navigate(typeof(PickFolder));
            back.IsEnabled = false;
            count = 0;
        }

        public static int count;

        private void next_Click(object sender, RoutedEventArgs e)
        {
            switch (count)
            {
                case 0:
                    if (PickFolder.Folder == null || PickFolder.Folder == string.Empty)
                    {
                        ContentDialogHelper.ShowTipDialog("请选择一个Minecraft文件夹。");
                        break;
                    }
                    step.Navigate(typeof(PickVersion));
                    back.IsEnabled = true;
                    title.Text = "初始化（2/3）";
                    count = 1;
                    break;
                case 1:
                    if (PickVersion.Version == null || PickVersion.Version == string.Empty)
                    {
                        ContentDialogHelper.ShowTipDialog("请选择一个Minecraft版本。");
                        break;
                    }
                    step.Navigate(typeof(PickTarget));
                    title.Text = "初始化（3/3）";
                    count = 2;
                    break;
                case 2:
                    if (PickTarget.Folder == null || PickTarget.Folder == string.Empty)
                    {
                        ContentDialogHelper.ShowTipDialog("请选择一个目标文件夹。");
                        break;
                    }
                    IniHelper.WriteIni("Minecraft", "Folder", PickFolder.Folder, MainPage.ConfigDataPath);
                    IniHelper.WriteIni("Minecraft", "Version", PickVersion.Version, MainPage.ConfigDataPath);
                    PageHelper.NavigateOneselfTo(this, typeof(UserMainPage));
                    count = 3;
                    break;
                default:
                    break;
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            switch (count)
            {
                case 1:
                    step.Navigate(typeof(PickFolder));
                    back.IsEnabled = false;
                    title.Text = "初始化（1/3）";
                    count = 0;
                    PickFolder.Folder = string.Empty;
                    break;
                case 2:
                    step.Navigate(typeof(PickVersion));
                    title.Text = "初始化（2/3）";
                    count = 1;
                    PickVersion.Version = string.Empty;
                    break;
                default:
                    break;
            }
        }
    }
}
