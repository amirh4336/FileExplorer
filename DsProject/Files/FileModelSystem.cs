using DsProject.TreeStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsProject.Files
{
    public class FileModelSystem : IPosition<string>
    {
            public string Element { get; set; }

            public List<IPosition<string>> Children { get; set; } = new List<IPosition<string>>();
    }
}
