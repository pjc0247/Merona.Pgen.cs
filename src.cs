using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merona.PacketGenerator
{
    using PacketFields = Dictionary<String, PacketData.Type>;

    [Pgen]
    public class MyGamePackets
    {
        public class Login
        {

        }
    }

    class Pgen : Attribute
    {
    }


    class PacketData
    {
        public enum Type
        {
            Int16,
            Int32,
            Int64,
            Float,
            String
        }

        public String name { get; set; }

        public PacketFields commonFields { get; set; }
        public PacketFields c2sFields { get; set; }
        public PacketFields s2cFields { get; set; }

        public PacketData()
        {
            commonFields = new PacketFields();
            c2sFields = new PacketFields();
            s2cFields = new PacketFields();
        }
    }
    class PgenData
    {
        public String prefix { get; set; }

        public List<PacketData> packets { get; set; }

        public PgenData()
        {
            packets = new List<PacketData>();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var nested = typeof(MyGamePackets).GetNestedTypes();
            
            foreach(var type in nested)
            {
                
            }

        }
    }
}
