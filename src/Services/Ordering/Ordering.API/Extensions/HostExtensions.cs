using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;

namespace Ordering.API.Extensions
{
    public static class HostExtensions
    {   
        //    public static void MigrateDatabase<TContext>(this IServiceProvider provider)
        //     {
        //         var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

        //         using (var scope = provider.CreateScope())
        //         {
        //             var dbContext = scope.ServiceProvider
        //                 .GetRequiredService<DbContext>();
                    
        //             // Here is the migration executed
        //             dbContext.Database.Migrate();
        //         }

        //         // operations
        //     }
        public static void MigrateDatabase<TContext>(this IServiceProvider provider, Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext : DbContext
        {
            int retryForAvailability = retry.Value;

            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                    InvokeSeeder(seeder, context, services);

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(provider, seeder, retryForAvailability);
                    }
                }
            }
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, 
                                                    TContext context, 
                                                    IServiceProvider services)
                                                    where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }

        public static WebApplicationBuilder MigrateDataBase(this WebApplicationBuilder builder, int retryForAvailability = 0)
        {
                var config = builder.Configuration;               
                try

                {                   
                    var connection = new NpgsqlConnection

                        (config.GetValue<string>("DatabaseSettings:ConnectionString"));

                    connection.Open();

                    var command = new NpgsqlCommand();

                    command.Connection = connection;

                    command.CommandText = "DROP TABLE IF EXISTS Coupon";

                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,

                                                                ProductName VARCHAR(24) NOT NULL,

                                                                Description TEXT,

                                                                Amount INT)";

                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone XR', 'IPhone Discount', 200);";

                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";

                    command.ExecuteNonQuery();
                }

                catch (Exception exception)
                {
                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;

                        System.Threading.Thread.Sleep(2000);

                        MigrateDataBase(builder, retryForAvailability);

                    }
                    throw;
                }
                return builder;           
        }

    }
}
