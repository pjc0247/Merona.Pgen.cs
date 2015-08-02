using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TextTemplating.Modeling;
using Microsoft.VisualStudio.TextTemplating;

namespace Merona.PacketGenerator
{
    using PacketFields = Dictionary<String, FieldData>;

    [AttributeUsage(AttributeTargets.Field)]
    public class C2S : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class S2C : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class PgenTarget : Attribute
    {
    }


    public class FieldData
    {
        public enum Type
        {
            Int16,
            Int32,
            Int64,
            Float,
            String,
            Bool
        }

        public FieldInfo field { get; set; }
        public Type type { get; set; }

        public MarshalAsAttribute marshal { get;set; }
    }
    public class PacketData
    {
        

        public String name { get; set; }

        public PacketFields commonFields { get; set; }
        public PacketFields c2sFields { get; set; }
        public PacketFields s2cFields { get; set; }

        public PacketData()
        {
            commonFields = new PacketFields();
            c2sFields = new PacketFields();
            s2cFields = new PacketFields();

            name = "";
        }
    }
    public class PacketListEntry
    {
        public String prefix { get; set; }

        public List<PacketData> packets { get; set; }

        public PacketListEntry()
        {
            packets = new List<PacketData>();
        }
    }
    public class PgenData
    {
        public List<PacketListEntry> entries;

        public PgenData()
        {
            entries = new List<PacketListEntry>();
        }
    }

    partial class OutputCSharp
    {
        private PgenData pgen { get; set; }

        public OutputCSharp(PgenData pgen)
        {
            this.pgen = pgen;
        }
    }
}
