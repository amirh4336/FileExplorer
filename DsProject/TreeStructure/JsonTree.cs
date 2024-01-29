using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsProject.TreeStructure
{
    public class JsonTreeNode
    {
        public ElementItem Element { get; set; }
        public IEnumerable<JsonTreeNode> Children { get; set; }

        public JsonTreeNode() { }
    }
}
