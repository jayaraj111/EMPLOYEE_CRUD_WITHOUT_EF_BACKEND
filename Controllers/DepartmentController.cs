using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace employeeSampleBackEndSQL.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController : ControllerBase
{

    private readonly ILogger<WeatherForecastController> _logger;

    string constr = @"Server=(localdb)\MSSQLLocalDB;Database=EmployeeCrud;Integrated Security=true;";
    private readonly IConfiguration _configuration;




    public DepartmentController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    // string myDb1ConnectionString = _configuration.GetConnectionString("myDb1");



    [HttpGet("GetDepartment")]
    public List<Department> GetDepartments()
    {
        List<Department> departments = new List<Department>();

        string connString = this._configuration.GetConnectionString("sqlconnectionstring");

        using (SqlConnection con = new SqlConnection(connString))
        {
            string query = "select departmentId,departmentName from department";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        departments.Add(new Department
                        {
                            DepartmentId = int.Parse(sdr["DepartmentId"].ToString()),
                            DepartmentName = sdr["DepartmentName"].ToString()
                        });
                    }
                }
                con.Close();
            }
        }

        return departments;
    }


    [HttpPost]
    public IActionResult CreateDepartment([FromBody] Department dep)
    {
        using (SqlConnection con = new SqlConnection(constr))
        {
            string query = "insert into Department values (@DepartmentName)";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
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
    [Route("{departmentId:int}")]
    public IActionResult updateDepartment([FromRoute] int departmentId, [FromBody] Department dep)
    {
        using (SqlConnection con = new SqlConnection(constr))
        {
            string query = "update  Department set DepartmentName= @DepartmentName where DepartmentId = @DepartmentId ";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                cmd.Parameters.AddWithValue("@DepartmentId", departmentId);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return Ok();
                }
            }
        }

        return NotFound("Department Not Found");
    }

    [HttpDelete]
    [Route("{departmentId:int}")]
    public IActionResult deleteDepartment([FromRoute] int departmentId)
    {
        using (SqlConnection con = new SqlConnection(constr))
        {
            //  string query = "Delete FROM Department where DepartmentId='" + departmentId + "'";
            string query = "delete from   Department  where DepartmentId = @DepartmentId ";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@DepartmentId", departmentId);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return Ok();
                }
            }
        }

        return NotFound("Department Not Found");

    }







}
