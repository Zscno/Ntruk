using Ntruk.API;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Ntruk.GUI
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PickTarget : Page
    {
        public PickTarget()
        {
            this.InitializeComponent();
            IsFolderOperable = false;
        }

        /// <summary>
        /// 指示该界面获取的Folder是否存在并可访问。
        /// </summary>
        public static bool IsFolderOperable { get; private set; } = false;

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            openButton.IsEnabled = false;
            StorageFolder targetFolder = await PickerHelper.GetSingleFolderByPicker();
            if (targetFolder != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("TargetFolderToken", targetFolder);
                IniHelper.WriteIni("Minecraft", "Target", targetFolder.Path, MainPage.ConfigDataPath);
                inputBox.Text = targetFolder.Path;
                IsFolderOperable = true;
            }
            else
            {
                await LogSystem.WriteLog(LogLevel.Warning, new PickTarget(), "用户没有选择文件夹。");
                IsFolderOperable = false;
            }
            openButton.IsEnabled = true;
        }
    }
}
