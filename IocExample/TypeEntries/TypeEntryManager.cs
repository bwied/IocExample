using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IocExample.TypeEntries
{
    public static class TypeEntryManager
    {
        public static T GetObject<T>(T message, Type type, Dictionary<string, dynamic> value)
        {
            foreach (var member in type.GetMembers().Where(x => new[] { MemberTypes.Property, MemberTypes.Field }.Contains(x.MemberType)))
            {
                var memberTypeType = GetNonNullableMemberType(member);
                var memberName = $"{type.FullName}.{member.Name}";
                var typeEntry = TypeEntryRegistry.Instance.GetTypeEntry(memberTypeType);

                if (typeEntry.Key == null) continue;

                typeEntry.SetMember(memberName, message, member, value);
            }

            return message;
        }

        public static Dictionary<string, dynamic> GetObjectMembers(Type type)
        {
            var dict = new Dictionary<string, dynamic>();

            foreach (var member in type.GetMembers().Where(x => new[] { MemberTypes.Property, MemberTypes.Field }.Contains(x.MemberType)))
            {
                var memberType = GetNonNullableMemberType(member);
                var typeEntry = GetRegistryTypeEntry(memberType);

                AddRegistryTypeEntry(typeEntry, dict, type, member);
            }

            return dict;
        }

        private static Type GetMemberType(MemberInfo member)
        {
            Type type = null;

            if (member.MemberType == MemberTypes.Property && ((PropertyInfo)member).CanWrite)
                type = ((PropertyInfo)member).PropertyType;
            else if (member.MemberType == MemberTypes.Field && !((FieldInfo)member).IsInitOnly)
                type = ((FieldInfo)member).FieldType;
            
            return type;
        }

        private static Type GetUnderlyingType(Type type)
        {
            if (type == null) return null;

            var underlyingType = Nullable.GetUnderlyingType(type);

            if (underlyingType != null)
            {
                type = underlyingType;
            }

            return type;
        }

        public static Type GetNonNullableMemberType(MemberInfo member)
        {
            var memberType = GetMemberType(member);

            memberType = GetUnderlyingType(memberType);

            return memberType;
        }

        private static KeyValuePair<Func<Type, bool>, TypeEntry> GetRegistryTypeEntry(Type memberType)
        {
            return memberType == null ? new KeyValuePair<Func<Type, bool>, TypeEntry>() : TypeEntryRegistry.Instance.GetTypeEntry(memberType);
        }

        private static void AddRegistryTypeEntry(KeyValuePair<Func<Type, bool>, TypeEntry> typeEntry, Dictionary<string, dynamic> dict, Type type, MemberInfo member)
        {
            if (typeEntry.Key == null) return;

            typeEntry.Value.AddMember(dict, type, member);
        }
    }
}