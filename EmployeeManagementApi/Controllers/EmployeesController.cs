using Microsoft.AspNetCore.Mvc;
using EmployeeManagementApi.Services;
using EmployeeManagementApi.Models;

namespace EmployeeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase   // -- inehrit from controllerBase coz we are creating apis
{
    private readonly EmployeeService _employeeService; //calling to service file and use it in methods to operate on databse

    public EmployeesController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]  // get method same as we mentioned in laravel routing
    public IActionResult GetEmployees()
    {
        var employees = _employeeService.GetAllEmployees();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public IActionResult GetEmployeeById(int id)
    {
        var employee = _employeeService.GetEmployeeById(id);

        if (employee == null)
            return NotFound("Employee not found.");

        return Ok(employee);
    }

    [HttpPost]  // -- post method same we mentioned laravel routing
    public IActionResult CreateEmployee(Employee employee)
    {
        _employeeService.AddEmployee(employee);

        return Ok("Employee created successfully.");
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEmployee(int id, Employee employee)
    {
        _employeeService.UpdateEmployee(employee,id);
        return Ok("Employee updated successfully.");
    }

    [HttpPatch("{id}")]   // -- partially update fields
    public IActionResult PatchEmployee(int id, EmployeePatchRequest employee)
    {
        _employeeService.PatchEmployee(employee, id);
        return Ok("Employee fields updated successfully.");
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEmployee(int id)
    {
        _employeeService.DeleteEmployee(id);
        return Ok("Employee deleted successfully.");
    }
}