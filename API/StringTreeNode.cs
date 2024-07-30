using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntruk.API
{
    internal class StringTreeNode
    {
        /// <summary>
        /// 该节点中所有子节点的集合。
        /// </summary>
        public List<StringTreeNode> Children { get; set; }

        /// <summary>
        /// 该节点的数据。
        /// </summary>
        public string Data {  get; set; }

        public StringTreeNode(string data)
        {
            Data = data;
            Children = new List<StringTreeNode>();
        }

        /// <summary>
        /// 添加子节点（递归）。
        /// </summary>
        /// <param name="pathParts">路径各个部分组成的<see cref="string"/>数组。</param>
        /// <param name="index">要添加的子节点在<paramref name="pathParts"/>中的位置。</param>
        public void Add(string[] pathParts, int index)
        {
            //越界检查。
            if (index >= pathParts.Length)
            {
                return;
            }
            //获取要操作的部分。
            string currentPart = pathParts[index];
            //跳过"minecraft"。
            if (currentPart == "minecraft")
            {
                Add(pathParts, index + 1);
                return;
            }
            //检查是否已出现此节点。
            StringTreeNode child = Children.Find(c => c.Data == currentPart);
            if (child == null)
            {
                child = new StringTreeNode(currentPart);
                Children.Add(child);
            }
            child.Add(pathParts, index + 1);
        }

        /// <summary>
        /// 通过标题寻找节点。
        /// </summary>
        /// <param name="title">标题。</param>
        /// <returns>找到的节点（没找到则返回null）。</returns>
        public StringTreeNode FindNodeByTitle(string title)
        {
            if (Data == title)
            {
                return this;
            }

            foreach (var child in Children)
            {
                var result = child.FindNodeByTitle(title);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }
}
