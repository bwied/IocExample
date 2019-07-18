using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IocExample.TypeEntries
{
    public class TypeEntryNumeric : TypeEntry
    {
        public Type[] Types { get; } = new[] { typeof(int), typeof(decimal), typeof(double), typeof(float), typeof(long), typeof(short), typeof(uint), typeof(ushort), typeof(ulong) };

        public override bool IsType(Type type)
        {
            return Types.Contains(type);
        }

        public override void SetMember(KeyValuePair<Func<Type, bool>, TypeEntry> typeEntry, string memberName, object obj, MemberInfo member, Dictionary<string, dynamic> value)
        {
            //throw new NotImplementedException();
        }
    }
}