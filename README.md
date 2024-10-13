# SysgamingApi

This is the SysgamingApi project, built with .NET 6. The project uses Docker to run a PostgreSQL database on port 5433.

## Prerequisites

- .NET 6 SDK
- Docker

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/yourusername/sysgaming-service.git
cd sysgaming-service/SysgamingApi
```

### Build and Run the Application

1. **Build the Docker containers:**

    ```bash
    docker-compose up --build
    ```

2. **Run the application:**

    ```bash
    dotnet run
    ```

### Docker Configuration

The Docker setup includes a PostgreSQL database running on port 4333. The configuration can be found in the `docker-compose.yml` file.

### Database Connection

The application connects to the PostgreSQL database using different connection strings depending on the environment:

- **Local with PostgreSQL in Docker:**

    ```
    Host=localhost;Port=4333;Database=sysgamingdb;Username=yourusername;Password=yourpassword
    ```

- **Application in Docker:**

    ```
    Host=database;Database=sysgaming;Username=postgres;Password=admin
    ```

Make sure to update the connection strings in your `appsettings.json` file accordingly:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "PostgreSqlDocker": "Host=database;Database=sysgaming;Username=postgres;Password=admin",
    "PostgreSqlLocal": "Host=localhost;Port=5433;Database=sysgaming;Username=postgres;Password=admin"
  },
  "JWT": {
    "Secret": "5aaa3b**********************06930b",  
    "ValidIssuer": "http://localhost:5132",
    "ValidAudience": "http://localhost:5000",
    "ExpireMinutes": 60
  }
}
```

## Project Structure

- **/Controllers**: API Controllers
- **/Models**: Data Models
- **/Services**: Business Logic
- **/Data**: Database Context and Migrations

## Running Tests

To run the tests, use the following command:

```bash
dotnet test
```

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License.
