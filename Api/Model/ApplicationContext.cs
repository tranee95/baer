using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Common;
using Common.Logging;

namespace Api.Model
{
	public class ApplicationContext : IdentityDbContext<User>
	{
		public ApplicationContext(DbContextOptions<ApplicationContext> options)
		   : base(options)
		{
		}

		public ApplicationContext()
		{

		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			if (!options.IsConfigured)
			{
				IConfigurationRoot configuration = new ConfigurationBuilder()
					 .SetBasePath(Directory.GetCurrentDirectory())
					 .AddJsonFile("appsettings.json")
					 .Build();

				var connectionString = configuration.GetConnectionString("Connection");
				options.UseNpgsql(connectionString);
			}
		}

		public DbSet<Test> Test { get; set; }
		public DbSet<EventLog> EventLog { get; set; }
	}
}
