using System;
using System.Collections.Generic;
using System.Linq;
using PhoneApp.Domain;
using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Interfaces;

namespace EmployeesExtraPlugin
{
    [Author(Name = "Daria")]

    public class Plugin : IPluggable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> args)
        {
            logger.Info("Starting Viewer");
            logger.Info("Type q or quit to exit");
            logger.Info("Available commands: list, add, del, sear, rand");

            var employeesList = args.Cast<EmployeesDTO>().ToList();

            int index;
            string command = "";

            while (!(command.ToLower().Contains("quit") || command.ToLower().Contains("q")))
            {
                Console.Write("> ");
                command = Console.ReadLine();

                switch (command)
                {
                    case "list":
                        index = 0;
                        foreach (var employee in employeesList)
                        {
                            Console.WriteLine($"{index} Name: {employee.Name} | Phone: {employee.Phone}");
                            ++index;
                        }
                        break;

                    case "add":
                        Console.Write("Name: ");
                        string name = Console.ReadLine();

                        if (string.IsNullOrEmpty(name))
                        {
                            Console.WriteLine("The name should not be empty");
                        }
                        else
                        {
                            Console.Write("Phone: ");

                            string phone = Console.ReadLine();

                            if (!int.TryParse(phone, out index))
                                Console.WriteLine("The phone should be number");
                            else
                            {
                                Console.WriteLine($"{name} added to employees");

                                EmployeesDTO employees = new EmployeesDTO();
                                employees.Name = name;
                                employees.AddPhone(phone);

                                employeesList.Add(employees);
                            }
                        }
                        break;

                    case "del":
                        Console.Write("Index of employee to delete: ");

                        int indexToDelete;

                        if (!Int32.TryParse(Console.ReadLine(), out indexToDelete))
                        {
                            logger.Error("Not an index or not an int value!");
                        }
                        else
                        {
                            if (indexToDelete >= 0 && indexToDelete < employeesList.Count())
                            {
                                employeesList.RemoveAt(indexToDelete);
                            }
                        }
                        break;

                    case "sear":
                        Console.Write("Enter the character(s) to search for: ");

                        string search = Console.ReadLine();

                        index = 0;
                        foreach (var employee in employeesList)
                            if (employee.Name.ToLower().Contains(search))
                            {
                                Console.WriteLine($"{index} Name: {employee.Name} | Phone: {employee.Phone}");
                                ++index;
                            }
                        break;

                    case "rand":
                        Random random = new Random();
                        int randId = random.Next(0, employeesList.Count());

                        Console.WriteLine($"Name: {employeesList[randId].Name} | Phone: {employeesList[randId].Phone}");
                        break;
                }

                Console.WriteLine("");
            }

            return employeesList.Cast<DataTransferObject>();
        }
    }
}
