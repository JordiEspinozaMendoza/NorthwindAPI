using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;


[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    string connectionString;
    GetResponses ResponsesData;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(ILogger<EmployeesController> logger)
    {
        // * Initialize connection string with our environment variables.
        connectionString = $"Data Source=({Environment.GetEnvironmentVariable("SQL_CONNECTION_HOST")});" +
        $"Initial Catalog=Northwind;User={Environment.GetEnvironmentVariable("SQL_CONNECTION_USER_ID")};" +
        $"Password={Environment.GetEnvironmentVariable("SQL_CONNECTION_PASSWORD")};Integrated Security=false";

        ResponsesData = new GetResponses(connectionString);
        _logger = logger;
    }
    //* This get all products between two id's.
    [HttpGet("getproducts/between/{start}/{end}")]
    public ActionResult<DataTable> GetProductsBetween(string start, string end)
    {
        // * Query string 
        string queryString =
            $"SELECT * FROM PRODUCTS WHERE ProductID BETWEEN @start AND @end";
        // * Response variable
        string response = "";

        using (SqlConnection connection =
            new SqlConnection(connectionString))
        {
            //* Create the Command and Parameter objects.
            SqlCommand command = new SqlCommand(queryString, connection);
            //* Add the parameters.
            command.Parameters.AddWithValue("@start", start);
            command.Parameters.AddWithValue("@end", end);
            //* Open the connection in a try/catch block.
            try
            {
                connection.Open();
                //* Run the query.
                SqlDataReader reader = command.ExecuteReader();
                // * Create a DataTable from the reader.
                DataTable dt = new DataTable();
                // * Load the DataTable from the DataReader.
                dt.Load(reader);
                // * Convert the DataTable to JSON.
                response = JsonConvert.SerializeObject(dt, Formatting.Indented);
                //  * Close the reader in a finally block.
                reader.Close();
            }
            catch (Exception ex)
            {
                //! Create a JSON string containing an error message.
                response = $"{{\"error\": \"{ex.Message}\"}}";
                // * Close the reader in a finally block.
                return BadRequest(response);
            }
        }
        // * If everything went OK, send a 200 OK response.
        return Ok(response);
    }
    // * EMPLOYEES ENDPOINTS
    // * This get all employees.
    [HttpGet("getemployees")]
    public ActionResult<DataTable> GetEmployees()
    {
        // * Query string 
        string queryString =
            $"SELECT * FROM Employees";
        Dictionary<string, string> response = ResponsesData.getResponses(queryString);

        if (response["status"] != "success")
            return BadRequest(response["message"]);

        // * If everything went OK, send a 200 OK response.
        return Ok(response["message"]);
    }
    // * This get all employees with a given id.
    [HttpGet("getemployees/{id}")]
    public ActionResult<DataTable> GetEmployees(string id)
    {
        // * Query string 
        string queryString =
            $"SELECT * FROM Employees WHERE EmployeeID = @id";
        Dictionary<string, string> paramaters = new Dictionary<string, string>();
        paramaters.Add("@id", id);
        Dictionary<string, string> response = ResponsesData.getResponses(queryString, paramaters);
        if (response["status"] != "success")
            return BadRequest(response["message"]);

        // * If everything went OK, send a 200 OK response.
        return Ok(response["message"]);
    }
    // * This search for employees with a given name.
    [HttpGet("getemployees/search/{name}")]
    public ActionResult<DataTable> GetEmployeesByName(string name)
    {
        // * Query string 
        string queryString =
            $"SELECT * FROM Employees WHERE FirstName LIKE @name OR LastName LIKE @name";
        Dictionary<string, string> paramaters = new Dictionary<string, string>();
        paramaters.Add("@name", "%" + name + "%");
        Dictionary<string, string> response = ResponsesData.getResponses(queryString, paramaters);
        if (response["status"] != "success")
            return BadRequest(response["message"]);

        // * If everything went OK, send a 200 OK response.
        return Ok(response["message"]);
    }
   
    // * This get all the data from the employees that are in one specific country.
    [HttpGet("getemployees/country/{country}")]
    public ActionResult<DataTable> GetEmployeesCountry(string country)
    {
        // * Query string
        string queryString = "SELECT * FROM Employees WHERE Country = @country";
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("@country", country);
        Dictionary<string, string> response = ResponsesData.getResponses(queryString, parameters);
        if (response["status"] != "success")
            return BadRequest(response["message"]);

        // * If everything went OK, send a 200 OK response.
        return Ok(response["message"]);
    }
    // * This get all the data from the employees that are in one specific region.
    [HttpGet("getemployees/region/{region}")]
    public ActionResult<DataTable> GetEmployeesRegion(string region)
    {
        // * Query string
        string queryString = "SELECT * FROM Employees WHERE Region = @region";
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("@region", region);
        Dictionary<string, string> response = ResponsesData.getResponses(queryString, parameters);
        if (response["status"] != "success")
            return BadRequest(response["message"]);

        // * If everything went OK, send a 200 OK response.
        return Ok(response["message"]);
    }
    // * This get all the data from the employees that are in one specific city.
    [HttpGet("getemployees/city/{city}")]
    public ActionResult<DataTable> GetEmployeesCity(string city)
    {
        // * Query string
        string queryString = "SELECT * FROM Employees WHERE City = @city";
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("@city", city);
        Dictionary<string, string> response = ResponsesData.getResponses(queryString, parameters);
        if (response["status"] != "success")
            return BadRequest(response["message"]);

        // * If everything went OK, send a 200 OK response.
        return Ok(response["message"]);
    }

}