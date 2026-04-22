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

            string filePath = "H:\\Visual studio 2022\\Human Resources (HR)\\employees.csv";

            if (!File.Exists(filePath))
                return employees;

            using (var reader = new StreamReader(filePath))
            {
                string header = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    string row = reader.ReadLine();
                    var splittedrow = row.Split(',');


                    var emp = new Employee
                    {
                        FirstName = splittedrow[0].Trim(),
                        LastName = splittedrow[1].Trim(),
                        GrossWage = decimal.Parse(splittedrow[2].Trim()),
                        NetWage = decimal.Parse(splittedrow[3].Trim()),
                        BeginDate = DateTime.Parse(splittedrow[4].Trim()),
                        EndDate = DateTime.Parse(splittedrow[5].Trim()),
                        JobTitle = splittedrow[6].Trim(),
                        Department = splittedrow[7].Trim()
                    };

                    employees.Add(emp);
                }
            }

            return employees;
        }
    }
}
