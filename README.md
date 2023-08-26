# Introduction 

Snowstorms can be wild, unpredictable, and dangerous.  It can blind you and you cannot see where you go. When it is done, 
you can be left standing in a beautiful winter landscape. There are some comparisons there when developing software. This 
project is an attempt to take some of the lessons I have learnt traveling through my snowstorms in developing software, 
and create something useful. A practical way to approach building applications.  A way that can meet the high standards 
we set for ourselves (and others), and hopefully not end up with creating a painful rabbit hole of spaghetti code, 
wrapped inside some fancy name, that is supposedly the "best" architecture or pattern being boasted about on the internet.

Review the code in this repository, and you will find implementations of the following concepts:
 - CQRS.
 - Domain Driven Design.
 - MediatR.
 - DbUp for database script deployment.
 - Unit Testing.
   - Integration tests as unit tests.
   - Feature based unit testing.
   - Testing DbUp scripts as part of the integration testing.


This project provides a sample implementation of the CQRS and MediatR patterns.  The core of this is the NuGet package SnowStorm.  The code for this is published in this project.


# CQRS + MediatR pattern
## CQRS: Command Query Responsibility Segregation
    SnowStorm provides query execution functionailty.
## MediatR: Implements Mediator Pattern for handling commands
    https://github.com/jbogard/MediatR

# Supported Platforms
.Net 7 (Latest version)

# Release Notes

2023-0x-xx (2.0.0)
 - BREAKING CODE!
 - Removing obsolete code.
 - Removing QueryExecutor as this functionality is fully integrated into the AppDbContext.
 - Moved AppDbContext to DbContext folder.

2023-07-13 (1.11.0)
 - Final version of current code base.  Next version will be 2.0.0 and will contain various breaking changes and the removal of depreciated functionality.
 - Updated NuGet packages and resolving some compiler warnings.

2023-06-01 (1.10.0)
 - Bug fix related to EF Core not always detecting updated values.
 - Sample application test updated to cleanup after itself.

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
  - tip: Had some problems updating the versions in my project.  Managed to to get things working by updating the project file.

2020-09-15 -- 0.14.0
- Changed CreateDateTime to CreatedOn
- Changed ModifyDateTime to ModiefiedOn
- Removed DomainException class
