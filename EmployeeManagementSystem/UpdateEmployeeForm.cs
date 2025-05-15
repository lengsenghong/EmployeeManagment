using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EmployeeManagementSystem
{
    public partial class UpdateEmployeeForm : Form
    {
        private string employeeId;  // Store the selected employee ID

        // Constructor to receive employee ID (or any other details you need)
        public UpdateEmployeeForm(string employeeId)
        {
            InitializeComponent();
            this.employeeId = employeeId;  // Store the employee ID
        }

        private void UpdateEmployeeForm_Load(object sender, EventArgs e)
        {
            // On form load, populate the employee data for editing
            LoadEmployeeData();
        }

        // Load employee data for the given ID
        private void LoadEmployeeData()
        {
            SqlConnection connect = new SqlConnection(@"Data Source=LENGSENGHOND5FC\SQLEXPRESSHONG;Initial Catalog=employees;Persist Security Info=True;User ID=sa;Password=hong07042004;");

            try
            {
                connect.Open();
                string query = "SELECT full_name, gender, contact_number, position, status FROM employee WHERE employee_id = @employeeId";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@employeeId", employeeId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Populate the form fields with the employee data
                    fullNameTextBox.Text = reader["full_name"].ToString();
                    genderComboBox.SelectedItem = reader["gender"].ToString();
                    phoneNumberTextBox.Text = reader["contact_number"].ToString();
                    positionComboBox.SelectedItem = reader["position"].ToString();
                    statusComboBox.SelectedItem = reader["status"].ToString();
                }
                else
                {
                    MessageBox.Show("Employee not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        // Save the updated employee details
        private void saveButton_Click(object sender, EventArgs e)
        {
            // Get the updated data from the form
            string newFullName = fullNameTextBox.Text;
            string newGender = genderComboBox.SelectedItem.ToString();
            string newPhoneNumber = phoneNumberTextBox.Text;
            string newPosition = positionComboBox.SelectedItem.ToString();
            string newStatus = statusComboBox.SelectedItem.ToString();

            if (string.IsNullOrEmpty(newFullName) || string.IsNullOrEmpty(newPhoneNumber))
            {
                MessageBox.Show("Please fill all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqlConnection connect = new SqlConnection(@"Data Source=LENGSENGHOND5FC\SQLEXPRESSHONG;Initial Catalog=employees;Persist Security Info=True;User ID=sa;Password=hong07042004;");

            try
            {
                connect.Open();
                string query = "UPDATE employee SET full_name = @fullName, gender = @gender, contact_number = @contactNum, position = @position, status = @status WHERE employee_id = @employeeId";

                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@fullName", newFullName);
                cmd.Parameters.AddWithValue("@gender", newGender);
                cmd.Parameters.AddWithValue("@contactNum", newPhoneNumber);
                cmd.Parameters.AddWithValue("@position", newPosition);
                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@employeeId", employeeId);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Employee updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Close the update form after successful update
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

        // Cancel button to close the form without saving
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Go back to EmployeeSearchForm
        private void backButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void UpdateEmployeeForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
