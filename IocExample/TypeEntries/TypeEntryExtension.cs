using System;
using System.Collections.Generic;
using System.Reflection;

namespace IocExample.TypeEntries
{
    public static class TypeEntryExtension
    {
        public static void SetMember(this KeyValuePair<Func<Type, bool>, TypeEntry> typeEntry, string memberName, object obj, MemberInfo member, Dictionary<string, dynamic> value)
        {
            typeEntry.Value.SetMember(typeEntry, memberName, obj, member, value);
        }
    }
}