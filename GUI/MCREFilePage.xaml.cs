using Ntruk.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Ntruk.GUI
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MCREFilePage : Page
    {
        public MCREFilePage()
        {
            this.InitializeComponent();
        }

        private async void MCREFilePage_Loaded(object sender, RoutedEventArgs e)
        {
            loadingAnimation.IsActive = true;
            await LogSystem.WriteLog(LogLevel.Info, this, "加载MCREFilePage...");
            try
            {
                numberText.Text = "已选择" + MCRE.ReadyData.Count + "个项目。";
                if (MCRE.MCFileObjs == null)
                {
                    MCRE.MCFileObjs = await GetMCREData();
                }
                if (CurrentData == null)
                {
                    CurrentData = new List<MCREObj>();
                    foreach (var item in MCRE.Root.Children)
                    {
                        MCREObj currentObj = MCRE.MCFileObjs.Find(c => c.Title == item.Data);
                        if (currentObj == null)
                        {
                            (string, SolidColorBrush) tuple = MinecraftHelper.GetIcon("Folder");
                            CurrentData.Add(new MCREObj()
                            {
                                Title = item.Data,
                                Icon = tuple.Item1,
                                IconColor = tuple.Item2,
                            });
                        }
                        else
                        {
                            CurrentData.Add(currentObj);
                        }
                    }
                }
                contentView.ItemsSource = CurrentData;
            }
            catch (Exception ex)
            {
                await LogSystem.WriteLog(LogLevel.Error, this, $"加载MCREFilePage时{ex.Message} And it is {ex.StackTrace}");
                ContentDialogResult result = await ContentDialogHelper.ShowErrorDialog("加载页面时遇到错误。请将日志反馈到Github上以解决问题。\n（通过“关于”界面上的“Github”超链接）");
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
            await LogSystem.WriteLog(LogLevel.Info, this, "MCREFilePage加载完成。");
            loadingAnimation.IsActive = false;
        }

        private async Task<List<MCREObj>> GetMCREData()
        {
            try
            {
                //初始化树形结构。
                MCRE.Root = new StringTreeNode("root");
                #region 获取Minecraft资源索引文件内容。
                StorageFolder minecraftFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken");
                StorageFolder indexesFolder = await StorageFolder.GetFolderFromPathAsync(Path.Combine(minecraftFolder.Path, "assets", "indexes"));
                string version = IniHelper.ReadIni("Minecraft", "Version", MainPage.ConfigDataPath);
                string fileName = MinecraftHelper.GetFileName(version);
                StorageFile indexeFile = await StorageFile.GetFileFromPathAsync(Path.Combine(indexesFolder.Path, fileName));
                string fileText = await FileIO.ReadTextAsync(indexeFile);
                #endregion

                List<MCREObj> objs = new List<MCREObj>();

                #region 反序列化Minecraft资源索引文件内容，并将提取内容到列表。
                // 将文本转换为Json文档。
                JsonDocument document = JsonDocument.Parse(fileText);
                //解析Json文档中根对象objects中的所有名称。
                document.RootElement.TryGetProperty("objects", out JsonElement objElement);
                //遍历解析的所有名称。
                foreach (JsonProperty objProperty in objElement.EnumerateObject())
                {
                    string[] pathParts = objProperty.Name.Split('/');
                    (string, SolidColorBrush) tuple = MinecraftHelper.GetIcon(Path.GetExtension(objProperty.Name));
                    MCREObj obj = new MCREObj()
                    {
                        Hash = objProperty.Value.GetProperty("hash").ToString(),
                        Title = pathParts[pathParts.Length - 1],//MinecraftHelper.GetTitle(objProperty.Value.GetProperty("hash").ToString(), objProperty.Name),
                        Path = objProperty.Name,
                        Icon = tuple.Item1,
                        IconColor = tuple.Item2,
                    };
                    objs.Add(obj);
                    MCRE.Root.Add(pathParts, 0);
                }
                #endregion

                return objs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void DetermineButton_Click(object sender, RoutedEventArgs e)
        {
            loadingAnimation.IsActive = true;
            determineButton.IsEnabled = false;
            await LogSystem.WriteLog(LogLevel.Info, this, "准备提取MCRE资源...");
            if (MCRE.ReadyData.Count == 0)
            {
                await LogSystem.WriteLog(LogLevel.Warning, this, "用户没有选择需要提取的资源。");
                ContentDialogHelper.ShowTipDialog("请选择要提取的资源。");
                loadingAnimation.IsActive = false;
                determineButton.IsEnabled = true;
                return;
            }
            await CopyFile(MCRE.ReadyData);
            MCRE.ReadyData = new List<MCREObj>();
            numberText.Text = "已选择" + MCRE.ReadyData.Count + "个项目。";
            ContentDialogHelper.ShowTipDialog("提取完毕。");
            await LogSystem.WriteLog(LogLevel.Info, this, "MCRE资源已提取至目标文件夹。");
            determineButton.IsEnabled = true;
            loadingAnimation.IsActive = false;
        }

        private async Task CopyFile(List<MCREObj> targetObjs)
        {
            try
            {
                StorageFolder mcFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken");
                StorageFolder objsFolder = await StorageFolder.GetFolderFromPathAsync(Path.Combine(mcFolder.Path, "assets", "objects"));
                StorageFolder targetFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("TargetFolderToken");
                foreach (MCREObj objItem in targetObjs)
                {
                    StorageFile objFile = await StorageFile.GetFileFromPathAsync(Path.Combine(objsFolder.Path, objItem.Hash.Substring(0, 2), objItem.Hash));
                    await objFile.CopyAsync(targetFolder, objItem.Title);
                }
            }
            catch (Exception ex)
            {
                await LogSystem.WriteLog(LogLevel.Error, this, $"提取MCRE资源时{ex.Message} And it is {ex.StackTrace}");
                ContentDialogResult result = await ContentDialogHelper.ShowErrorDialog("提取资源时遇到错误。请将日志反馈到Github上以解决问题。\n（通过“关于”界面上的“Github”超链接）");
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

        private List<MCREObj> CurrentData;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CurrentData = (List<MCREObj>)e.Parameter;
        }

        private async void ContentView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MCREObj obj = e.ClickedItem as MCREObj;
            List<MCREObj> currentObjs = new List<MCREObj>();
            if (obj.Hash == null)
            {
                await LogSystem.WriteLog(LogLevel.Info, this, "打开用户所点击的文件夹...");
                StringTreeNode treeNode = MCRE.Root.FindNodeByTitle(obj.Title);
                foreach (var item in treeNode.Children)
                {
                    MCREObj currentObj = MCRE.MCFileObjs.Find(c => c.Title == item.Data);
                    if (currentObj == null)
                    {
                        (string, SolidColorBrush) tuple = MinecraftHelper.GetIcon("Folder");
                        currentObjs.Add(new MCREObj()
                        {
                            Title = item.Data,
                            Icon = tuple.Item1,
                            IconColor = tuple.Item2,
                        });
                    }
                    else
                    {
                        currentObjs.Add(currentObj);
                    }
                }
                Frame frame = PageHelper.GetParentElement(this);
                frame.Navigate(typeof(MCREFilePage), currentObjs);

            }
            else
            {
                MCRE.ReadyData.Add(obj);
                numberText.Text = "已选择" + MCRE.ReadyData.Count + "个项目。";
                await LogSystem.WriteLog(LogLevel.Info, this, "将用户所点击的文件添加至提取文件列表...");
            }
        }
    }
}
