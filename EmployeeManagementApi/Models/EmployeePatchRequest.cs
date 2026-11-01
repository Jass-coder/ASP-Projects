namespace EmployeeManagementApi.Models;

public class EmployeePatchRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Department { get; set; }
    public decimal? Salary { get; set; }
    public bool? IsActive { get; set; }
}