using Ntruk.API;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Ntruk.GUI
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class InitPage : Page
    {
        public InitPage()
        {
            this.InitializeComponent();
            contentFrame.Navigate(typeof(PickFolder));
            backButton.IsEnabled = false;
            Count = 0;
        }

        public static int Count;

        private void NextBotton_Click(object sender, RoutedEventArgs e)
        {
            switch (Count)
            {
                case 0:
                    if (PickFolder.Folder == null || PickFolder.Folder == string.Empty)
                    {
                        ContentDialogHelper.ShowTipDialog("请选择一个Minecraft文件夹。");
                        break;
                    }
                    contentFrame.Navigate(typeof(PickVersion));
                    backButton.IsEnabled = true;
                    titleText.Text = "初始化（2/3）";
                    Count = 1;
                    break;
                case 1:
                    if (PickVersion.Version == null || PickVersion.Version == string.Empty)
                    {
                        ContentDialogHelper.ShowTipDialog("请选择一个Minecraft版本。");
                        break;
                    }
                    contentFrame.Navigate(typeof(PickTarget));
                    titleText.Text = "初始化（3/3）";
                    Count = 2;
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
                    Count = 3;
                    break;
                default:
                    break;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Count)
            {
                case 1:
                    contentFrame.Navigate(typeof(PickFolder));
                    backButton.IsEnabled = false;
                    titleText.Text = "初始化（1/3）";
                    Count = 0;
                    PickFolder.Folder = string.Empty;
                    break;
                case 2:
                    contentFrame.Navigate(typeof(PickVersion));
                    titleText.Text = "初始化（2/3）";
                    Count = 1;
                    PickVersion.Version = string.Empty;
                    break;
                default:
                    break;
            }
        }
    }
}
