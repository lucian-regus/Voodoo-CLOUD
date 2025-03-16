using System.Linq.Expressions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Domain.Database;

public class DatabaseContext : DbContext
{
    public DbSet<BlacklistedIpAddress> BlacklistedIpAddresses { get; set; }
    public DbSet<MalwareSignature> MalwareSignatures { get; set; }
    public DbSet<YaraRule> YaraRules { get; set; }
    public DbSet<ScrapingLog> ScrapingLogs { get; set; }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            AddEntityQueryFilter(builder, entityType);
            
            foreach (var mutableForeignKey in entityType.GetForeignKeys())
                mutableForeignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
    
    private static void AddEntityQueryFilter(ModelBuilder builder, IReadOnlyTypeBase entityType)
    {
        var type = entityType.ClrType;
        if (type.IsSubclassOf(typeof(EntityBase)))
        {
            var parameter = Expression.Parameter(type);
            var propertyInfo = Expression.Property(parameter, "DeletedAt");
            var nullConstant = Expression.Constant(null, typeof(DateTime?));
            var equalExpression = Expression.Equal(propertyInfo, nullConstant);
            var filter = Expression.Lambda(equalExpression, parameter);
            builder.Entity(type).HasQueryFilter(filter).HasIndex("DeletedAt");
        }
    }
}