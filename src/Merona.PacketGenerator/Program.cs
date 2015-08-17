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
    using PacketFields = List<FieldData>;

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

    public class Emit : Attribute
    {
        public String msg { get; set; }

        public Emit(String msg)
        {
            this.msg = msg;
        }
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
        public String name { get; set; }

        public MarshalAsAttribute marshal { get;set; }

        public List<String> emits { get; set; }

        public FieldData()
        {
            emits = new List<String>();
        }

        public String ToCppField()
        {
            return new CppField(this).TransformText();
        }
        public String ToCSharpField()
        {
            return new CSharpField(this).TransformText();
        }
    }
    public class PacketData
    {
        public int id { get; set; }
        public String name { get; set; }
        public Type type { get; set; }

        public PacketFields commonFields { get; set; }
        public PacketFields c2sFields { get; set; }
        public PacketFields s2cFields { get; set; }

        public List<String> emits { get; set; }
  
        public PacketData()
        {
            commonFields = new PacketFields();
            c2sFields = new PacketFields();
            s2cFields = new PacketFields();
            emits = new List<String>();

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
    partial class OutputCpp
    {
        private PgenData pgen { get; set; }
        private int id { get; set; }

        public OutputCpp(PgenData pgen)
        {
            this.pgen = pgen;
            this.id = 0;
        }
    }

    partial class CppField
    {
        private FieldData field { get; set; }

        public CppField(FieldData field)
        {
            this.field = field;
        }

        private String TypeToString()
        {
            switch (field.type)
            {
                case FieldData.Type.Bool:
                    return "bool";
                case FieldData.Type.Float:
                    return "float";
                case FieldData.Type.Int16:
                    return "short";
                case FieldData.Type.Int32:
                    return "int";
                case FieldData.Type.Int64:
                    return "long int";
                case FieldData.Type.String:
                    return "char";
            }
            throw new InvalidOperationException();
        }
        private String SuffixToString()
        {
            if (field.type == FieldData.Type.String)
            {
                return "[" + field.marshal.SizeConst.ToString() + "]";
            }

            return "";
        }
    }
    partial class CSharpField
    {
        private FieldData field { get; set; }

        public CSharpField(FieldData field)
        {
            this.field = field;
        }
    }
    partial class OutputCSharp
    {
        private PgenData pgen { get; set; }
        private int id { get; set; }

        public OutputCSharp(PgenData pgen)
        {
            this.pgen = pgen;
            this.id = 0;
        }
    }
}
