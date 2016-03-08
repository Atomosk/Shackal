using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shackal.Core;

namespace Shackal.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = args[0];
            var partsCount = int.Parse(args[1]);
            new Compressor().Compress(fileName, partsCount);
        }
    }
}
