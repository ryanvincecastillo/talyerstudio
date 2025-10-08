using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TalyerStudio.JobOrder.Infrastructure.Data;

public class JobOrderDbContextFactory : IDesignTimeDbContextFactory<JobOrderDbContext>
{
    public JobOrderDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<JobOrderDbContext>();
        
        // Temporary connection string for migrations
        optionsBuilder.UseNpgsql("Host=localhost;Database=talyerstudio_joborders;Username=postgres;Password=postgres");
        
        return new JobOrderDbContext(optionsBuilder.Options);
    }
}