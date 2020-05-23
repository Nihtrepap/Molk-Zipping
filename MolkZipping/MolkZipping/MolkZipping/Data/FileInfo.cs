using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolkZipping
{
    /// <summary>
    /// Used to get filename and filetype.
    /// </summary>
    public class FileInfo
    {
        public string Name { get; private set; }
        public string Type { get; private set; }

        public FileInfo(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
