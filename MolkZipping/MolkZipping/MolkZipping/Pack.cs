﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MolkZipping
{
    /// <summary>
    /// Used to get filename and filetype.
    /// </summary>
    public class Pack
    {
        public string Name { get; private set; }
        public string Type { get; private set; }

        public Pack(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
