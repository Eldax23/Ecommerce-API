# eCommerce API Project

## Overview
This project is a robust and scalable eCommerce backend built using **.NET 8** and follows **Clean Architecture** principles. It is designed to handle typical eCommerce functionalities, providing a maintainable and efficient solution.

## Features
- **Entity Framework Core** for data access
- **AutoMapper** for object mapping
- **FluentValidation** for input validation
- **Serilog** for logging
- **Swagger** for API documentation
- Dependency injection for enhanced testability and scalability

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (or any other supported database)

### Installation
1. Clone the repository:
    ```bash
    git clone https://github.com/yourusername/ecommerce-api.git
    cd ecommerce-api
    ```

2. Restore the dependencies:
    ```bash
    dotnet restore
    ```

3. Update the database connection string in `appsettings.json`.

4. Apply database migrations:
    ```bash
    dotnet ef database update
    ```

5. Run the application:
    ```bash
    dotnet run
    ```

### Usage
- Access the API documentation at `http://localhost:5000/swagger`.

## Contributing
Contributions are welcome! Please submit a pull request or open an issue to discuss any changes.

## License
This project is licensed under the MIT License. See the LICENSE file for details.
