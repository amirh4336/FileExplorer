using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsProject.TreeStructure
{
     public class ElementItem
    {
        public string Name;
        public string Path;

        public ElementItem(string name)
        {
            Name = name;
        }
        public ElementItem(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}
