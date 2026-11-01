using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using EmployeeManagementApi.Models;

namespace EmployeeManagementApi.Services;

public class EmployeeService
{
    private readonly IConfiguration _configuration;
    private readonly string? _connectionString;

    public EmployeeService(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    public void AddEmployee(Employee employee)
    {
        using var connection = new SqlConnection(_connectionString);  // creating connection to database using connection string from appsettings.json
        connection.Open();

        using var command = new SqlCommand("usp_AddNewEmp", connection);   // same as L.Model we create object and map each field to store value
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Name", employee.Name);
        command.Parameters.AddWithValue("@Email", employee.Email);
        command.Parameters.AddWithValue("@Department", employee.Department);
        command.Parameters.AddWithValue("@Salary", employee.Salary);
        command.Parameters.AddWithValue("@IS_ACTIVE", employee.IsActive);
        command.ExecuteNonQuery();
    }

    public List<Employee> GetAllEmployees()
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("sp_GetAllEmployees", connection);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        var employees = new List<Employee>();
        using var reader = command.ExecuteReader();
        // return employees;

        while(reader.Read())
        {
            var employee = new Employee
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Email = reader.GetString(reader.GetOrdinal("email")),
                Department = reader.GetString(reader.GetOrdinal("department")),
                Salary = reader.GetDecimal(reader.GetOrdinal("salary")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("is_active"))
            };
            employees.Add(employee);
        }
        return employees;
    }

    public Employee? GetEmployeeById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("usp_GetEmpById", connection);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@EmployeeId", id);
        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Employee
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Email = reader.GetString(reader.GetOrdinal("email")),
                Department = reader.GetString(reader.GetOrdinal("department")),
                Salary = reader.GetDecimal(reader.GetOrdinal("salary")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("is_active"))
            };
        }

        return null;
    }

    public void UpdateEmployee(Employee employee, int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("usp_UpdateEmp", connection);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Name", employee.Name);
        command.Parameters.AddWithValue("@Email", employee.Email);
        command.Parameters.AddWithValue("@Department", employee.Department);
        command.Parameters.AddWithValue("@Salary", employee.Salary);
        command.Parameters.AddWithValue("@IS_ACTIVE", employee.IsActive);
        command.Parameters.AddWithValue("@EmployeeId", id);
        command.ExecuteNonQuery();
    }


    public void PatchEmployee(EmployeePatchRequest employee, int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("usp_PatchEmp", connection);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@EmployeeId", id);
        command.Parameters.AddWithValue("@Name", (object?)employee.Name ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email", (object?)employee.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("@Department", (object?)employee.Department ?? DBNull.Value);
        command.Parameters.AddWithValue("@Salary", (object?)employee.Salary ?? DBNull.Value);
        command.Parameters.AddWithValue("@IsActive", (object?)employee.IsActive ?? DBNull.Value);
        command.ExecuteNonQuery();
    }

    public void DeleteEmployee(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("usp_DeleteEmp", connection);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@EmployeeId", id);
        command.ExecuteNonQuery();
    }
}