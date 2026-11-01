namespace EmployeeManagementApi.DTOs;

public class ApiResponseDto<T>
{
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}