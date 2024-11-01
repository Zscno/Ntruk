using Ntruk.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
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
                    CurrentData = new List<MinecraftResourceFile>();
                    foreach (var item in MCRE.Root.Children)
                    {
                        MinecraftResourceFile currentObj = MCRE.MCFileObjs.Find(c => c.Title == item.Data);
                        if (currentObj == null)
                        {
                            (string, SolidColorBrush) tuple = MinecraftHelper.GetIcon("Folder");
                            CurrentData.Add(new MinecraftResourceFile()
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
                if (MCRE.backPageFiles.Count == 0)
                {
                    MCRE.homePage = CurrentData;
                }
                await LogSystem.WriteLog(LogLevel.Info, this, "MCREFilePage加载完成。");
                loadingAnimation.IsActive = false;
            }
            catch (Exception ex)
            {
                await LogSystem.WriteLog(LogLevel.Error, this, $"加载MCREFilePage时在{ex.TargetSite}触发了{ex.InnerException}：{ex.Message}");
                await ContentDialogHelper.ShowErrorDialog("加载页面时遇到错误。");
                loadingAnimation.IsActive = false;
            }
        }

        private async Task<List<MinecraftResourceFile>> GetMCREData()
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

                List<MinecraftResourceFile> objs = new List<MinecraftResourceFile>();

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
                    MinecraftResourceFile obj = new MinecraftResourceFile()
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
                await ContentDialogHelper.ShowTipDialog("请选择要提取的资源。");
                loadingAnimation.IsActive = false;
                determineButton.IsEnabled = true;
                return;
            }
            await CopyFile(MCRE.ReadyData);
            MCRE.ReadyData = new List<MinecraftResourceFile>();
            numberText.Text = "已选择" + MCRE.ReadyData.Count + "个项目。";
            loadingAnimation.IsActive = false;
            await ContentDialogHelper.ShowTipDialog("提取完毕。");
            await LogSystem.WriteLog(LogLevel.Info, this, "MCRE资源已提取至目标文件夹。");
            determineButton.IsEnabled = true;
        }

        private async Task CopyFile(List<MinecraftResourceFile> targetObjs)
        {
            try
            {
                StorageFolder mcFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("MinecraftFolderToken");
                StorageFolder objsFolder = await StorageFolder.GetFolderFromPathAsync(Path.Combine(mcFolder.Path, "assets", "objects"));
                StorageFolder targetFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("TargetFolderToken");
                foreach (MinecraftResourceFile objItem in targetObjs)
                {
                    if (File.Exists(Path.Combine(targetFolder.Path, objItem.Title)))
                    {
                        break;
                    }
                    StorageFile objFile = await StorageFile.GetFileFromPathAsync(Path.Combine(objsFolder.Path, objItem.Hash.Substring(0, 2), objItem.Hash));
                    await objFile.CopyAsync(targetFolder, objItem.Title);
                }
            }
            catch (Exception ex)
            {
                await LogSystem.WriteLog(LogLevel.Error, this, $"提取MCRE资源时在{ex.TargetSite}触发了{ex.InnerException}：{ex.Message}");
                await ContentDialogHelper.ShowErrorDialog("提取资源时遇到错误。");
            }
        }

        private List<MinecraftResourceFile> CurrentData;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CurrentData = (List<MinecraftResourceFile>)e.Parameter;
        }

        private async void ContentView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MinecraftResourceFile obj = e.ClickedItem as MinecraftResourceFile;
            if (obj.Hash == null)
            {
                List<MinecraftResourceFile> currentObjs = new List<MinecraftResourceFile>();
                await LogSystem.WriteLog(LogLevel.Info, this, "打开用户所点击的文件夹...");
                StringTreeNode treeNode = MCRE.Root.FindNodeByTitle(obj.Title);
                foreach (var item in treeNode.Children)
                {
                    MinecraftResourceFile currentObj = MCRE.MCFileObjs.Find(c => c.Title == item.Data);
                    if (currentObj == null)
                    {
                        (string, SolidColorBrush) tuple = MinecraftHelper.GetIcon("Folder");
                        currentObjs.Add(new MinecraftResourceFile()
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
                MCRE.backPageFiles.Add(CurrentData);
                (Parent as Frame).Navigate(typeof(MCREFilePage), currentObjs);
            }
            else
            {
                for (int i = 0; i < MCRE.ReadyData.Count; i++)
                {
                    if (obj.Hash == MCRE.ReadyData[i].Hash)
                    {
                        MCRE.ReadyData.Remove(obj);
                        numberText.Text = $"已选择{MCRE.ReadyData.Count}个项目。";
                        await LogSystem.WriteLog(LogLevel.Info, this, $"将用户已选择的文件（路径：{obj.Path}）从提取文件列表中清除。");
                        return;
                    }
                }
                MCRE.ReadyData.Add(obj);
                numberText.Text = "已选择" + MCRE.ReadyData.Count + "个项目。";
                await LogSystem.WriteLog(LogLevel.Info, this, "将用户所点击的文件添加至提取文件列表...");
            }
        }
    }
}
