# Introduction 

This package provides an Enity Framework DBContext class to easily implement the query part of the CQRS pattern.  
It also provides additional features around auto configuration of the domain entities.

For more details, see...
https://github.com/BenVanZyl/SnowStorm

# How to use

# Sample App

Located under: *src\Sample\*
Root Namespace: *WebSample.SnowStorm* 

This is a Blazor WebAssembly app using a .NET CORE hosted backend using REST API.
Using DbUp to manage changes to the database and to run end to end testing.

Tests for this application is End-to-End based and run as unit tests.  It is a more practical approach and reduces the risks that your app might pass test but break in production.  What it runs is what you get.

The test provides a sample of 
- just testing the API
- testing end to end using Blazor Web Assembly UI
- testing end to end using Blazor Web Assembly UI and HTTP mocking. *(Not my recommended way.)*


# Supported Platforms
.Net 7 (Latest version) 

# Latest Release Notes

2024-03-17 (2.2.0)
 - Fixed  save feature (POST)
 - Fixed tests - Save data and DbCleanup.
 - Improved Async handling when saving.
 - [wip] some UI elements and associated tests

2024-01-17 (2.1.0)
 - Update NuGet packages to resolve reported vulnerabilities and to ensure all is as upo to date as possible.
 - Target Frame work is .NET 7
 - WebSample.Tests is making progress.

2024-01-04 (2.0.0)
 - BREAKING CODE!
 - Removing obsolete code.
 - Removing QueryExecutor as this functionality is fully integrated into the AppDbContext.
 - Moved AppDbContext to DbContext folder and namespace.
 - Change sample application to a Blazor WebAssembly app with ASP.NET CORE hosted backend.
 - Added End-to-End testing for the sample app. 
 - Bug fixes.
 - Beta: Use CurrentUser to extract user info from HttpContext (id, name, guid)
   - could recquire the host app to inject HttpContextAccessor.
     - builder.Services.AddHttpContextAccessor();
 - Beta: When saving, use CurrentUser and reflection to update audit properties. 
 - Beta: GetById<T> methods added to reduce the need for query objects.
 - Beta: GetAll<T> method to get all the rows from a table.  This will reduce the need for query classes for reference data like dropdowns.

