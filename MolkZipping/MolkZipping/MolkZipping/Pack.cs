using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolkZipping
{
    public class Pack
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public double Size { get; private set; }

        public Pack(string name, string type, double size)
        {
            Name = name;
            Type = type;
            Size = size;
        }
    }
}
