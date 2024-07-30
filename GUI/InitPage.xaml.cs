using Ntruk.API;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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
        }

        public static int Count;

        private async void NextBotton_Click(object sender, RoutedEventArgs e)
        {
            switch (Count)
            {
                case 0:
                    if (!PickFolder.IsFolderOperable)
                    {
                        await ContentDialogHelper.ShowTipDialog("请点击“打开...”按钮并选择一个Minecraft文件夹。");
                    }
                    else
                    {
                        await LogSystem.WriteLog(LogLevel.Info, new PickFolder(), "应用初始化第一步完成。");
                        contentFrame.Navigate(typeof(PickVersion));
                        backButton.IsEnabled = true;
                        titleText.Text = "初始化（2/3）";
                        Count = 1;
                    }
                    break;
                case 1:
                    if (!PickVersion.DoesTheUserChoose)
                    {
                        await LogSystem.WriteLog(LogLevel.Warning, new PickVersion(), "用户没有选择Minecraft版本。");
                        await ContentDialogHelper.ShowTipDialog("请选择一个Minecraft版本。");
                    }
                    else
                    {
                        await LogSystem.WriteLog(LogLevel.Info, new PickVersion(), "应用初始化第二步完成。");
                        contentFrame.Navigate(typeof(PickTarget));
                        titleText.Text = "初始化（3/3）";
                        nextButton.Content = "完成";
                        Count = 2;
                    }
                    break;
                case 2:
                    if (!PickTarget.IsFolderOperable)
                    {
                        await ContentDialogHelper.ShowTipDialog("请点击“打开...”按钮并选择一个文件夹。");
                    }
                    else
                    {
                        await LogSystem.WriteLog(LogLevel.Info, new PickTarget(), "应用初始化第三步完成。");
                        await LogSystem.WriteLog(LogLevel.Info, this, "应用初始化已全部完成。");
                        (VisualTreeHelper.GetParent(this) as Frame).Navigate(typeof(UserMainPage));
                        Count = 3;
                    }
                    break;
                default:
                    break;
            }
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Count)
            {
                case 1:
                    contentFrame.Navigate(typeof(PickFolder));
                    backButton.IsEnabled = false;
                    titleText.Text = "初始化（1/3）";
                    Count = 0;
                    await LogSystem.WriteLog(LogLevel.Info, new PickFolder(), "用户已回退并重置至应用初始化第一步。");
                    break;
                case 2:
                    contentFrame.Navigate(typeof(PickVersion));
                    titleText.Text = "初始化（2/3）";
                    Count = 1;
                    await LogSystem.WriteLog(LogLevel.Info, new PickVersion(), "用户已回退并重置至应用初始化第二步。");
                    break;
                default:
                    break;
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(PickFolder));
            Count = 0;
            await LogSystem.WriteLog(LogLevel.Info, this, "“初始化应用界面”加载完成。");
        }
    }
}
