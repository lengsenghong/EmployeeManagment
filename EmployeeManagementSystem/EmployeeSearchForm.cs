using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EmployeeManagementSystem
{
    public partial class EmployeeSearchForm : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=LENGSENGHOND5FC\SQLEXPRESSHONG;Initial Catalog=employees;Persist Security Info=True;User ID=sa;Password=hong07042004;");

        public EmployeeSearchForm()
        {
            InitializeComponent();
            InitializeFilters();
        }

        private void InitializeFilters()
        {
            positionComboBox.Items.Add("All");
            positionComboBox.Items.Add("Software Engineer");
            positionComboBox.Items.Add("Manager");
            positionComboBox.Items.Add("Developer");
            positionComboBox.SelectedIndex = 0; // Default to "All"

            statusComboBox.Items.Add("All");
            statusComboBox.Items.Add("Active");
            statusComboBox.Items.Add("Inactive");
            statusComboBox.SelectedIndex = 0; // Default to "All"
        }

        public void DisplayEmployeeData()
        {
            string searchQuery = searchTextBox.Text.Trim();
            string positionFilter = positionComboBox.SelectedItem?.ToString();
            string statusFilter = statusComboBox.SelectedItem?.ToString();

            string query = "SELECT employee_id, full_name, gender, contact_number, position, salary, insert_date, status " +
                           "FROM employee WHERE delete_date IS NULL";

            // Apply search query (search by Employee ID or Full Name)
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query += " AND (employee_id LIKE @searchQuery OR full_name LIKE @searchQuery)";
            }

            // Apply position filter
            if (!string.IsNullOrEmpty(positionFilter) && positionFilter != "All")
            {
                query += " AND position = @positionFilter";
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
            {
                query += " AND status = @statusFilter";
            }

            try
            {
                connect.Open();
                SqlCommand cmd = new SqlCommand(query, connect);

                // Add parameters for search and filters
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");
                }

                if (!string.IsNullOrEmpty(positionFilter) && positionFilter != "All")
                {
                    cmd.Parameters.AddWithValue("@positionFilter", positionFilter);
                }

                if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
                {
                    cmd.Parameters.AddWithValue("@statusFilter", statusFilter);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                employeeDataGridView.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connect.Close();
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            DialogResult confirmExit = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmExit == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void UpdateEmployeeForm_Load_1(object sender, EventArgs e)
        {

            DisplayEmployeeData();
        }

        private void EmployeeSearchForm_Load(object sender, EventArgs e)
        {
            DisplayEmployeeData();
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            DisplayEmployeeData();
        }

        private void positionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayEmployeeData();
        }

        private void statusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayEmployeeData();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            DisplayEmployeeData();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {

            // Check if any employee is selected
            if (employeeDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an employee to update.", "No Employee Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the employee ID from the selected row (assumes employee_id is in the first column)
            string selectedEmployeeId = employeeDataGridView.SelectedRows[0].Cells["employee_id"].Value.ToString();

            // Open the UpdateEmployeeForm, passing the selected employee's ID
            UpdateEmployeeForm updateForm = new UpdateEmployeeForm(selectedEmployeeId);
            updateForm.ShowDialog(); // Show the form as a modal

            // After closing the UpdateEmployeeForm, refresh the data to reflect any changes
            DisplayEmployeeData();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Clear();
            positionComboBox.SelectedIndex = 0;
            statusComboBox.SelectedIndex = 0;
            DisplayEmployeeData();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (employeeDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an employee to delete.", "No Employee Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedEmployeeId = employeeDataGridView.SelectedRows[0].Cells["employee_id"].Value.ToString();
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this employee?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                
                
                    connect.Open();
                    string query = "DELETE FROM employee WHERE employee_id = @employeeId";
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@employeeId", selectedEmployeeId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Employee deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisplayEmployeeData();
                    }
                
               
            }
            // Refresh the data grid after updating
            DisplayEmployeeData();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.ShowDialog();
            this.Hide();
        }
    }
}
