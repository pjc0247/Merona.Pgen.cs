using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Merona.PacketGenerator
{
    public sealed partial class P
    {
        public enum Target
        {
            Cpp,
            Csharp
        }

        public static PgenData Gen(String path, Target target)
        {
            var data = ParseAssembly();

            Output(path, data, target);
            
            return data;      
        }
    }
}
