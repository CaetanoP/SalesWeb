using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMVc.Data;
using SalesWebMVc.Services;
namespace SalesWebMVc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<SalesWebMVcContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("SalesWebMVcContext")
                                        ?? throw new InvalidOperationException("Connection string 'SalesWebMVcContext' not found.");

                var serverVersion = new MySqlServerVersion(new Version(8, 0, 2)); // Exemplo: MySQL 8.0.2

                options.UseMySql(connectionString, serverVersion, mySqlOptions =>
                    mySqlOptions.MigrationsAssembly("SalesWebMVc") // Passe o nome do Assembly diretamente
                );
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //Add a scope for SeedingService
            builder.Services.AddScoped<SeedingService>();
			//Add a scope for SellerService
			builder.Services.AddScoped<SellerService>();
            //Add a scope for DepartmentService
            builder.Services.AddScoped<DepartmentService>();


			var app = builder.Build();
            //Populate the database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seedingService = services.GetRequiredService<SeedingService>();
                seedingService.Seed();
            }

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
