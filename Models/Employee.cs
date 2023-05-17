using System.ComponentModel.DataAnnotations;

namespace employeeSampleBackEndSQL;

public class Employee
{
    [Key]
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? Department { get; set; }
    public DateTime DateOfJoining { get; set; }
    public string? PhotoFileName { get; set; }

}
