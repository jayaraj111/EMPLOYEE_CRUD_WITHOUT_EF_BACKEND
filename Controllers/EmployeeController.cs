using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace employeeSampleBackEndSQL.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    
    private readonly ILogger<WeatherForecastController> _logger;
     string constr = @"Server=(localdb)\MSSQLLocalDB;Database=EmployeeCrud;Integrated Security=true;";


    public EmployeeController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

   
  [HttpGet("GetEmployee")]
     public List<Employee> GetEmployees()
    {
        List<Employee> employees = new List<Employee>();
      
        using (SqlConnection con = new SqlConnection(constr))
        {
            string query = "select * from Employee";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        employees.Add(new Employee
                        {
                            EmployeeId = int.Parse(sdr["EmployeeId"].ToString()),
                            EmployeeName = sdr["EmployeeName"].ToString(),
                            Department = sdr["Department"].ToString(),
                            DateOfJoining = DateTime.Parse(sdr["DateOfJoining"].ToString()),
                            PhotoFileName = sdr["PhotoFileName"].ToString()
                        });
                    }
                }
                con.Close();
            }
        }
 
        return employees;
    }


    [HttpPost]
    public IActionResult CreateEmployee([FromBody] Employee emp)
    {
        using (SqlConnection con = new SqlConnection(constr))
        {
            string query = "insert into Employee values (@EmployeeName,@Department,@DateOfJoining,@PhotoFileName)";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                cmd.Parameters.AddWithValue("@Department", emp.Department);
                cmd.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                cmd.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return Ok();
                }
            }
        }

        return BadRequest();
    }

    [HttpPut]
    [Route("{employeeId:int}")]
    public IActionResult updateEmployee([FromRoute] int employeeId, [FromBody] Employee emp)
    {
        using (SqlConnection con = new SqlConnection(constr))
        {
            string query = "update  Employee set EmployeeName= @EmployeeName,Department=@Department,DateOfJoining=@DateOfJoining,PhotoFileName=@PhotoFileName  where EmployeeId = @EmployeeId ";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Connection = con;
               cmd.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                cmd.Parameters.AddWithValue("@Department", emp.Department);
                cmd.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                cmd.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return Ok();
                }
            }
        }

        return NotFound("Employee Not Found");
    }

    [HttpDelete]
    [Route("{employeeId:int}")]
    public IActionResult deleteEmployee([FromRoute] int employeeId)
    {
        using (SqlConnection con = new SqlConnection(constr))
        {
             string query = "Delete FROM Employee where EmployeeId='" + employeeId + "'";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Connection = con;
                
                con.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return Ok();
                }
            }
        }

        return NotFound("Employee Not Found");

    }







}
