/*
 * Cameron Winters, 000299896 
 * March 20, 2015
 * 
 * Program Purpose: the purpose of this program is to allow a user to access employee record information
 * and sort the information 5 different ways.  They may sort by Employee Name (asc), Number (asc), Pay
 * Rate (desc), Hours (desc), or Gross Pay (desc).  The employee records are contained in a CSV file, and up
 * to 100 employees may be read by the program.
 * 
 * This program has been modified from my Lab 1 program.
 * 
 * Statement of Authorship: I, Cameron Winters, 000299896 certify that this material is my original work. No other 
 * person's work has been used without due acknowledgement.
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab_4
{
    /// <summary>
    /// The primary class for the Lab4 program.  This class accesses the employee records CSV file and creates 
    /// a generic list of Employee objects.  The user may choose to sort by Employee Name, Number, Pay Rate, Hours, or
    /// Gross Pay, or they may choose to exit the application.
    /// 
    /// Exit Codes 
    ///    0: clean exit
    ///    1: read error - employees.txt file missing or altered
    /// </summary>
    class Program
    {
        // Class variables
        static List<Employee> employees = new List<Employee>();

        /// <summary>
        /// The main class first reads through the Employee CSV file, then calls the Prompt method to gather user input.
        /// </summary>
        /// <param name="args">The program arguments.</param>
        static void Main(string[] args)
        {
            // Populate employees list
            Read();

            // Prompt user for input
            Prompt();
        }

        /// <summary>
        /// This method is used to read data from the Employee CSV file, create new Employee objects for each
        /// employee found in the CSV file, and store each object in the global employees list.
        /// </summary>
        private static void Read()
        {
            try
            {
                FileStream employeeFile = new FileStream(@"..\..\..\..\employees.txt", FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(employeeFile);
                string line;
                int iteration = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(',');
                    string name = values[0];
                    int number = Convert.ToInt32(values[1]);
                    decimal rate = Convert.ToDecimal(values[2]);
                    double hours = Convert.ToDouble(values[3]);
                    Employee emp = new Employee(name, number, rate, hours);
                    employees.Add(emp);

                    iteration++;
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Oops!  Looks like the Employee CSV file has been deleted or moved.  Please place the file ('employees.txt') back into the root directory and run this application again.\n\n" + e + "\n\n");
                System.Console.WriteLine("Program will close in 20 seconds.");
                System.Threading.Thread.Sleep(20000);
                System.Environment.Exit(1);
            }
        }

        /// <summary>
        /// This method is used to obtain the user's selection.  They can choose to receive a sorted table of employee
        /// information, or they may cleanly exit the application.
        /// </summary>
        private static void Prompt()
        {
            bool exitFlag = false;
            int selection = 0;

            Choices(); // Present choices to the user.
            while (!exitFlag)
            {
                selection = 0;
                // Get user's sorting choice
                try
                {
                    while (selection < 1 || selection > 6)
                    {
                        selection = Convert.ToInt32(System.Console.ReadLine());
                        if (selection < 1 || selection > 6)
                        {
                            System.Console.WriteLine("You must make a valid choice between 1 and 6.");
                        }
                    }
                }
                catch (Exception e) { }
                // Call Sort method or exit application
                if (selection > 0 && selection < 7 && selection != 6)
                {
                    Sort(selection);
                }
                else if (selection == 6)
                {
                    exitFlag = true;
                }
                else
                {
                    System.Console.WriteLine("Sorry, your choice must be between 1 and 6.");
                }
            }
            System.Environment.Exit(0);
        }

        /// <summary>
        /// This method is called when the user selects a sorting method.  The employees list is sorted based
        /// on Name (ascending), Number (ascending), Pay Rate (descending), Hours (descending), and 
        /// Gross Pay (descending).
        /// </summary>
        /// <param name="selection">An integer from 1 to 5 representing the user's choice of sorting method.</param>
        private static void Sort(int selection)
        {
            String title = "";

            switch (selection)
            {
                // Employee Name (ascending)
                case 1:
                    employees.Sort(delegate(Employee emp1, Employee emp2)
                    {
                        return emp1.Name.CompareTo(emp2.Name);
                    });
                    title = "Employees sorted by name, ascending:";
                    break;

                // Employee Number (ascending)
                case 2:
                    employees.Sort(delegate(Employee emp1, Employee emp2)
                    {
                        return emp1.Number.CompareTo(emp2.Number);
                    });
                    title = "Employees sorted by number, ascending:";
                    break;

                // Employee Pay Rate (descending)
                case 3:
                    employees.Sort(delegate(Employee emp1, Employee emp2)
                    {
                        return emp2.Rate.CompareTo(emp1.Rate);
                    });
                    title = "Employees sorted by pay rate, descending:";
                    break;

                // Employee Hours (descending)
                case 4:
                    employees.Sort(delegate(Employee emp1, Employee emp2)
                    {
                        return emp2.Hours.CompareTo(emp1.Hours);
                    });
                    title = "Employees sorted by hours, descending:";
                    break;

                // Employee Gross Pay (descending)
                case 5:
                    employees.Sort(delegate(Employee emp1, Employee emp2)
                    {
                        return emp2.Gross.CompareTo(emp1.Gross);
                    });
                    title = "Employees sorted by gross pay, descending:";
                    break;
            }

            // Display list of sorted employees in a neatly formatted "table".
            System.Console.Clear();
            System.Console.WriteLine(title);
            System.Console.WriteLine();
            System.Console.WriteLine("--------------------------------------------------------------------------");
            System.Console.WriteLine(String.Format("{0, 5} | {1, -15} | {2, -15} | {3, -15} | {4, -15}", "Number", "Name", "Hours Worked", "Gross Pay", "Pay Rate"));
            System.Console.WriteLine("--------------------------------------------------------------------------");
            foreach (var emp in employees)
            {
                System.Console.WriteLine(String.Format("{0, 5} | {1, -15} | {2, -15} | {3, -15} | {4, -15}", emp.Number, emp.Name, emp.Hours, emp.Gross, emp.Rate));
            }
            System.Console.WriteLine("--------------------------------------------------------------------------");
            System.Console.WriteLine();

            Choices();
        }

        /// <summary>
        /// Called when the list of options is to be shown to the user.  Writes their input options to the 
        /// console.
        /// </summary>
        private static void Choices()
        {
            System.Console.WriteLine("Make a selection:");
            System.Console.WriteLine();
            System.Console.WriteLine("1. Sort by Employee Name (ascending)");
            System.Console.WriteLine("2. Sort by Employee Number (ascending)");
            System.Console.WriteLine("3. Sort by Employee Pay Rate (descending)");
            System.Console.WriteLine("4. Sort by Employee Hours (descending)");
            System.Console.WriteLine("5. Sort by Employee Gross Pay (descending)");
            System.Console.WriteLine("6. Exit");
        }
    }
}
