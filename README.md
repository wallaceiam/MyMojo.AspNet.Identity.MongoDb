# MyMojo.AspNet.Identity.MongoDb

ASP.NET MVC 5 shipped with a new Identity system (in the Microsoft.AspNet.Identity.Core package) in order to support both local login and remote logins via OpenID/OAuth, but only ships with an Entity Framework provider (Microsoft.AspNet.Identity.EntityFramework).   

MyMojo.AspNet.Identity.MongoDb is a MongoDB backend provider that is a nearly in place replacement for the EF version.

# Quick Start

```csharp
using MyMojo.AspNet.Identity.MongoDb
```

Replace
```csharp
 services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
```
with
```csharp
services.AddMongoDbContext<IMongoDatabase>(
                connectionString: Configuration.GetConnectionString("MongoDbConnection"));
```

And Replace
```csharp
services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
```
with
```csharp
services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddMongoDbStores<IMongoDatabase>("users", "roles")
                .AddDefaultTokenProviders();
```

Please raise any Issues.

Thanks and enjoy
