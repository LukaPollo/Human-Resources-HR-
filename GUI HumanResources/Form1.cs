using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GUI_HumanResources
{

    // DVG button under it 
    // onclick load all employee data (only show the data with value)
    // foreign keyhez egy panel ami megmutatja hogy mi


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            string selectQuery = "SELECT FirstName, Lastname, GrossWage, NetWage FROM Employee";
            using (MySqlConnection conn = new MySqlConnection("server=localhost;port=3307;database=humanresources2;uid=root"))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                {
                    var reader = cmd.ExecuteReader();
                    var table = new DataTable();
                    table.Load(reader);
                    dataGridView1.DataSource = table;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }
                string selectEmployee = "SELECT JobName FROM JobPosition JOIN Employee ON JobPositionId = JobPosition.Id WHERE FirstName = @FN AND LastName = @LN AND GrossWage = @GW AND JobPosition.JobName = @JN";

                using (MySqlCommand cmd = new MySqlCommand(selectEmployee, conn)
                {

                }
            }
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
