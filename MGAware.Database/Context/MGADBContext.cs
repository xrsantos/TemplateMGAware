using MGAware.Database.DAL;
using MGAware.Database.DTO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace MGAware.Database.Context;

public class MGADBContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Person> Person { get; set; }
    public DbSet<Contact> Contact { get; set; }
    public MGADBContext()
        : base()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

        var connection = configuration["ConnectionStrings:MySqlConnectionString"];
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
        options.UseMySql(connection,serverVersion);
    }

    public MGADBContext(DbContextOptions<MGADBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Person>()
            .HasMany(c => c.Contacts)
            .WithOne(e => e.Person);
        
        base.OnModelCreating(builder);
    }
}


