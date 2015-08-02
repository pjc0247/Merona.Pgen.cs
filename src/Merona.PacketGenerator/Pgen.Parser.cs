using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Merona.PacketGenerator
{
    public sealed partial class P
    {
        private enum FieldType
        {
            Common,
            C2S,
            S2C
        }

        static FieldData.Type GetFieldType(FieldInfo field)
        {
            var type = field.FieldType;

            if (type == typeof(Int16))
                return FieldData.Type.Int16;
            else if (type == typeof(Int32))
                return FieldData.Type.Int32;
            else if (type == typeof(Int64))
                return FieldData.Type.Int64;
            else if (type == typeof(String))
                return FieldData.Type.String;
            else if (type == typeof(float))
                return FieldData.Type.Float;
            else if (type == typeof(bool))
                return FieldData.Type.Bool;

            throw new ArgumentException("unsupported type");
        }

        private static PgenData ParseAssembly()
        {
            var assembly = Assembly.GetEntryAssembly();
            var pgen = new PgenData();

            foreach (var type in assembly.GetTypes())
            {
                var attr = type.GetCustomAttribute<PgenTarget>();
                if (attr != null)
                {
                    var entry = new PacketListEntry();
                    
                    entry.prefix = type.Name;
                    foreach (var packet in type.GetNestedTypes())
                    {
                        entry.packets.Add(ParsePacket(packet));
                    }

                    pgen.entries.Add(entry);
                }
            }

            foreach (var entry in pgen.entries)
            {
                foreach (var packet in entry.packets)
                {
                    foreach (var c2s in packet.c2sFields)
                        Console.WriteLine("S2C {0} : {1}", c2s.Key, c2s.Value);
                }
            }

            return pgen;
        }

        private static PacketData ParsePacket(Type packet)
        {
            var data = new PacketData();
            var fields =
                packet.GetFields()
                .OrderBy(field => field.MetadataToken);

            FieldType fieldType = FieldType.Common;

            foreach (var field in fields)
            {
                var fieldData = new FieldData();
                fieldData.field = field;
                fieldData.type = GetFieldType(field);
                
                /* 속성들 파싱 */
                var attrs = field.GetCustomAttributes();
                foreach (var attr in attrs)
                {
                    if (attr.GetType() == typeof(C2S))
                        fieldType = FieldType.C2S;
                    else if (attr.GetType() == typeof(S2C))
                        fieldType = FieldType.S2C;
                    else if (attr.GetType() == typeof(MarshalAsAttribute))
                    {
                        fieldData.marshal = (MarshalAsAttribute)attr;
                    }
                }
                
                switch (fieldType)
                {
                    case FieldType.Common:
                        data.commonFields.Add(field.Name, fieldData);
                        break;
                    case FieldType.C2S:
                        data.c2sFields.Add(field.Name, fieldData);
                        break;
                    case FieldType.S2C:
                        data.s2cFields.Add(field.Name, fieldData);
                        break;
                }
            }

            return data;
        }
    }
}
