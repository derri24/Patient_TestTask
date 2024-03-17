using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Patient.TestTask.RestApi.Configuration;
using Patient.TestTask.RestApi.DataAccess;
using Patient.TestTask.RestApi.Helpers;
using Patient.TestTask.RestApi.Services;
using Patient.TestTask.RestApi.Services.Interfaces;
using Prakle.ManagementWarehouse.Api.Base.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new GuidJsonConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "OpenAPI",
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});



// Add Database
builder.Services.AddSingleton<IDatabaseConfiguration, DatabaseConfiguration>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// Add services
builder.Services.AddScoped<IPatientService, PatientService>();


var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();