using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Merona.PacketGenerator
{
    public sealed partial class P
    {
        private static String BuildCSharp(PgenData data)
        {
            OutputCSharp cs = new OutputCSharp(data);
            return cs.TransformText();
        }
        private static String BuildCpp(PgenData data)
        {
            OutputCpp cpp = new OutputCpp(data);
            return cpp.TransformText();
        }

        private static void Output(String path, PgenData data, Target target)
        {
            var result = "";

            switch (target)
            {
                case Target.Cpp:
                    result = BuildCpp(data);
                    break;
                case Target.Csharp:
                    result = BuildCSharp(data);
                    break;
            }

            File.WriteAllText(path, result);
        }
    }
}
