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

            using (MySqlConnection conn = new MySqlConnection("server=localhost;port=3307;database=humanresources;uid=root"))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
                {
                    var reader = cmd.ExecuteReader();
                    var table = new DataTable();
                    table.Load(reader);

                    dataGridView1.DataSource = table;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridView1.MultiSelect = false;
                }
                dataGridView1.CellClick -= dataGridView1_CellClick;
                dataGridView1.CellClick += dataGridView1_CellClick;
                dataGridView1.Rows[0].Selected = true;
                LoadLabels();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadLabels();
        }

        private void LoadLabels()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;port=3307;database=humanresources;uid=root"))
            {
                conn.Open();

                string selectEmployee = "SELECT JobName FROM JobPosition JOIN Employee ON JobPositionId = JobPosition.Id WHERE FirstName = @FN AND LastName = @LN AND GrossWage = @GW";

                using (MySqlCommand cmd = new MySqlCommand(selectEmployee, conn))
                {
                    cmd.Parameters.AddWithValue("@FN", dataGridView1.CurrentRow.Cells["FirstName"].Value);
                    cmd.Parameters.AddWithValue("@LN", dataGridView1.CurrentRow.Cells["Lastname"].Value);
                    cmd.Parameters.AddWithValue("@GW", dataGridView1.CurrentRow.Cells["GrossWage"].Value);

                    label1.Text = cmd.ExecuteScalar().ToString();
                }

                string selectDepartment = "SELECT DepartmentName FROM Departments JOIN Employee ON DepartmentsID = Departments.Id WHERE FirstName = @FN AND LastName = @LN AND GrossWage = @GW";

                using (MySqlCommand cmd = new MySqlCommand(selectDepartment, conn))
                {
                    cmd.Parameters.AddWithValue("@FN", dataGridView1.CurrentRow.Cells["FirstName"].Value);
                    cmd.Parameters.AddWithValue("@LN", dataGridView1.CurrentRow.Cells["Lastname"].Value);
                    cmd.Parameters.AddWithValue("@GW", dataGridView1.CurrentRow.Cells["GrossWage"].Value);

                    label2.Text = cmd.ExecuteScalar().ToString();
                }
            }
        }
    }
}
