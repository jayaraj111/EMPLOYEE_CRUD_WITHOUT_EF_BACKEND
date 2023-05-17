using System.ComponentModel.DataAnnotations;

namespace employeeSampleBackEndSQL;

public class Department
{
    [Key]
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
}
