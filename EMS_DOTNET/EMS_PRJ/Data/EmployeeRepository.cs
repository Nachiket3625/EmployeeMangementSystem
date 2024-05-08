using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Data
{
    public class EmployeeRepository : IRepository
    {
        private readonly string connectionString;
        public EmployeeRepository()
        {
            connectionString = "Server=localhost\\SQLEXPRESS;Database=EMS;Trusted_Connection=True;TrustServerCertificate=true;";
        }



        public bool Add(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Employees (Id, FirstName, LastName, DateOfBirth, DateOfJoining, Gender, JobTitle, Email, MobileNo, Password) " +
                               "VALUES (@Id, @FirstName, @LastName, @DateOfBirth, @DateOfJoining, @Gender, @JobTitle, @Email, @MobileNo, @Password)";

                
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", employee.Id);
                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                command.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                command.Parameters.AddWithValue("@Gender", employee.GetGenderString());
                command.Parameters.AddWithValue("@JobTitle", employee.GetJobTitleString());
                command.Parameters.AddWithValue("@Email", employee.Email); 
                command.Parameters.AddWithValue("@MobileNo", employee.Phone);
                command.Parameters.AddWithValue("@Password", employee.Password);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        public bool Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Employees WHERE Id = @Id";


                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
                
            }
        }

        public bool Edit(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Employees SET FirstName = @FirstName, LastName = @LastName, " +
                               "DateOfBirth = @DateOfBirth, DateOfJoining = @DateOfJoining, " +
                               "Gender = @Gender, JobTitle = @JobTitle, Email = @Email, MobileNo = @MobileNo WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                command.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                command.Parameters.AddWithValue("@Gender", employee.GetGenderString());
                command.Parameters.AddWithValue("@JobTitle", employee.GetJobTitleString());
                command.Parameters.AddWithValue("@Email", employee.Email);
                command.Parameters.AddWithValue("@Id", employee.Id);
                command.Parameters.AddWithValue("@MobileNo", employee.Phone);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        public IEnumerable<Employee> GetByGender(Gender gender)
        {
            var employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employees WHERE Gender = @Gender";
                string gen = gender == Gender.Male ? "Male" : "Female";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Gender", gen);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var employee = MapEmployeeFromReader(reader);
                    employees.Add(employee);
                }
                reader.Close();
            }
            return employees;
        }

        public Employee GetById(int employeeId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employees WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", employeeId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var employee = MapEmployeeFromReader(reader);
                    return employee;
                }
                reader.Close();
            }
            return null;
        }

        public IEnumerable<Employee> GetByJobTitle(JobTitle jobTitle)
        {
            var employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employees WHERE JobTitle = @JobTitle";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@JobTitle", jobTitle.ToString());

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var employee = MapEmployeeFromReader(reader);
                    employees.Add(employee);
                }
                reader.Close();
            }
            return employees;
        }

        public IEnumerable<Employee> GetEmployees()
        {
            var employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employees";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var employee = MapEmployeeFromReader(reader);
                    employees.Add(employee);
                }
                reader.Close();
            }
            return employees;
        }

        private Employee MapEmployeeFromReader(SqlDataReader reader)
        {

            _ = Enum.TryParse((string)reader["Gender"], out Gender gender);

            _ = Enum.TryParse((string)reader["JobTitle"], out JobTitle jobTitle);

            return new Employee
            {
                Id = (int)reader["Id"],
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                DateOfBirth = (DateTime)reader["DateOfBirth"],
                DateOfJoining = (DateTime)reader["DateOfJoining"],
                Gender = gender,
                JobTitle = jobTitle,
                Email = (string)reader["Email"],
                Phone = long.Parse((string)reader["MobileNo"]),
                Password = (string)reader["Password"]
            };
        }
    }
}
