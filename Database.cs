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
            List<Employee> list = CsvHelper.ReadEmployees("employees.csv");
            using (MySqlConnection conn = new MySqlConnection())
            {

                string createTableQuery = "CREATE TABLE IF NOT EXISTS People (Id INT AUTO_INCREMENT PRIMARY KEY, Firstname VARCHAR(255), LastName VARCHAR(255))";
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(createTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                string insertQuery = "INSERT INTO People (FirstName, LastName) VALUES (@FirstName, @LastName);";
                using (MySqlCommand cmd2 = new MySqlCommand(insertQuery, conn))
                {
                    cmd2.Parameters.Clear();
                    cmd2.Parameters.Add("@FirstName", MySqlDbType.VarChar, 255).Value = list[0];
                    cmd2.Parameters.Add("@LastName", MySqlDbType.VarChar, 255).Value = list[1];
                    cmd2.ExecuteNonQuery();
                }

                string createDepartmentTable = "CREATE TABLE IF NOT EXISTS Departments (Id INT AUTO_INCREMENT PRIMARY KEY, DepartmentName VARCHAR(255));";
                using (MySqlCommand cmd3 = new MySqlCommand(createDepartmentTable, conn))
                {
                    cmd3.ExecuteNonQuery();
                }
                string insertDepartmentQuery = "INSERT INTO Departments (DepartmentName) VALUES (@DepartmentName);";
                using (MySqlCommand cmd4 = new MySqlCommand(insertDepartmentQuery, conn))
                {
                    cmd4.Parameters.Clear();
                    cmd4.Parameters.Add("@DepartmentName", MySqlDbType.VarChar, 255).Value = list[5];
                    cmd4.ExecuteNonQuery();
                }
                string createJobPositionQuery = "CREATE TABLE IF NOT EXISTS JobPosition (Id INT AUTO_INCREMENT PRIMARY KEY, JobName VARCHAR(255));";
                using (MySqlCommand cmd = new MySqlCommand(createJobPositionQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                string insertJobPosition = "INSERT INTO JobPosition (JobName) VALUES (@JobName);";
                using (MySqlCommand cmd = new MySqlCommand(insertJobPosition, conn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@JobName", MySqlDbType.VarChar, 255).Value = list[4];
                    cmd.ExecuteNonQuery();
                }

                string createEmployeeTable = "CREATE TABLE IF NOT EXISTS Employee (Id INT AUTO_INCREMENT PRIMARY KEY, GrossWage INT, NetWage INT, JobPositionID INT, PeopleID INT, DepartmentsID INT, CONSTRAINTS FK_Employee_JobPositionID FOREIGN KEY (JobPositionID) REFERENCES JobPosition (Id), CONTRAINTS FK_Employee_PeopleID FOREIGN KEY (PeopleID) REFERENCES People (Id), CONSTRAINTS FK_Employee_DepartmentsID FOREIGN KEY (DepartmentsId) REFERENCES Departments (Id));";
                using(MySqlCommand cmd = new MySqlCommand(createEmployeeTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                string insertEmployeeQuery = "INSERT INTO Employee (GrossWage, NetWage) VALUES (@GrossWage, @NetWage);";
                using (MySqlCommand cmd = new MySqlCommand(insertEmployeeQuery, conn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@GrossWage", MySqlDbType.VarChar, 255).Value = list[2];
                    cmd.Parameters.Add("@NetWage", MySqlDbType.VarChar, 255).Value=list[3];
                    cmd.ExecuteNonQuery();
                }
                string createHistoryTable = "CREATE TABLE IF NOT EXISTS History (Id INT AUTO_INCREMENT PRIMARY KEY, BeginTime DateTime, EndTime DateTime);";
                using (MySqlCommand cmd = new MySqlCommand(createHistoryTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                string insertHistoryQuery = "INSERT INTO History (BeginTime, EndTime) VALUES (@BeginTime, @Endtime);";
                using (MySqlCommand cmd = new MySqlCommand(insertHistoryQuery, conn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@BeginTime", MySqlDbType.VarChar, 255).Value = list[6];
                    cmd.Parameters.Add("@EndTime", MySqlDbType.VarChar, 255).Value = list[7];
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
