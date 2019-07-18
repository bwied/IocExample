using System;
using System.Collections.Generic;
using System.Reflection;

namespace IocExample.TypeEntries
{
    public abstract class TypeEntry
    {
        public abstract bool IsType(Type type);
        private readonly List<MemberTypes> _validMemberTypes;

        protected TypeEntry()
        {
            _validMemberTypes = new List<MemberTypes>() { MemberTypes.Property, MemberTypes.Field };
        }

        public virtual void AddMember(Dictionary<string, dynamic> dict, Type type, MemberInfo member)
        {
            dict.Add($"{member?.DeclaringType?.FullName}.{member?.Name}", null);
        }

        public abstract void SetMember(KeyValuePair<Func<Type, bool>, TypeEntry> typeEntry, string memberName, object obj, MemberInfo member, Dictionary<string, dynamic> value);

        public virtual void SetMember(object obj, MemberInfo member, dynamic value)
        {
            if (!_validMemberTypes.Contains(member.MemberType)) return;

            switch (member.MemberType)
            {
                case MemberTypes.Property:
                    SetProperty(obj, member, value);
                    break;
                case MemberTypes.Field:
                    SetField(obj, member, value);
                    break;
            }
        }

        protected virtual void SetProperty(object obj, MemberInfo property, dynamic value)
        {
            var derivedProperty = (PropertyInfo)property;
            derivedProperty.SetValue(obj, value);
        }

        protected virtual void SetField(object obj, MemberInfo field, dynamic value)
        {
            var derivedField = (FieldInfo)field;
            derivedField.SetValue(obj, value);
        }
    }
}