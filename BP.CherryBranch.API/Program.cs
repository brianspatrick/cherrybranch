using Lucet.CherryBranch.API;
using Lucet.CherryBranch.API.Contracts;
using Lucet.CherryBranch.DataModel;
using Lucet.CherryBranch.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using System.Diagnostics;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? applicationInsights = builder.Configuration.GetValue<string>("ApplicationInsights:InstrumentationKey");

// Add Application Insights 
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = applicationInsights;
});

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

string AllowAll = "_allowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowAll,
    builder =>
    {
        builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});

// Inject the Repository instance here - which is currently ADOService,
// but when we change to GitHub, all we need to do is inject GitHubService
builder.Services.AddScoped<IRepository>(provider => new ADOService(builder.Configuration.GetSection("RepoApi")));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseCors(AllowAll);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

// Make sure authentication is called before authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run();

