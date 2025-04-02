using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Services.Context;
using Repository.Services.Contract;
using Repository.Services.Library;
using System;


namespace Repository.Services.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureRepoServiceImplementation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISecurityRepositoryService, SecurityRepositoryService>();
            services.AddScoped<IUserRepositoryService, UserRepositoryService>();

            // services.AddScoped<IChatGTPRepositoryService, ChatGTPAIRepositoryService>();

            //services.AddScoped<AppDbContext>(options => new AppDbContext(configuration.GetSection("Logging").GetConnectionString("DefaultConnection")));

           // services.AddDbContext<AddDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<AddDbContext>(options => new AddDbContext(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
