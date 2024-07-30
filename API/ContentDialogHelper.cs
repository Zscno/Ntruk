using Ntruk.GUI;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Ntruk.API
{
    internal static class ContentDialogHelper
    {
        /// <summary>
        /// 显示一个提示对话框。
        /// </summary>
        /// <param name="text">对话框中的文字。</param>
        public static async Task ShowTipDialog(string text)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "提示",
                PrimaryButtonText = "确定",
                DefaultButton = ContentDialogButton.Primary,
                Content = new InfoDialog(text),
            };
            await dialog.ShowAsync();
        }

        /// <summary>
        /// 显示一个错误对话框并让用户选择上传日志或退出应用。
        /// </summary>
        /// <param name="message">错误信息。</param>
        public static async Task ShowErrorDialog(string message)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "错误",
                PrimaryButtonText = "退出并打开日志所在位置",
                SecondaryButtonText = "退出",
                DefaultButton = ContentDialogButton.Primary,
                Content = new ErrorDialog(message + "请将日志反馈到Github上以解决问题。\n（通过“关于”界面上的“Github”超链接）"),
            };
            ContentDialogResult result = await dialog.ShowAsync();
            switch (result)
            {
                case ContentDialogResult.None:
                case ContentDialogResult.Primary:
                    FolderLauncherOptions folderLauncherOptions = new FolderLauncherOptions();
                    folderLauncherOptions.ItemsToSelect.Add(LogSystem.LogFile);
                    await Launcher.LaunchFolderAsync(ApplicationData.Current.LocalFolder, folderLauncherOptions);
                    CoreApplication.Exit();
                    break;
                case ContentDialogResult.Secondary:
                    CoreApplication.Exit();
                    break;
                default:
                    break;
            }
        }
    }
}
