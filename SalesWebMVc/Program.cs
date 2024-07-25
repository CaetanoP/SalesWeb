using Microsoft.EntityFrameworkCore;
using SalesWebMVc.Data;
using SalesWebMVc.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.OpenApi.Models;
using SalesWebMVc.Filter;
using SalesWebMVc.Models.Validator;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SalesWebMVcContext>(options =>
{
	var connectionString = builder.Configuration.GetConnectionString("SalesWebMVcContext")
							?? throw new InvalidOperationException("Connection string 'SalesWebMVcContext' not found.");

	var serverVersion = new MySqlServerVersion(new Version(8, 0, 2));

	options.UseMySql(connectionString, serverVersion, mySqlOptions =>mySqlOptions.MigrationsAssembly("SalesWebMVc"));
	//Use EnableRetryOnFailure to enable automatic retry on failure
	

});


// Configuração do CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin", builder =>
	{
		builder.AllowAnyOrigin() // Substitua pela URL do seu front-end
			   .AllowAnyHeader()
			   .AllowAnyMethod();
	});
});

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "SalesWebMVc API", Version = "v1" });
	c.EnableAnnotations();
});
//Filter configuration
builder.Services.AddControllers(options =>
{
	options.Filters.Add<CustomExceptionFilter>();
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<SeedingService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<SalesRecordService>();
builder.Services.AddScoped<DepartmentValidator>();
builder.Services.AddScoped<SellerValidator>();
builder.Services.AddScoped<SalesRecordValidator>();

var app = builder.Build();

// Populate the database
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var seedingService = services.GetRequiredService<SeedingService>();
	seedingService.Seed();
}

if (app.Environment.IsDevelopment())
{
	// Configuração de localização para desenvolvimento (opcional)
	var supportedCultures = new[]
	{
		new CultureInfo("en-US"),
		new CultureInfo("pt-BR"),
	};
	var localizationOptions = new RequestLocalizationOptions
	{
		DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US"),
		SupportedCultures = supportedCultures,
		SupportedUICultures = supportedCultures
	};
	app.UseRequestLocalization(localizationOptions);
}
else
{
	app.UseExceptionHandler("/Home/Error"); // Configuração de erro para produção
	app.UseHsts(); // Configuração de HSTS para produção
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowSpecificOrigin"); // Habilitar CORS

// Habilitar o Swagger em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "SalesWebMVc API V1");
		c.RoutePrefix = string.Empty; // Corrige a rota do SwaggerUI
	});
}

app.UseAuthorization();

// Configuração de rotas para MVC (opcional)
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
