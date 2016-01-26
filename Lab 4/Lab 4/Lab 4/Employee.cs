/*
 * Cameron Winters, 000299896 
 * March 20, 2015
 * 
 * Program Purpose: the purpose of this program is to allow a user to access employee record information
 * and sort the information 5 different ways.  They may sort by Employee Name (asc), Number (asc), Pay
 * Rate (desc), Hours (desc), or Gross Pay (desc).  The employee records are contained in a CSV file, and up
 * to 100 employees may be read by the program.
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

namespace Lab_4
{
    /// <summary>
    /// Employee class contains the information for an individual employee, including:
    /// Name, Employee Number, Hours Worked, Rate of Pay, and Gross Pay.  These values can
    /// be updated, accessed, or all information can be printed to output stream via 
    /// PrintEmployee method.
    /// </summary>
    class Employee : IComparable<Employee>
    {
        private decimal rate;
        private decimal gross;
        private double hours;
        private int number;
        private string name;

        /// <summary>
        /// Constructor for the Employee class.
        /// </summary>
        /// <param name="name">The name of the employee.</param>
        /// <param name="number">The employee number.</param>
        /// <param name="rate">The employee's rate of pay.</param>
        /// <param name="hours">The number of hours the employee has worked.</param>
        public Employee(string name, int number, decimal rate, double hours)
        {
            this.name = name;
            this.number = number;
            this.rate = rate;
            this.hours = hours;
            CalcGross();
        }
        
        /// <summary>
        /// Gross pay property for the employee.
        /// </summary>
        public decimal Gross
        {
            get { return gross; }
            set { gross = value; }
        }

        /// <summary>
        /// Hours property for the employee.
        /// </summary>
        public double Hours
        {
            get { return hours; }
            set { hours = value; CalcGross(); }
        }

        /// <summary>
        /// Name property of the employee.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Employee number property of the employee.
        /// </summary>
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        /// <summary>
        /// Hourly rate property of the employee.
        /// </summary>
        public decimal Rate
        {
            get { return rate; }
            set { rate = value; CalcGross(); }
        }

        /// <summary>
        /// Calculate gross pay; time and a half after 40 hours.
        /// </summary>
        private void CalcGross()
        {
            if (hours <= 40)
            {
                gross = rate * (decimal)hours;
            }
            else
            {
                double bonus;
                bonus = ((hours - 40) * 0.5) + (hours - 40);
                gross = (rate * 40) + (rate * (decimal)bonus);
            }
        }

        /// <summary>
        /// Prints employee information to the standard output stream.
        /// </summary>
        public void PrintEmployee()
        {
            System.Console.WriteLine("Name: " + name);
            System.Console.WriteLine("Number: " + number);
            System.Console.WriteLine("Rate of Pay: " + rate);
            System.Console.WriteLine("Hours: " + hours);
            System.Console.WriteLine("Gross Pay: " + gross);
        }

        /// <summary>
        /// Implemented CompareTo method from the IComparable interface.  Compares values between employee
        /// objects.
        /// </summary>
        /// <param name="emp">The other employee object.</param>
        /// <returns>The result of the comparison.  An integer.</returns>
        public int CompareTo(Employee emp)
        {
            if (emp == null)
            {
                return 1;
            }

            int result = 0;

            // Compare names
            result = this.name.CompareTo(emp.name);
            if (result != 0)
            {
                return result;
            }

            // Compare employee numbers
            result = this.number.CompareTo(emp.number);
            if (result != 0)
            {
                return result;
            }

            // Compare pay rates
            result = emp.rate.CompareTo(this.rate);
            if (result != 0)
            {
                return result;
            }

            // Compare hours worked
            result = emp.hours.CompareTo(this.hours);
            if (result != 0)
            {
                return result;
            }

            // Compare gross pay
            result = emp.gross.CompareTo(this.gross);
            if (result != 0)
            {
                return result;
            }
            return result;
        }
    }
}
