using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagerShared;

namespace TaskManagerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().AddNewtonsoftJson();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDataProtection();


            // Auth
            builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
            builder.Services.AddAuthorizationBuilder();


            // DB Context
            builder.Services.AddDbContext<TaskManagerDBContext>(options => options.UseSqlServer(
                "Data Source=" + builder.Configuration["TaskManagerDBContext:DataSource"] + ";" +
                "Initial Catalog=" + builder.Configuration["TaskManagerDBContext:InitialCatalog"] + ";" +
                "User id=" + builder.Configuration["TaskManagerDBContext:UserId"] + ";" +
                "Password=" + builder.Configuration["TaskManagerDBContext:Password"] + ";" +
                "Encrypt=True;" +
                "TrustServerCertificate=True")
            );

            builder.Services.AddIdentityCore<TaskManagerUser>()
                .AddEntityFrameworkStores<TaskManagerDBContext>()
                .AddApiEndpoints();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapIdentityApi<TaskManagerUser>();


            app.MapControllers();

            app.Run();
        }
    }
}
