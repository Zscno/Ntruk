using Ntruk.API;
using System;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using System.Collections.Generic;

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

        public static string Version;

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(Path.Combine(PickFolder.Folder, "assets", "indexes"));
                IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
                string[] versions = new string[files.Count];
                for (int i = 0; i < files.Count; i++)
                {
                    MinecraftHelper.GetVersion(Path.GetFileNameWithoutExtension(files[i].Path));
                }
                Array.Sort(versions);
                pickBox.ItemsSource = versions;
            }
            catch (Exception)
            {
                PageHelper.NavigateOneselfTo(this, typeof(PickFolder));
                ContentDialogHelper.ShowTipDialog("请重新选择一个Minecraft文件夹。\n目前选择的文件夹不起作用。");
            }
        }

        private void PickBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Version = (sender as ComboBox).SelectedItem.ToString();
        }
    }
}
