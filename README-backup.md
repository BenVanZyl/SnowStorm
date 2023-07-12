# Introduction 

This project provides a sample implemetation of the CQRS and MediatR patterns.  The core of this is the NuGet package SnowStorm.  The code for this is published in this project.


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

# Performance considerations for connection string

The foiloing connection properties shpould be considered 

 - MultipleActiveResultSets=true;
 - Enlist=true;
 - Pooling=true;Min Pool Size=1;Max Pool Size=32;
 - Timeout=35;
 - Connection Lifetime=1;

 Notes:
  - Retry is handle by the DbContext EnableRetryOnFailure option
  - Connection Lifetime would need to be adjusted according to your setup. ex. 5 secs for server, 1 sec for local.
  - See SnowStorm.PerformanceTesting on GitHub for example of how this could be tested.

# Supported Platforms
.Net 7 (Latest version)

# Release Notes

See README.md under ../src/SnowStorm/README.md
