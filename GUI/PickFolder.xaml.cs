﻿using System;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Ntruk.GUI
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PickFolder : Page
    {
        public PickFolder()
        {
            this.InitializeComponent();
        }

        public static string Folder;

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker picker = new FolderPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.Desktop
            };
            picker.FileTypeFilter.Add("*");
            StorageFolder folder = await picker.PickSingleFolderAsync();
            if (folder != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("MinecraftFolderToken", folder);
                inputBox.Text = folder.Path;
            }
        }

        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Folder = inputBox.Text;
        }
    }
}