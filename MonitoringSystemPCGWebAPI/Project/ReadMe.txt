NuGet Packages Required

The project should download the following NuGet packages:

PM> Install-Package Microsoft.EntityFrameworkCore
PM> Install-Package Microsoft.EntityFrameworkCore.Tools
PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer
PM> Install-Package EFCore.BulkExtensions

For Auto Mapper Utility
PM> Install-Package AutoMapper

For App Utility
PM> Install-Package Microsoft.Extensions.Configuration
PM> Install-Package Microsoft.Extensions.Configuration.Json
PM> Install-Package Microsoft.Extensions.Configuration.Binder

In Program.cs (Main project) add this 
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddUtilities();
builder.Services.AddHttpContextAccessor(); 
