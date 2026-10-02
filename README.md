# CareAndPride

School management system for Care and Pride Academy.

Backend: ASP.NET Core Web API (.NET 10) with Clean Architecture (Domain, Application, Infrastructure, API).
Frontend: Razor Pages.
Database: SQL Server (LocalDB for development).
Auth: JWT for the API, cookie sessions for the web portal.

## Running locally

Terminal A:
    cd CarePrideSystem.API
    dotnet run

Terminal B:
    cd CarePrideSystem.Web
    dotnet run

Open http://localhost:5000

Default admin credentials: admin / Admin@123
Sample teacher: acristina / Teacher@123
