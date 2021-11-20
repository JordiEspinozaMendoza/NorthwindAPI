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

    // * This gets the product with the specified id
     [HttpGet("getproducts/{id}")]
    public ActionResult<DataTable> GetProductsID(string id)
    {
        // * Query string 
        string queryString =
            $"SELECT * FROM Products WHERE ProductID = @id";
        Dictionary<string, string> paramaters = new Dictionary<string, string>();
        paramaters.Add("@id", id);
        Dictionary<string, string> response = ResponsesData.getResponses(queryString, paramaters);
        if (response["status"] != "success")
            return BadRequest(response["message"]);

        // * If everything went OK, send a 200 OK response.
        return Ok(response["message"]);
    }

    // * This gets all the data from the products that their name starts with the specified letter.
    [HttpGet("getproducts/search/{letter}")]
    public ActionResult<DataTable> GetProductsByLetter(string letter)
    {
        // * Query string 
        string queryString =
        $"SELECT * FROM Products WHERE ProductName LIKE @letter";
        Dictionary<string, string> paramaters = new Dictionary<string, string>();
        parameters.Add("@letter", letter + "%");
        Dictionary<string, string> response = ResponsesData.getResponses(queryString, paramaters);
        if (response["status"] != "success")
            return BadRequest(response["message"]);

        // * If everything went OK, send a 200 OK response.
        return Ok(response["message"]);
    }

    // * This gets the specified product by its name.
    [HttpGet("getproducts/productname/{name}")]
    public ActionResult<DataTable> GetProductsName(string name)
    {
        // * Query string
        string queryString = "SELECT * FROM Products WHERE ProductName = @name";
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("@name", name);
        Dictionary<string, string> response = ResponsesData.getResponses(queryString, parameters);
        if (response["status"] != "success")
            return BadRequest(response["message"]);

        // * If everything went OK, send a 200 OK response.
        return Ok(response["message"]);
    }

    // * This gets all the data from the products from the specified supplier .
    [HttpGet("getproducts/productinfosup/{supplier}")]
    public ActionResult<DataTable> GetProductsInfoSup(string supplier)
    {
        // * Query string
        string queryString = "SELECT * FROM Products WHERE SupplierID = @supplier";
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("@supplier", supplier);
        Dictionary<string, string> response = ResponsesData.getResponses(queryString, parameters);
        if (response["status"] != "success")
            return BadRequest(response["message"]);

        // * If everything went OK, send a 200 OK response.
        return Ok(response["message"]);
    }

// * This gets all the data from the products which unit price is higher than the specified.
     [HttpGet("getproducts/maxunitprice/{muprice}")]
    public ActionResult<DataTable> GetProductsMUPrice(string muprice)
    {
        // * Query string
        string queryString = "SELECT * FROM Products WHERE UnitPrice > @muprice";
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("@muprice", muprice);
        Dictionary<string, string> response = ResponsesData.getResponses(queryString, parameters);
        if (response["status"] != "success")
            return BadRequest(response["message"]);

        // * If everything went OK, send a 200 OK response.
        return Ok(response["message"]);
    }

// * This gets all the data from the products with the specified unit price.
     [HttpGet("getproducts/unitprice/{uprice}")]
    public ActionResult<DataTable> GetProductsUPrice(string uprice)
    {
        // * Query string
        string queryString = "SELECT * FROM Products WHERE UnitPrice > @uprice";
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("@uprice", uprice);
        Dictionary<string, string> response = ResponsesData.getResponses(queryString, parameters);
        if (response["status"] != "success")
            return BadRequest(response["message"]);

        // * If everything went OK, send a 200 OK response.
        return Ok(response["message"]);
    }
}