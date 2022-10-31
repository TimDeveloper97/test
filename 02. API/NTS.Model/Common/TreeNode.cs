using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Common
{
    public class TreeNode<T>
    {
        public T data { get; set; }
        public List<TreeNode<T>> children { get; set; }
    }
}
