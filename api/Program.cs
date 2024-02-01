using api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<MyDbContext>();
    dbContext.Database.EnsureDeleted(); 
    dbContext.Database.EnsureCreated(); 
    SeedDatabase(dbContext);
}

app.UseAuthorization();

app.MapControllers();

app.Run();

void SeedDatabase(MyDbContext context)
{
    if (!context.Subscriptions.Any())
    {
        context.Subscriptions.AddRange(new List<Subscription>()
        {
            new()
            {
                Id = 1,
                Name = "Premium"
            },
            new()
            {
                Id = 2,
                Name = "Standard"
            },
            new()
            {
                Id = 3,
                Name = "Restricted"
            }
        });
        
        context.Persons.AddRange(new List<Person>()
        {
            new()
            {
                Id = 1,
                FirstName = "John",
                LastName = "Smith",
                SubscriptionId = 2,
                IsAdmin = false
            },
            new()
            {
                Id = 2,
                FirstName = "Mary",
                LastName = "West",
                SubscriptionId = 1,
                IsAdmin = true
            }
        });
        
        context.SaveChanges();
    }
}