using ActionCommandGame.Api.Installers.Extensions;
using ActionCommandGame.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Install services to the container using IInstaller classes.
builder.Services.InstallServicesInAssembly(builder.Configuration);

var app = builder.Build();

//Initialize dbContext data

var dbContext = app.Services.GetRequiredService<ActionCommandGameDbContext>();
if (dbContext.Database.IsInMemory())
{
    dbContext.Initialize();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Action Command Game API v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
