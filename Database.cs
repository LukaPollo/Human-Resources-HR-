using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
            List<Employee> list = CsvHelper.ReadEmployees("C:\\Users\\podluk956\\Downloads\\Human-Resources-HR-\\employees.csv");
            Employee emp = list[0];

            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                conn.Open();

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

                string createEmployeeTable = "CREATE TABLE IF NOT EXISTS Employee (Id INT AUTO_INCREMENT PRIMARY KEY, FirstName VARCHAR(255), LastName VARCHAR (255) ,GrossWage DECIMAL(10,2), NetWage DECIMAL(10,2), JobPositionID INT, DepartmentsID INT, CONSTRAINT FK_Employee_JobPositionID FOREIGN KEY (JobPositionID) REFERENCES JobPosition(Id), CONSTRAINT FK_Employee_DepartmentsID FOREIGN KEY (DepartmentsID) REFERENCES Departments(Id))";
                using (MySqlCommand cmd = new MySqlCommand(createEmployeeTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                string createHistoryTable = "CREATE TABLE IF NOT EXISTS History (Id INT AUTO_INCREMENT PRIMARY KEY, BeginTime DATE, EndTime DATE, EmployeeId INT ,CONSTRAINT FK_Employee_EmployeeId FOREIGN KEY (EmployeeId) REFERENCES Employee(Id))";
                using (MySqlCommand cmd = new MySqlCommand(createHistoryTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                string insertDepartment = "INSERT INTO Departments (DepartmentName) VALUES (@DepartmentName)";
                using (MySqlCommand cmd = new MySqlCommand(insertDepartment, conn))
                {
                    var departmentposdist = list.Select(departmentpos => departmentpos.Department).Distinct();
                    foreach (var item in departmentposdist)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@DepartmentName", item);
                        cmd.ExecuteNonQuery();
                    }
                }

                string insertJobPosition = "INSERT INTO JobPosition (JobName) VALUES (@JobName)";
                using (MySqlCommand cmd = new MySqlCommand(insertJobPosition, conn))
                {
                    var jobposdist = list.Select(jobpos => jobpos.JobTitle).Distinct().OrderBy(jobpos => jobpos);
                    foreach (var item in jobposdist)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@JobName", item);
                        cmd.ExecuteNonQuery();
                    }
                }

                string insertEmployee = "INSERT INTO Employee (FirstName, Lastname, GrossWage, NetWage, JobPositionID, DepartmentsID ) VALUES (@FirstName, @LastName, @GrossWage, @NetWage, @JobPositionID, @DepartmentsID)";
                using (MySqlCommand cmd = new MySqlCommand(insertEmployee, conn))
                {
                    string selectQuery = "SELECT Id FROM JobPosition WHERE JobName = @JobTitle";
                    string selectQueryDept = "SELECT Id FROM Departments WHERE DepartmentName = @DepartmentTitle";
                    for (int i = 0; i <= list.Count -1; i++)
                    {
                        cmd.Parameters.Clear();
                        int jobtitleid;
                        using(MySqlCommand cmd2 = new MySqlCommand(selectQuery, conn))
                        {
                            cmd2.Parameters.Add("@JobTitle", MySqlDbType.VarChar).Value = list[i].JobTitle;
                            jobtitleid = (int)cmd2.ExecuteScalar();
                        }
                        int departmentid;
                        using(MySqlCommand cmd3 = new MySqlCommand(selectQueryDept, conn))
                        {
                            cmd3.Parameters.Add("@DepartmentTitle", MySqlDbType.VarChar).Value = list[i].Department;
                            departmentid = (int)cmd3.ExecuteScalar();
                        }

                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@FirstName", MySqlDbType.VarChar).Value = list[i].FirstName;
                        cmd.Parameters.Add("@LastName", MySqlDbType.VarChar).Value = list[i].LastName;
                        cmd.Parameters.Add("@GrossWage", MySqlDbType.Decimal).Value = list[i].GrossWage;
                        cmd.Parameters.Add("@NetWage", MySqlDbType.Decimal).Value = list[i].NetWage;
                        cmd.Parameters.Add("@JobPositionId", MySqlDbType.Int32).Value = jobtitleid;
                        cmd.Parameters.Add("@DepartmentsID", MySqlDbType.Int32).Value = departmentid;
                        
                        
                        cmd.ExecuteNonQuery();
                    }
                }

                string insertHistory = "INSERT INTO History (BeginTime, EndTime, EmployeeId) VALUES (@BeginTime, @EndTime, @EmployeeId)";
                using (MySqlCommand cmd = new MySqlCommand(insertHistory, conn))
                {
                    var historyemployeeid = list.Select(historyemployeeidpos=> historyemployeeidpos).Distinct().OrderBy(historyemployeeidpos=> historyemployeeidpos);
                    string selectEmployee = "SELECT Employee.Id FROM Employee JOIN JobPosition ON JobPositionId = JobPosition.Id WHERE FirstName = @FN AND LastName = @LN AND GrossWage = @GW AND JobPosition.JobName = @JN";

                    int EmpID;
                    for (int i = 0; i < list.Count -1; i++)
                    {
                        using (MySqlCommand cmd2 = new MySqlCommand(selectEmployee, conn))
                        {
                            cmd2.Parameters.AddWithValue("@FN", list[i].FirstName);
                            cmd2.Parameters.AddWithValue("@LN", list[i].LastName);
                            cmd2.Parameters.AddWithValue("@GW", list[i].GrossWage);
                            cmd2.Parameters.AddWithValue("@JN", list[i].JobTitle);
                            EmpID = (int)cmd2.ExecuteScalar();
                        }


                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@BeginTime", MySqlDbType.DateTime).Value = list[i].BeginDate;
                        cmd.Parameters.Add("@EndTime", MySqlDbType.DateTime).Value = list[i].EndDate;
                        cmd.Parameters.AddWithValue("@EmployeeId", EmpID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}