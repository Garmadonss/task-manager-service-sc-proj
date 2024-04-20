To create DB via EF code-first models please following command:
dotnet ef migrations add InitialCreate --project TaskManagerShared -s TaskManagerAPI

// To update Migrations
dotnet ef database update --project TaskManagerShared -s TaskManagerAPI