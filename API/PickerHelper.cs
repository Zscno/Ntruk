using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;

namespace Ntruk.API
{
    internal class PickerHelper
    {
        /// <summary>
        /// 使用选取器获取单个文件夹。
        /// </summary>
        /// <param name="token">将获取到的文件夹添加到访问列表时赋予的名称。</param>
        /// <returns>一个<see cref="StorageFolder"/>类型的对象。</returns>
        public async static Task<StorageFolder> UsePickerGetSingleFolder(string token)
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
                StorageApplicationPermissions.FutureAccessList.AddOrReplace(token, folder);
                return folder;
            }
            return null;
        }
    }
}
