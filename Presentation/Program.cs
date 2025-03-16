using Domain.Database;
using Microsoft.EntityFrameworkCore;
using Presentation;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddControllersAsServices();

builder.Services
    .AddDatabaseContext(builder.Configuration, builder.Environment)
    .AddSwagger()
    .AddRepositories()
    .AddServices()
    .AddQuartzJobs()
    .AddQuartzHostedService(q => q.WaitForJobsToComplete = true)
    .AddHttpClients(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.UseExceptionHandlerMiddleware();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    await db.Database.MigrateAsync();
}

await app.RunAsync();
