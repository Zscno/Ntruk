using System;
using Windows.UI.Xaml.Controls;

namespace Ntruk
{
    /// <summary>
    /// 帮助简化<c>ContentDialog</c>的操作。
    /// </summary>
    internal static class ContentDialogHelper
    {
        /// <summary>
        /// 显示一个提示对话框。
        /// </summary>
        /// <param name="content">对话框的内容。<para>可传入字符串对象或一个容器。</para></param>
        public static void ShowTipDialog(object content)
        {
            ShowDialog("提示", content);
        }

        /// <summary>
        /// 显示一个错误对话框。
        /// </summary>
        /// <param name="content">对话框的内容。<para>可传入字符串对象或一个容器。</para></param>
        public static void ShowErrorDialog(object content)
        {
            ShowDialog("错误", content);
        }

        /// <summary>
        /// 显示一个对话框。
        /// </summary>
        /// <param name="title">对话框的标题。</param>
        /// <param name="content">对话框的内容。<para>可传入字符串对象或一个容器。</para></param>
        public static async void ShowDialog(string title, object content)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                PrimaryButtonText = "确定",
                DefaultButton = ContentDialogButton.Primary,
            };
            await dialog.ShowAsync();
        }
    }
}
