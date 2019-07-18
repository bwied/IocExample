using System;
using System.Collections.Generic;
using System.Reflection;

namespace IocExample.TypeEntries
{
    public class TypeEntryClass : TypeEntry
    {
        public override bool IsType(Type type)
        {
            return type.IsClass;
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
            if (!value.ContainsKey(memberName)) return;

            var memberType = TypeEntryManager.GetNonNullableMemberType(member);
            var derivedObj = TypeEntryManager.GetObject(Activator.CreateInstance(memberType), memberType, value);
            typeEntry.Value.SetMember(obj, member, derivedObj);
        }
    }
}