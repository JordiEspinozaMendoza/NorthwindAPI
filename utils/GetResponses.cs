using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
public class GetResponses
{
    string connectionString;
    public GetResponses(string connectionString)
    {
        this.connectionString = connectionString;
    }
    // * This function is used to get the response from the database.
    public Dictionary<string, string> getResponses(string queryString, Dictionary<string, string> parameters = null)
    {
        // * Response variable
        string response = "";
        using (SqlConnection connection =
            new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            // * Create the Command and Parameter objects.
            if (parameters != null)
                foreach (KeyValuePair<string, string> parameter in parameters)
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);

            // * Open the connection in a try/catch block.
            try
            {
                connection.Open();
                // * Run the query.
                SqlDataReader reader = command.ExecuteReader();
                // * Create a DataTable from the reader.
                DataTable dt = new DataTable();
                // * Load the DataTable from the DataReader.
                dt.Load(reader);
                // * Convert the DataTable to JSON.
                response = JsonConvert.SerializeObject(dt, Formatting.Indented);
                // * Close the reader in a finally block.
                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                // * Create a JSON string containing an error message.
                response = $"{{\"error\": \"{ex.Message}\"}}";
                // * Close the reader in a finally block.
                connection.Close();
                return new Dictionary<string, string>{
                    { "status", "error"},
                    { "message", response}
                };
            }
        }
        // * If everything went OK, send a success message.
        return new Dictionary<string, string>{
            { "status", "success"},
            { "message", response}
        };
    }
}