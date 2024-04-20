// To Trust Dev Certificate
dotnet dev-certs https --trust

// If using IIS when you need .NET Core tools for IIS
.NET Core tools for IIS 
https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-8.0.4-windows-hosting-bundle-installer

// Restart IIS after install
net stop was /y
net start w3svc

// Config file for DB
\TaskManager\TaskManagerAPI\appsettings.json