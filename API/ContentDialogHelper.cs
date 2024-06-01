using Ntruk.GUI;
using System;
using Windows.UI.Xaml.Controls;

namespace Ntruk.API
{
    internal static class ContentDialogHelper
    {
        /// <summary>
        /// 显示一个提示对话框。
        /// </summary>
        /// <param name="text">对话框中的文字。</param>
        public static async void ShowTipDialog(string text)
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
        /// 显示一个错误对话框。
        /// </summary>
        /// <param name="text">对话框中的文字。</param>
        public static async void ShowErrorDialog(string text)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "错误",
                PrimaryButtonText = "确定",
                DefaultButton = ContentDialogButton.Primary,
                Content = new ErrorDialog(text),
            };
            await dialog.ShowAsync();
        }
    }
}
