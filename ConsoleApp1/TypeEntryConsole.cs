using System;
using System.Collections.Generic;
using FirebaseAdmin.Messaging;
using IocExample.TypeEntries;

namespace IocExample
{
    public class TypeEntryConsole
    {
        public static void Execute()
        {
            var console = new TypeEntryConsole();
            var completedPropeties = console.Display();
            var message = TypeEntryManager.GetObject(new Message(), typeof(Message), completedPropeties);

            console.Send(message);
        }

        private TypeEntryConsole()
        {
        }

        private void Send(Message message)
        {
            var test = new IocExample.Class1();
            test.Authenticate();
            var response = FirebaseMessaging.DefaultInstance.SendAsync(message);

            response.Wait();
        }

        private Dictionary<string, dynamic> Display()
        {
            var completedPropeties = new Dictionary<string, dynamic>();
            var properties = TypeEntryManager.GetObjectMembers(typeof(Message));

            Display(properties, completedPropeties);

            return completedPropeties;
        }

        private void Display(Dictionary<string, dynamic> properties, Dictionary<string, dynamic> completedPropeties)
        {
            foreach (var property in properties)
            {
                if (property.Value == null)
                {
                    Console.WriteLine($"Enter a value for {property.Key}:");
                    var result = Console.ReadLine();

                    if (!string.IsNullOrEmpty(result))
                    {
                        completedPropeties.Add(property.Key, property.Value);
                        completedPropeties[property.Key] = result;
                    }
                }
                else
                {
                    Console.WriteLine($"Has {property.Key}:");
                    var result = Console.ReadLine() == "1";

                    if (result)
                    {
                        completedPropeties.Add(property.Key, new Dictionary<string, dynamic>());
                        Display(((Dictionary<string, dynamic>)properties[property.Key]), completedPropeties);
                    }
                }
            }
        }
    }
}