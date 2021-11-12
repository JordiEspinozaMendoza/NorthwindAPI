using Microsoft.AspNetCore.Mvc;

using System;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;


[ApiController]
[Route("[controller]")]
public class NorthwindController : ControllerBase
{
    public NorthwindController()
    {
    }
    [HttpGet("getproducts/between/{start}/{end}")]
    public ActionResult<DataTable> GetProductsBetween(string start, string end)
    {
        string connectionString = $"Data Source=(local);Initial Catalog=Northwind;User=SA;Password={Environment.GetEnvironmentVariable("SQL_CONNECTION_PASSWORD")};Integrated Security=false";

        string queryString =
            $"SELECT * FROM PRODUCTS WHERE ProductID BETWEEN ${start} AND ${end}";

        int paramValue = 5;
        string response = "";

        using (SqlConnection connection =
            new SqlConnection(connectionString))
        {
            // Create the Command and Parameter objects.
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@pricePoint", paramValue);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                response = JsonConvert.SerializeObject(dt, Formatting.Indented);

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        return Ok(response);
    }
}