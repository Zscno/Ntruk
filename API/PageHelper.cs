using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Ntruk.API
{
    internal class PageHelper
    {
        /// <summary>
        /// 让<paramref name="currentObj"/>的父控件（<see cref="Frame"/>类型）导航到<see cref="Page"/>。
        /// </summary>
        /// <param name="currentObj">当前控件。</param>
        /// <param name="page">将要导航的控件。</param>
        /// <returns>指示操作是否成功。返回空字符串就是操作成功；返回错误信息就是操作失败。</returns>
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
        /// <returns>一个<see cref="Frame"/>类型的对象。<para>返回<see cref="null"/>则操作失败。</para></returns>
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

        /// <summary>
        /// 获取<paramref name="depObj"/>中的<see cref="RelativePanel"/>类型（采用递归，直到找到<see cref="RelativePanel"/>类型）。
        /// </summary>
        /// <param name="depObj">要操作的元素。</param>
        /// <returns>一个<see cref="RelativePanel"/>类型的对象。</returns>
        private static RelativePanel FindChild<RelativePanel>(DependencyObject depObj) where RelativePanel : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child != null && child is RelativePanel panel)
                {
                    return panel;
                }
                else
                {
                    RelativePanel childItem = FindChild<RelativePanel>(child);
                    if (childItem != null)
                    {
                        return childItem;
                    }
                }
            }
            return null;
        }
    }
}
