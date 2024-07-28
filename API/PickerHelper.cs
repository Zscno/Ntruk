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
        /// <returns>一个<see cref="StorageFolder"/>类型的对象。<para>（如果用户没有选择文件夹将返回null）</para></returns>
        public async static Task<StorageFolder> GetSingleFolderByPicker()
        {
            FolderPicker picker = new FolderPicker()
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.Desktop
            };
            picker.FileTypeFilter.Add("*");
            return await picker.PickSingleFolderAsync();
        }
    }
}
