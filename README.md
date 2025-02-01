Simple demo to accompany this blog post:  
https://davecallan.com/dotnet-caching-one-second-really-make-difference/


**Start**:
1. Create a SQL Server DB and run OneSecondCacheDemo/CreateAndPopulateTable.sql to create a Currencies table and populate it with 1000 dummy currencies.
2. Update the connection string in OneSecondCacheDemo/appsettings.json to point to your SQL Server DB.
3. Hit endpoints via the OneSecondCacheDemo/OneSecondCacheDemo.http file.