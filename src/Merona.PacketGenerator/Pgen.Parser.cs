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
        private enum FieldType
        {
            Common,
            C2S,
            S2C
        }

        static PacketData.Type GetFieldType(FieldInfo field)
        {
            var type = field.FieldType;

            if (type == typeof(Int16))
                return PacketData.Type.Int16;
            else if (type == typeof(Int32))
                return PacketData.Type.Int32;
            else if (type == typeof(Int64))
                return PacketData.Type.Int64;
            else if (type == typeof(String))
                return PacketData.Type.String;
            else if (type == typeof(float))
                return PacketData.Type.Float;

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
                if (field.GetCustomAttribute<C2S>() != null)
                    fieldType = FieldType.C2S;
                else if (field.GetCustomAttribute<S2C>() != null)
                    fieldType = FieldType.S2C;

                switch (fieldType)
                {
                    case FieldType.Common:
                        data.commonFields.Add(field.Name, GetFieldType(field));
                        break;
                    case FieldType.C2S:
                        data.c2sFields.Add(field.Name, GetFieldType(field));
                        break;
                    case FieldType.S2C:
                        data.s2cFields.Add(field.Name, GetFieldType(field));
                        break;
                }
            }

            return data;
        }
    }
}
