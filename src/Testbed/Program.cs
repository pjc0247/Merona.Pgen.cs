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
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst =32)]
            public String id;
            public String password;

            [S2C]
            public bool result;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class Logout
        {
            [C2S]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String id;

            [S2C]
            public bool result;
        }
    }

    class Program
    {
        static byte[] ToByte<T>(T data)
        {
            unsafe
            {
                byte[] byteArray = new byte[Marshal.SizeOf(data)];

                fixed (byte* ptr = byteArray)
                {
                    Marshal.StructureToPtr(data, (IntPtr)ptr, true);
                }

                return byteArray;
            }
        }
        static void Main(string[] args)
        {

            var obj = new MyGamePackets.Login();

            obj.id = "ASDFASDFASDFASDFASDF";

            Console.WriteLine(Marshal.SizeOf<MyGamePackets.Login>());
            Console.WriteLine(Marshal.SizeOf(obj));

            Console.WriteLine(ToByte<MyGamePackets.Login>(obj).Length);
            foreach (var b in ToByte<MyGamePackets.Login>(obj))
                Console.Write(b.ToString() + " ");
            Console.WriteLine();

            P.Gen("output_test.cs", P.Target.Csharp);
        }
    }
}
