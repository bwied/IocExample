using System;
using System.Collections.Generic;
using System.Linq;

namespace IocExample.TypeEntries
{
    internal class TypeEntryRegistry
    {
        private static TypeEntryRegistry _instance;

        public static TypeEntryRegistry Instance => _instance ?? (_instance = new TypeEntryRegistry());

        private IDictionary<Func<Type, bool>, TypeEntry> TypeEntries { get; } = new Dictionary<Func<Type, bool>, TypeEntry>();

        private TypeEntryRegistry()
        {
            var numericEntry = new TypeEntryNumeric();
            PutTypeEntry(numericEntry.IsType, numericEntry);
            var stringEntry = new TypeEntryString();
            PutTypeEntry(stringEntry.IsType, stringEntry);
            var collectionEntry = new TypeEntryCollection();
            PutTypeEntry(collectionEntry.IsType, collectionEntry);
            var boolEntry = new TypeEntryBool();
            PutTypeEntry(boolEntry.IsType, boolEntry);
            var classEntry = new TypeEntryClass();
            PutTypeEntry(classEntry.IsType, classEntry);
        }

        private void PutTypeEntry(Func<Type, bool> isType, TypeEntry typeEntry)
        {
            if (TypeEntries.ContainsKey(isType))
            {
                TypeEntries[isType] = typeEntry;
            }
            else
            {
                TypeEntries.Add(isType, typeEntry);
            }
        }

        public void DeleteTypeEntry(Func<Type, bool> isType)
        {
            TypeEntries.Remove(isType);
        }

        public KeyValuePair<Func<Type, bool>, TypeEntry> GetTypeEntry(Type type)
        {
            var values = TypeEntries.Where(x => x.Key.Invoke(type)).ToList();

            return values.Count > 1 ? values.FirstOrDefault(x => x.Value.GetType() != typeof(TypeEntryClass)) : values.FirstOrDefault();
        }

        public IDictionary<Func<Type, bool>, TypeEntry> GetTypeEntries()
        {
            return TypeEntries;
        }
    }
}