using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using Merona.PacketGenerator;

namespace Testbed
{
    [PgenTarget]
    public class MyGamePackets
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class Login
        {
            [C2S]
            public String id;
            public String password;

            [S2C]
            public bool result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            P.Gen("output_test.cs", P.Target.Csharp);
        }
    }
}
