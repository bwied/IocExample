using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace IocExample
{
    public class Class1
    {
        public async Task<string> Main()
        {
            var response = "";
            Authenticate();

            try
            {
                response = await FirebaseMessaging.DefaultInstance.SendAsync(new Message() { Token = "cNdKJeWfhyo:APA91bFDoKtrHSySIq9xO4ybVb-WV_lRWXXAv7KSl13pFNzg85xUQUzQHnzTHqOOuzFayDLvkGTaSzMvIPv8bBG8jZmj1hXMQzNaI_i6Di9TqXyt57bhqVakLQdO9xvE_tTHnWQJFQkq", Notification = new Notification() { Title = "Brian", Body = "Test" } }, false);

                Console.WriteLine("Message sent");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
            catch (FirebaseException ex)
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return response;
        }

        public bool IsNumeric(Type type)
        {
            var numericTypes = new[] { typeof(int), typeof(decimal), typeof(double), typeof(float), typeof(long), typeof(short), typeof(uint), typeof(ushort), typeof(ulong) };

            return numericTypes.Contains(type);
        }

        public T GetObject<T>(T message, Type type)
        {
            foreach (var prop in type.GetProperties())
            {
                var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
                Type propertyType = prop.PropertyType;

                if (underlyingType != null)
                {
                    propertyType = underlyingType;
                }

                if (propertyType == typeof(string))
                {
                    Console.WriteLine($"Enter a value for {prop.Name}: ({propertyType})");
                    var value = Console.ReadLine();
                    prop.SetValue(message, string.IsNullOrEmpty(value) ? null : value);
                }
                else if (propertyType == typeof(bool))
                {
                    Console.WriteLine($"Enter a value ('true' or 'false') for {prop.Name}: ({propertyType})");
                    var value = Console.ReadLine()?.ToLower();
                    prop.SetValue(message, Convert.ChangeType(string.IsNullOrEmpty(value) ? null : value, propertyType));
                }
                else if (IsNumeric(propertyType))
                {
                    Console.WriteLine($"Enter a value for {prop.Name}: ({propertyType})");
                    var value = Console.ReadLine();
                    prop.SetValue(message, Convert.ChangeType(string.IsNullOrEmpty(value) ? null : value, propertyType));
                }
                else if (propertyType == typeof(IReadOnlyDictionary<string, string>))
                {
                    var dict = IocRegistration.Container.Instance.IocContainer.Resolve<IReadOnlyDictionary<string, string>>();
                    bool hasValue;
                    do
                    {
                        Console.WriteLine($"Has {prop.Name}? ({propertyType})");

                        hasValue = Console.ReadLine() == "1";

                        if (hasValue)
                        {
                            Console.WriteLine("Enter a [key][,][value] pair");
                            var pair = Console.ReadLine()?.Split(',');

                            if (pair != null && pair.Length == 2)
                            {
                                ((IDictionary<string, string>)dict).Add(pair[0], pair[1]);
                            }
                        }
                    } while (hasValue);

                    prop.SetValue(message, dict);
                }
                else if (propertyType.IsEnum)
                {
                    Console.WriteLine($"Has {prop.Name}? ({propertyType})");

                    var hasValue = Console.ReadLine() == "1";

                    if (hasValue)
                    {
                        var enumType = IsNullableEnum(propertyType) ? Nullable.GetUnderlyingType(propertyType) : propertyType;

                        if (enumType != null)
                        {
                            Console.WriteLine("Enter one of the following:");

                            foreach (int value in Enum.GetValues(enumType))
                            {
                                Console.WriteLine($"{value} - {Enum.GetName(enumType, value)}");
                            }

                            var userInput = Console.ReadLine();
                            if (userInput != null && int.TryParse(userInput, out var intValue))
                            {
                                var enumValue = Enum.GetName(enumType, intValue);
                                if (enumValue == null) continue;
                                prop.SetValue(message, Enum.Parse(enumType, enumValue));
                            }
                        }
                    }
                }
                else if (propertyType.IsClass)
                {
                    Console.WriteLine($"Has {prop.Name}? ({propertyType})");
                    var hasValue = Console.ReadLine() == "1";

                    if (hasValue)
                    {
                        var obj = Activator.CreateInstance(propertyType);
                        prop.SetValue(message, GetObject(obj, propertyType));
                    }
                }
            }

            return message;
        }

        public static bool IsNullableEnum(Type t)
        {
            Type u = Nullable.GetUnderlyingType(t);
            return (u != null) && u.IsEnum;
        }

        public void Authenticate()
        {
            try
            {
                //var cred = await GoogleCredential.GetApplicationDefaultAsync();
                if(FirebaseApp.DefaultInstance != null) return;

                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.GetApplicationDefault()
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
