﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Ordering.Api.Extensions
{
    public static class HostExtensions
    {
        public static WebApplication MigrateDatabase<TContext>(this WebApplication web,
            Action<TContext, IServiceProvider> seeder,
            int? retry = 0) where TContext : DbContext
        {
            int retryForAvailability = retry.Value;

            using (var scope = web.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("migrating started for sql server");
                    InvokeSeeder(seeder, context, services);
                    logger.LogInformation("migrating has been done for sql server");
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, "an error occurred while migrating database");

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(web, seeder, retryForAvailability);
                    }
                    throw;
                }
            }

            return web;
        }

        private static void InvokeSeeder<TContext>(
            Action<TContext, IServiceProvider> seeder,
            TContext context,
            IServiceProvider services)
            where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
