using System;
using System.Collections.Generic;
using System.Reflection;

namespace IocExample.TypeEntries
{
    public class TypeEntryString : TypeEntry
    {
        public override bool IsType(Type type)
        {
            return type == typeof(string);
        }

        public override void SetMember(KeyValuePair<Func<Type, bool>, TypeEntry> typeEntry, string memberName, object obj, MemberInfo member, Dictionary<string, dynamic> value)
        {
            if (!value.ContainsKey(memberName)) return;
            typeEntry.Value.SetMember(obj, member, value[memberName]);
        }
    }
}