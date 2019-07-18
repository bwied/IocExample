using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IocExample.TypeEntries
{
    public class TypeEntryCollection : TypeEntry
    {
        public override bool IsType(Type type)
        {
            return type == typeof(IEnumerable) || type.GetInterfaces().Contains(typeof(IEnumerable));
        }

        public override void AddMember(Dictionary<string, dynamic> dict, Type type, MemberInfo member)
        {
            var memberType = TypeEntryManager.GetNonNullableMemberType(member);
            var value = TypeEntryManager.GetObjectMembers(memberType);

            if (value == null) return;

            dict.Add($"{member?.DeclaringType?.FullName}.{member?.Name}", value);
        }

        public override void SetMember(KeyValuePair<Func<Type, bool>, TypeEntry> typeEntry, string memberName, object obj, MemberInfo member, Dictionary<string, dynamic> value)
        {
            if (!value.ContainsKey(memberName) || (value[memberName] != null && !IsType(value[memberName].GetType()))) return;

            var memberType = TypeEntryManager.GetNonNullableMemberType(member);

            var arguments = memberType.GenericTypeArguments;
            var arr = value[memberName];

            typeEntry.Value.SetMember(obj, member, arr);
        }
    }
}