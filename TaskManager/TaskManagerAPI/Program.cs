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

            // TODO: DEV/PROD Configs
            builder.Services.AddDbContext<TaskManagerDBContext>(options => options.UseSqlServer(
                "Data Source=localhost;" +
                "Initial Catalog=TaskManager;" +
                "User id=taskmng;" +
                "Password=sasaVV0595;" +
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
