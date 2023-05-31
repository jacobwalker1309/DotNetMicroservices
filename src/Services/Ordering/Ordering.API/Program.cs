using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

//builder.Services.MigrateDatabase<Program>();

var app = builder.Build();

app.Services.MigrateDatabase<OrderContext>((context, service) => 
{
    var logger = app.Services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context, logger).Wait();

    /*
     * SqlException: Cannot insert the value NULL into column 'CVV', table 'OrderDb.dbo.Orders'; column does not allow nulls. INSERT fails.
    The statement has been terminated.
     * 
     * 
     * 
     * 
     */
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
