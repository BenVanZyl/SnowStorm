# Introduction 
Making it easy to implement the CQRS pattern:
Query executors
MediatR
Swagger

CQRS stands for Command Query Responsibility Segregation

At its heart is the notion that you can use a different model to update information than the model you use to read information.
https://www.martinfowler.com/bliki/CQRS.html

https://en.wikipedia.org/wiki/CQRS

https://microservices.io/patterns/data/cqrs.html

Microsoft reference
https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs

https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/apply-simplified-microservice-cqrs-ddd-patterns

# How to use Azure Active Directory Integrated Authentication:

1. Add the followng NuGet package

       Microsoft.Azure.Services.AppAuthentication

2. Add the following to in 'startup.cs' method 'ConfigureServices(...)' before you implement 'services.AddDbContext<AppDbContext>(o => o.UseSqlServer(myConnectionString));' 
   
       services.AddSingleton<AzureServiceTokenProvider>(new AzureServiceTokenProvider()); 

       todo: code sample to enable az ad auth

3. Your connection string should only contain the 'Server' and 'Database' parameters.


# Supported Platforms
.Net Core 3.1 (Latest version)

# Release Notes

2021-04-05 -- 0.20.0
 - Breaking changes!
 - Removed 'Infrastructure' from the namespace
 - Introduced 'ExternalAssemblyName' for configuration of AppDbContext and MediatR for following reasons:
   - to use an external Library (project) for domain, command, etc. objects.
   - assist in setting up AppDbContext when doing integration testing in the test project.

2020-11-29 -- 0.17.0
- Bugfix -> supporting Azure Active Directory Integrated Authentication failed testing.
  - Removed code supporting Azure Active Directory Integrated Authentication.  
  - This is done in 'startup.cs' in method 'ConfigureServices(...)'
  

2020-11-29 -- 0.17.0
- Now supporting Azure Active Directory Integrated Authentication.
- Updated various NuGet components.
  - You should also update components in your project.
  - tip: Had some problems updating the versions in my project.  Managed to to get things working by updating the 'csproj' file.

2020-09-15 -- 0.14.0
- Changed CreateDateTime to CreatedOn
- Changed ModifyDateTime to ModiefiedOn
- Removed DomainException class
