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
        public string Size { get; private set; }
        public string Date { get; private set; }
        public string Time { get; private set; }

        public Pack(string name, string size, string date, string time)
        {
            Name = name;
            Size = size;
            Date = date;
            Time = time;
        }
    }
}
