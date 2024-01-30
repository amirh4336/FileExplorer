using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsProject.TreeStructure
{
    public class ElementItem
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public long? Size { get; set; }

        public ElementItem() { }

        public ElementItem(string name)
        {
            Name = name;
        }

        public ElementItem(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public ElementItem(string name , long size)
        {
            Name = name;
            Size = size;
        }
    }
}
