using Microsoft.EntityFrameworkCore;
using WorkflowPlatform.Api.Data;
using WorkflowPlatform.Api.Endpoints;
using WorkflowPlatform.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<WorkflowPlatformDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WorkflowPlatformDb")));

builder.Services.AddScoped<IWorkflowDefinitionService, WorkflowDefinitionService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WorkflowPlatformDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapWorkflowDefinitionEndpoints();

app.Run();
