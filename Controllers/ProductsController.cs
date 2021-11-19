using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;


[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    string connectionString;
    GetResponses ResponsesData;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger) // * Initialize connection string with our environment variables.
    {
        connectionString = $"Data Source=({Environment.GetEnvironmentVariable("SQL_CONNECTION_HOST")});" +
        $"Initial Catalog=Northwind;User={Environment.GetEnvironmentVariable("SQL_CONNECTION_USER_ID")};" +
        $"Password={Environment.GetEnvironmentVariable("SQL_CONNECTION_PASSWORD")};Integrated Security=false";

        ResponsesData = new GetResponses(connectionString);
        _logger = logger;
    }
    
    [HttpGet("getproducts/between/{start}/{end}")] //* This get all products between two id's. (???)
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
    // * PRODUCTS ENDPOINTS
    // * This get all products.
    [HttpGet("getproducts")]
    public ActionResult<DataTable> GetProducts()
    {
        // * Query string 
        string queryString =
            $"SELECT * FROM PRODUCTS";
        Dictionary<string, string> response = ResponsesData.getResponses(queryString);

        if (response["status"] != "success")
            return BadRequest(response["message"]);

        // * If everything went OK, send a 200 OK response.
        return Ok(response["message"]);
    }

    // * This gets 
    [HttpGet("getproducts/city/{city}")]
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
        //cuack
    }
}