using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using IocExample;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var selection = string.Empty;

            do
            {
                try
                {

                    Console.Clear();
                    Console.WriteLine("Select a program:");
                    Console.WriteLine("0 - Exit");
                    Console.WriteLine("1 - Class1 (static)");
                    Console.WriteLine("2 - Class1");
                    Console.WriteLine("3 - TypeEntryConsole");
                    selection = Console.ReadLine();
                    DisplayMenu(selection);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                }
            } while (selection != "0");
        }

        static bool DisplayMenu(string selection)
        {
            var class1 = new IocExample.Class1();
            class1.Authenticate();

            switch (selection)
            {
                case "1":
                    var test = new IocExample.Class1();
                    var response = test.Main();

                    response.Wait();
                    return true;
                case "2":
                    var message = class1.GetObject(new Message(), typeof(Message));
                    var result = FirebaseMessaging.DefaultInstance.SendAsync(message);

                    result.Wait();
                    return true;
                case "3":
                    TypeEntryConsole.Execute();
                    return true;
            }

            return false;
        }
    }
}
