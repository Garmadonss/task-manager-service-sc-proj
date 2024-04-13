To create DB via EF code-first models please following command:
dotnet ef migrations add InitialCreate --project TaskManagerShared -s TaskManagerAPI


// To update Migrations
dotnet ef database update --project TaskManagerShared -s TaskManagerAPI

dotnet dev-certs https --trust

.NET Core tools for IIS 
https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-8.0.4-windows-hosting-bundle-installer

net stop was /y
net start w3svc