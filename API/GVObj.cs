using System.Drawing;
using Windows.UI.Xaml.Media;

namespace Ntruk.API
{
    internal class HomeObj
    {
        public string IconLocation { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    internal class MCREObj
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public SolidColorBrush IconColor { get; set; }
        public string Hash {  get; set; }
        public int Tag {  get; set; }
    }
}
