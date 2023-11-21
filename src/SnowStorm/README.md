# Introduction 

This package provides an Enity Framework DBContext class to easily implement the query part of the CQRS pattern.  
It also provides additional features around auto configuration of the domain entities.

For more details, see...
https://github.com/BenVanZyl/SnowStorm

# How to use

# Supported Platforms
.Net 7 (Latest version) 

# Latest Release Notes

2023-0x-xx (2.0.0)
 - BREAKING CODE!
 - Removing obsolete code.
 - Removing QueryExecutor as this functionality is fully integrated into the AppDbContext.
 - Moved AppDbContext to DbContext folder and namespace.
 - Bug fixes.
 - Beta: Use CurrentUser to extract user info from HttpContext (id, name, guid)
   - could recquire the host app to inject HttpContextAccessor.
     - builder.Services.AddHttpContextAccessor();
 - Beta: When saving, use CurrentUser and reflection to update audit properties. 
 - Beta: GetById<T> methods added to reduce the need for query objects.
 - Beta: GetAll<T> method to get all the rows from a table.  This will reduce the need for query classes for reference data like dropdowns.