using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Ntruk.API
{
    /// <summary>
    /// 关于Page的常用操作。
    /// </summary>
    internal class PageHelper
    {
        /// <summary>
        /// 让<paramref name="currentObj"/>的父控件（<c>Frame</c>类型）导航到<paramref name="page"/>。
        /// </summary>
        /// <param name="currentObj">当前的控件。</param>
        /// <param name="page">将要导航的控件。</param>
        /// <returns>指示操作是否成功。返回<c>""</c>就是操作成功；返回错误信息就是操作失败。</returns>
        public static string NavigateOneselfTo(DependencyObject currentObj, Type page)
        {
            try
            {
                
                Frame frame = GetParentElement(currentObj);
                frame.Navigate(page);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取<paramref name="element"/>的父元素。
        /// </summary>
        /// <param name="element">要操作的元素。</param>
        /// <returns>一个<c>Frame</c>实例。<para>当返回<c>null</c>时，操作不成功。</para></returns>
        public static Frame GetParentElement(DependencyObject element)
        {
            try
            {
                DependencyObject parent = VisualTreeHelper.GetParent(element);
                while (parent != null && !(parent is Frame))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
                return parent as Frame;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
