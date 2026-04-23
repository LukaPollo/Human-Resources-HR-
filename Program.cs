using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources__HR_
{
    internal class Program
    {

        static void Main(string[] args)
        {
            string dbPath = "server=localhost;port=3307;database=humanresources2;uid=root";
            List<Employee> list = CsvHelper.ReadEmployees("C:\\Users\\explo\\Documents\\Human-Resources-HR-\\employees.csv");
            Database.Database1(dbPath);
        }
    }
}
