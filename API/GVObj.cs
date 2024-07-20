using System.Collections.Generic;
using Windows.UI.Xaml.Media;

namespace Ntruk.API
{
    internal class HomeObj
    {
        /// <summary>
        /// 显示在界面上的图标的相对路径。
        /// </summary>
        public string IconLocation { get; set; }

        /// <summary>
        /// 显示在界面上的标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 显示在界面上的介绍。
        /// </summary>
        public string Description { get; set; }
    }

    internal class MCREObj
    {
        /// <summary>
        /// 显示在界面上的标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 路径。
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 显示在界面上的图标。
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 显示在界面上的图标的填充颜色。
        /// </summary>
        public SolidColorBrush IconColor { get; set; }

        /// <summary>
        /// 哈希值。
        /// </summary>
        public string Hash { get; set; }
    }
}
