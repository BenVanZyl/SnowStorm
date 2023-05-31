# Introduction 

This project provides a sample implemetation of the CQRS and MediatR patterns.  The core of this is the NuGet package SnowStorm.  The code for this is published in this project.


# CQRS + MediatR pattern
## CQRS: Command Query Responsibility Segregation
    - SnowStorm provides query execution functionailty.
## MediatR: Implements Mediator Pattern
 - https://github.com/jbogard/MediatR

# Supported Platforms
.Net 7 (Latest version)

# Release Notes

2023-06-01 (1.9.0)
 - Breaking changes on the way!
 - Upgraded to .Net 7
 - QueryExecutor is becoming obsolete.  Functionality is migrated to AppDbContext.  Classes are still available for backward compatibility but will be removed soon.
 - Container class.  Helper class to get objects from the DI container.  Is setup as last step in the services.AddSnowstorm(...) method.
 - Prepare for other features related to user management and auditing.
 - Beta release to test new changes and deployment.

2022-09-11
 - Performance enhancement by implementing db context pool
 - Bugfix for missing async operations for query executor

2021-11-26 -- 1.0.0
 - Breaking changes!
 - Upgraded to .Net 6
 - Changed queryExecutor.Execute to queryExecutor.Get
 - Expanded AppDbContext to return the underlaying connectionstring and DbConnection
 - Expanded AppDbContext to run raw sql command

2021-07-27 -- 0.21.0 -- FAILED TO DEPLOY AS .NET 5
 - Updated to .Net 5
 - There might be breaking changes due to the framework update.

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
