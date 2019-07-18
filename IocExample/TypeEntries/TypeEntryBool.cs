using System;
using System.Collections.Generic;
using System.Reflection;

namespace IocExample.TypeEntries
{
    public class TypeEntryBool : TypeEntry
    {
        public override bool IsType(Type type)
        {
            return type == typeof(bool);
        }

        public override void SetMember(KeyValuePair<Func<Type, bool>, TypeEntry> typeEntry, string memberName, object obj, MemberInfo member, Dictionary<string, dynamic> value)
        {
            if (!value.ContainsKey(memberName)) return;
            var memberValue = ParseValue(value[memberName]);
            typeEntry.Value.SetMember(obj, member, memberValue);
        }

        private bool ParseValue(string value)
        {
            return value == "1" || value.ToLower() == "true";
        }
    }
}