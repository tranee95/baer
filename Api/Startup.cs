using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Api.Model;
using Api.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
			var connectionString = Configuration.GetConnectionString("Connection");
			services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));

			services.AddIdentity<User, IdentityRole>(options => {
				options.Password = new PasswordOptions
				{
					RequireDigit = true,
					RequiredLength = 6,
					RequireLowercase = true,
					RequireUppercase = false,
					RequireNonAlphanumeric = false
				};

			}).AddEntityFrameworkStores<ApplicationContext>()
			  .AddDefaultTokenProviders();

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddMvc();
		}

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDBLogging((category, level) =>
			{
				if (level >= LogLevel.Information)
					return true;

				return false;
			});

			app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
