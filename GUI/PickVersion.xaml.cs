using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
                DirectoryInfo directory = new DirectoryInfo(Path.Combine(PickFolder.Folder, "assets", "indexes"));
                FileInfo[] files = directory.GetFiles();
                string[] versions = new string[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    switch (Path.GetFileNameWithoutExtension(files[i].FullName))
                    {
                        case "2":
                            versions[i] = "1.19.3";
                            break;
                        case "3":
                            versions[i] = "1.19.4";
                            break;
                        case "5":
                            versions[i] = "1.20";
                            break;
                        case "8":
                            versions[i] = "1.20.2";
                            break;
                        case "12":
                            versions[i] = "1.20.3";
                            break;
                        default:
                            versions[i] = Path.GetFileNameWithoutExtension(files[i].FullName);
                            break;
                    }

                }
                Array.Sort(versions);
                pick.ItemsSource = versions;
            }
            catch (Exception)
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "提示",
                    PrimaryButtonText = "确定",
                    DefaultButton = ContentDialogButton.Primary,
                    Content = "请在摁下“打开...”后弹出的窗口中选择文件夹或在其中的地址栏中填写路径。",
                };
                await dialog.ShowAsync();
            }
            
        }

        private void pick_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Version = (sender as ComboBox).SelectedItem.ToString();
        }
    }
}
