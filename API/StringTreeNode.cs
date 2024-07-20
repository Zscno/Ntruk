using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntruk.API
{
    internal class StringTreeNode
    {
        public List<StringTreeNode> Children { get; set; }

        public string Data {  get; set; }

        public StringTreeNode(string data)
        {
            Data = data;
            Children = new List<StringTreeNode>();
        }

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
