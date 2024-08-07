﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OutOfOfficeApp.Infrastructure.Extensions
{
    public static class ConnectionExtensions
    {
        public static void AddDbConnection(this IServiceCollection service, IConfiguration configuration)
        {
            IServiceCollection serviceCollection = service.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("_"));
        }
    }
}
