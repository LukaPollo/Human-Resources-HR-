using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MySql.Data.MySqlClient;


namespace Human_Resources__HR_
{
    internal class Database
    {
        //
        //People (Firtname, Lastname, id),
        //Department(id, departmentname),
        //JobPosition(name, id),
        //employee(grosswage, netwage, jobpos id, department id, people id, id),
        ///hisytory(people id, begin, end)
        //

        public static void Database1(string connection)
        {
            List<Employee> list = CsvHelper.ReadEmployees("C:\\Users\\explo\\Downloads\\Human-Resources-HR-\\employees.csv");
            Employee emp = list[0];

            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                conn.Open();

                string createPeopleTable = "CREATE TABLE IF NOT EXISTS People (Id INT AUTO_INCREMENT PRIMARY KEY, FirstName VARCHAR(255), LastName VARCHAR(255))";
                using (MySqlCommand cmd = new MySqlCommand(createPeopleTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                string createDepartmentTable = "CREATE TABLE IF NOT EXISTS Departments (Id INT AUTO_INCREMENT PRIMARY KEY, DepartmentName VARCHAR(255))";
                using (MySqlCommand cmd = new MySqlCommand(createDepartmentTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                string createJobPositionTable = "CREATE TABLE IF NOT EXISTS JobPosition (Id INT AUTO_INCREMENT PRIMARY KEY, JobName VARCHAR(255))";
                using (MySqlCommand cmd = new MySqlCommand(createJobPositionTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                string createEmployeeTable = "CREATE TABLE IF NOT EXISTS Employee (Id INT AUTO_INCREMENT PRIMARY KEY, GrossWage DECIMAL(10,2), NetWage DECIMAL(10,2), JobPositionID INT, PeopleID INT, DepartmentsID INT, CONSTRAINT FK_Employee_JobPositionID FOREIGN KEY (JobPositionID) REFERENCES JobPosition(Id), CONSTRAINT FK_Employee_PeopleID FOREIGN KEY (PeopleID) REFERENCES People(Id), CONSTRAINT FK_Employee_DepartmentsID FOREIGN KEY (DepartmentsID) REFERENCES Departments(Id))";
                using (MySqlCommand cmd = new MySqlCommand(createEmployeeTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                string createHistoryTable = "CREATE TABLE IF NOT EXISTS History (Id INT AUTO_INCREMENT PRIMARY KEY, BeginTime DATETIME, EndTime DATETIME, PeopleID INT, CONSTRAINT FK_History_PeopleID FOREIGN KEY (PeopleID) REFERENCES People(Id))";
                using (MySqlCommand cmd = new MySqlCommand(createHistoryTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                for (int x = 0; x < list.Count; x++)
                {
                    string insertPeople = "INSERT INTO People (FirstName, LastName) VALUES (@FirstName, @LastName)";
                    using (MySqlCommand cmd = new MySqlCommand(insertPeople, conn))
                    {
                        cmd.Parameters.Add("@FirstName", MySqlDbType.VarChar).Value = list[x].FirstName;
                        cmd.Parameters.Add("@LastName", MySqlDbType.VarChar).Value = list[x].LastName;
                        cmd.ExecuteNonQuery();
                    }

                    string insertDepartment = "INSERT INTO Departments (DepartmentName) VALUES (@DepartmentName)";
                    using (MySqlCommand cmd = new MySqlCommand(insertDepartment, conn))
                    {
                        cmd.Parameters.Add("@DepartmentName", MySqlDbType.VarChar).Value = list[x].Department;
                        cmd.ExecuteNonQuery();
                    }

                    string insertJobPosition = "INSERT INTO JobPosition (JobName) VALUES (@JobName)";
                    using (MySqlCommand cmd = new MySqlCommand(insertJobPosition, conn))
                    {
                        cmd.Parameters.Add("@JobName", MySqlDbType.VarChar).Value = list[x].JobTitle;
                        cmd.ExecuteNonQuery();
                    }

                    string insertEmployee = "INSERT INTO Employee (GrossWage, NetWage) VALUES (@GrossWage, @NetWage)";
                    using (MySqlCommand cmd = new MySqlCommand(insertEmployee, conn))
                    {
                        cmd.Parameters.Add("@GrossWage", MySqlDbType.Decimal).Value = list[x].GrossWage;
                        cmd.Parameters.Add("@NetWage", MySqlDbType.Decimal).Value = list[x].NetWage;
                        cmd.ExecuteNonQuery();
                    }

                    string insertHistory = "INSERT INTO History (BeginTime, EndTime) VALUES (@BeginTime, @EndTime)";
                    using (MySqlCommand cmd = new MySqlCommand(insertHistory, conn))
                    {
                        cmd.Parameters.Add("@BeginTime", MySqlDbType.DateTime).Value = list[x].BeginDate;
                        cmd.Parameters.Add("@EndTime", MySqlDbType.DateTime).Value = list[x].EndDate;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}