using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using VacationRental.Api.Extensions;
using VacationRental.Api.Models;
using VacationRental.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

builder.Services.AddCustomApiVersioning();

builder.Services.AddControllers(opts =>
{
    opts.Filters.Add<OperationCanceledExceptionFilter>();
    opts.Filters.Add(new ProducesResponseTypeAttribute(499)); // Client Closed Request);
    opts.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
});
builder.Services.Configure<RouteOptions>(opts => opts.LowercaseUrls = true);

builder.Services.AddApplicationHealthChecks();

builder.Services.AddSwaggerGen(opts =>
    opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" }));

builder.Services.AddSingleton<IDictionary<int, RentalViewModel>>(new Dictionary<int, RentalViewModel>());
builder.Services.AddSingleton<IDictionary<int, BookingViewModel>>(new Dictionary<int, BookingViewModel>());

builder.Services.AddVacationRentalApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app.UseApplicationHealthChecks();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapApplicationHealthChecks();
});

app.UseSwagger();
app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));

app.Run();
