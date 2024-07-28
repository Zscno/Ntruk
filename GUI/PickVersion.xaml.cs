using Ntruk.API;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Ntruk.GUI
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PickVersion : Page
    {
        public PickVersion()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 指示用户是否选择了任意一个版本。
        /// </summary>
        public static bool DoesTheUserChoose { get; private set; } = false;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            pickBox.ItemsSource = PickFolder.Versions;
            DoesTheUserChoose = false;
        }

        private void PickBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IniHelper.WriteIni("Minecraft", "Version", pickBox.SelectedItem.ToString(), MainPage.ConfigDataPath);
            DoesTheUserChoose = true;
        }
    }
}
