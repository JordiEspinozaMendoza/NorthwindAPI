### Northwind API with ASP.NET Core

This is a simple ASP.NET Core API that provides access to the Northwind database.

Before you can use the API, you need to have the Northwind database installed. If you don't have it yet, here is the repo where you can get the raw SQL files:

[Link to the repo](https://github.com/Microsoft/sql-server-samples/tree/master/samples/databases/northwind-pubs)

### Getting Started

#### Install System.Data.SqlClient

Open a new terminal and run the following command:

```
dotnet add package System.Data.SqlClient --version 4.8.3
```

#### Install Newtonsoft.Json

Open a new terminal and run the following command:

```
dotnet add package Newtonsoft.Json --version 13.0.1
```

#### Adding launchSettings.json

Inside of Properties directory, create a new file named launchSettings.json and copy the following contents into it:
Take a look at the following example and make sure to replace the values with your own

```
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:1711",
      "sslPort": 44306
    }
  },
  "profiles": {
    "NorthwindAPI": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:7177;http://localhost:5178",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "SQL_CONNECTION_PASSWORD": "HERE_IS_YOUR_PASSWORD",
        "DATA_BASE_NAME": "Northwind"
        "SQL_CONNECTION_USER_ID": "HERE_IS_YOUR_USER_ID_OR_NAME"
        "SQL_CONNECTION_HOST": "HERE_IS_YOUR_HOST"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

If everything goes well, you should see the following in the terminal:

```
dotnet run build
```

### Testing the API

You could test the API in [Postman](https://www.postman.com/) or any other tool or client to make petitions to the API.

### Tables of the Northwind database

The Northwind database contains the following tables:

![](img/tables.png?raw=true)

You can create a new Controllers for each table and make petitions to the API.
