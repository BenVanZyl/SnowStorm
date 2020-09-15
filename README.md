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

# Supported Platforms
.Net Core 3.0 (Latest version)
.Net Core 2.2 (0.3.0)

# Release Notes

2020-09-15 -- 0.14.0
- Changed CreateDateTime to CreatedOn
- Changed ModifyDateTime to ModiefiedOn
- Removed DomainException class
