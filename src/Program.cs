using System.Text;
using AspNetCore.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AspNetCore.WebApi.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
ConfigureAuthentication(builder);
ConfigureAuthorization(builder);
ConfigureServices(builder);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();


void ConfigureAuthentication(WebApplicationBuilder builder)
{

    var Key = Encoding.ASCII.GetBytes(Configuration.Secret);

    builder.Services.AddAuthentication(x =>
    {

        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })

    .AddJwtBearer(x =>
    {

        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = 
        new TokenValidationParameters
        {

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Key),
            ValidateIssuer = false,
            ValidateAudience = false
        };

    });

}

void ConfigureAuthorization(WebApplicationBuilder builder)
{

    builder.Services.AddAuthorization(x =>
    {
        x.AddPolicy(name: "Admin", configurePolicy: x => x.RequireRole("Manager"));
        x.AddPolicy(name: "Employee", configurePolicy: x => x.RequireRole("Employee"));

    });
}

void ConfigureServices(WebApplicationBuilder builder)
{

    builder.Services.AddDbContext<Context>
    (opt => opt.UseInMemoryDatabase("Database"));
}
