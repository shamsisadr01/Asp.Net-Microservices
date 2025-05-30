﻿using Npgsql;

namespace Discount.Api.Extentions
{
    public static class Extentions
    {
        public static WebApplication MigrateDatabase<TContext>(this WebApplication webApplication, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using (var scope = webApplication.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                // migrate database
                try
                {
                    logger.LogInformation("migrating posgtresql database");

                    using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };

                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,
                                                                ProductName VARCHAR(200) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";

                    command.ExecuteNonQuery();

                    // seed data

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('IPhone x', 'iphone discount', 150);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Samsung 10', 'samsung discount', 150);";
                    command.ExecuteNonQuery();

                    logger.LogInformation("migration has been completed!!!");

                }
                catch (NpgsqlException ex)
                {
                    logger.LogError("an error has been occured");

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(webApplication, retryForAvailability);
                    }
                }
            }

            return webApplication;
        }
    }
}
