using MGAware.Security.JWT;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using MGAware.Database.Context;
using MGAware.Database.DTO;
using Microsoft.AspNetCore.OData;
using MGAware.Security.JWT.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddControllers().AddOData(options =>
    options.Select().Filter().Count().OrderBy().Expand().SetMaxTop(1000));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenJwt("v1",
    new OpenApiInfo
    {
        Title = "MGAWare.WebApi",
        Description = "Controle de Acesso MGAWare",
        Version = "v1"
    });


var connection = builder.Configuration["ConnectionStrings:MySqlConnectionString"];
var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));

builder.Services.AddDbContext<MGADBContext>(options =>
        options.UseMySql(connection,serverVersion));


var tokenConfigurations = new TokenConfigurationsDto();
    new ConfigureFromConfigurationOptions<TokenConfigurationsDto>(
        builder.Configuration.GetSection("TokenConfigurations"))
            .Configure(tokenConfigurations);

builder.Services.AddJwtSecurity(tokenConfigurations);

builder.Services.AddScoped<IdentityInitializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsapp");

app.UseHttpsRedirection();

app.UseAuthorization();

using var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<IdentityInitializer>().Initialize();


app.MapControllers();

app.Run();
