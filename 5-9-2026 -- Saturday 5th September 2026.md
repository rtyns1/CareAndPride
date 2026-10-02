# PHASE 1: PROOF OF LIFE — SIMPLIFIED GUIDE

## Goal

Get a running API with a database that can create a user and retrieve a user. No authentication, no MediatR, no DTOs. Just raw EF Core and SQL Server working together.

---

## Step 1: Write the Domain Entity

Start with the `User` entity in the Domain project. This is a plain C# class with properties like Id, Username, Email, PasswordHash, FullName, Role, IsApproved, IsActive, and CreatedAtUtc.

Properties should have private setters so they cannot be changed accidentally. Use a private constructor for EF Core. Use a factory method called `Create()` that enforces business rules like "Username is required" and "Email must contain @". This method sets IsApproved to false by default and generates a new GUID for the Id.

Add a method called `SetPasswordHash()` so the Infrastructure layer can set the hashed password later.

---

## Step 2: Create the DbContext

Go to the Infrastructure project and create a class called `AppDbContext` that inherits from `DbContext`. Add a `DbSet<User>` property. In the `OnModelCreating` method, configure the User entity: set primary key, max lengths for strings, required fields, and unique indexes on Username and Email.

---

## Step 3: Install EF Core Packages

In the Infrastructure project, install three NuGet packages:
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools

---

## Step 4: Add Connection String

Open the API project's `appsettings.json` file and add a connection string under `ConnectionStrings`. Use `(localdb)\mssqllocaldb` as the server and name the database something like `CarePrideDb`.

---

## Step 5: Register DbContext in Program.cs

In the API project's `Program.cs`, register the DbContext with the connection string using `AddDbContext<AppDbContext>()` and `UseSqlServer()`. Also add controllers, Swagger, and endpoint mapping.

---

## Step 6: Create a Simple Controller

Create a `UsersController` in the API project with two endpoints:

- `GET /api/users` — Returns all users from the database using the DbContext.
- `POST /api/users` — Accepts a request object with Username, Email, FullName, and Role. Creates a new User using the `User.Create()` factory method, sets a placeholder password hash, adds it to the DbContext, saves changes, and returns the created user.

---

## Step 7: Add Migration and Create Database

Run the EF Core migration commands from the API project:

- `dotnet ef migrations add InitialCreate` — This creates a migration file based on your DbContext.
- `dotnet ef database update` — This creates the actual database and tables.

You need to specify the paths to the Infrastructure project and the API project so EF Core knows where to find the DbContext and the startup project.

---

## Step 8: Run the API

Run the API project using `dotnet run`. The API will start and listen for requests.

---

## Step 9: Test via Swagger

Open your browser and navigate to the Swagger URL (usually `https://localhost:5001/swagger`). You will see the `Users` endpoint.

First, send a POST request with a JSON body containing a username, email, full name, and role. The API should create the user and return a 201 response with the user's details.

Then, send a GET request to retrieve all users. You should see the user you just created in the response.

---

## Proof of Life Checklist

- Domain entity created
- DbContext created
- EF Core packages installed
- Connection string added
- DbContext registered in Program.cs
- Controller created
- Migration created
- Database created
- API runs without errors
- Swagger shows endpoints
- POST request creates a user
- GET request returns users

---

## What You Achieved

- A working API that runs on your machine
- A working database that stores user records
- A verified round trip: Domain → Infrastructure → API → Swagger → Database
- A foundation to build everything else on

---

## Next Steps (After Proof of Life)

- Add JWT authentication
- Add MediatR and split logic into commands and queries
- Add DTOs
- Add FluentValidation
- Build remaining entities: Student, Class, Subject, Grade, Attendance, Homework, TeacherNote, QuestionBank, AuditLog
- Build the Blazor MAUI desktop client

---

## Common Issues

- **"No DbContext found":** Make sure you run migrations from the correct folder and specify the correct paths to the Infrastructure and API projects.
- **"Cannot open database":** Ensure SQL Server is running. You can check and start it using `sqllocaldb`.
- **Swagger not showing:** Ensure `UseSwagger()` and `UseSwaggerUI()` are called before `Run()`.

---

**This is your foundation. Once this works, you have a running system. Everything else is building on top of this.**

**Go make it work.**