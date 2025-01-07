using Boilerplate.API.Filters;
using Boilerplate.Repository;
using Boilerplate.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Boilerplate.API", Version = "v1" });
});

//Serilog Configuration
builder.Logging.ConfigureLog(configuration);
//Serilog Configuration

//Business Login & Data Access Registration.
builder.Services.AddBusinessLogicLayer();
builder.Services.AddDataAccessLayer();
//Business Login & Data Access Registration.

builder.Services.AddOptions();

//Set application session period.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(12);
});
//Set application session period.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = configuration["SecurityJwt:Issuer"],
                   ValidAudience = configuration["SecurityJwt:Issuer"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecurityJwt:Key"]))
               };
           });

//Configure Custome Authorization & Model Validator.
builder.Services.ConfigureCustomeAuthorization();
//Configure Custome Authorization & Model Validator.

builder.Services.AddEndpointsApiExplorer();
//Set Content root path.
builder.Host.UseContentRoot(Directory.GetCurrentDirectory());


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment() && app.Environment.IsProduction())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.UseMiddleware(typeof(ErrorHandlingMiddleware));
app.MapControllers();
app.UseSession();
//app.UseMvcWithDefaultRoute();
//app.UseMvc();

app.Run();
