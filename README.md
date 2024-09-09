# Casino-Star
Casino Star created by Patryk 'UltiPro' Wójtowicz using ASP.NET Core with Angular.

---

Warning:

> This application is my second major project in college, so there is a lot of ugly code. Additionally, instead of using EF Core and Identity, stored procedures and JWT Token Authentication/Authorization were used. The template of the project comes from .NET 6.

# Dependencies and Usage

Dependencies:

<ul>
  <li>BCrypt.Net-Next 4.0.3</li>
  <li>Microsoft.AspNetCore.Authentication.JwtBearer 6.0.12</li>
  <li>Microsoft.AspNetCore.SpaProxy 6.0.9</li>
  <li>Microsoft.Extensions.Configuration.Json 7.0.0</li>
  <li>System.Data.SqlClient 4.8.5</li>
</ul>

Before running or publishing the application:

> On the database server (Microsoft SQL Server), execute the script creating the database (./Casino Star/Casino Star Database/Create.sql) and then provide the appropriate connection string to the database in the "appsettings.json" file (in ./Casino Star/Casino Star folder).

Running the app:

> cd "/Casino Star/Casino Star"

> dotnet run

Publishing the app:

> cd "/Casino Star/Casino Star"

> dotnet publish

> cd "/bin/Debug/net8.0/publish"

# Preview

![Index Page 1 Preview](/screenshots/IndexPage1.png)
