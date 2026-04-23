using System;
using System.Collections.Generic;
using System.IO;

namespace Human_Resources__HR_
{
    public class Employee
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public decimal GrossWage { get; set; }
        public decimal NetWage { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
    }

    public class CsvHelper
    {
        public static List<Employee> ReadEmployees(string path)
        {
            var employees = new List<Employee>();

            using (var olvasott = new StreamReader(path))
            {
                olvasott.ReadLine();

                while (!olvasott.EndOfStream)
                {
                    string row = olvasott.ReadLine();
                    string[] splittedrow = row.Split(','); 

                    decimal.TryParse(splittedrow[2], out decimal grossWage);
                    decimal.TryParse(splittedrow[3], out decimal netWage);
                    DateTime.TryParse(splittedrow[6], out DateTime beginDate);
                    DateTime.TryParse(splittedrow[7], out DateTime endDate);

                    var emp = new Employee
                    {
                        FirstName = splittedrow[0],
                        LastName = splittedrow[1],
                        GrossWage = grossWage,
                        NetWage = netWage,
                        JobTitle = splittedrow[4],
                        Department = splittedrow[5],
                        BeginDate = beginDate,
                        EndDate = endDate
                    };

                    employees.Add(emp);
                }
                return employees;
            }
        }
    }
}