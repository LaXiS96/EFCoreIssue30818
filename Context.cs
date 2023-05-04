using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCoreIssue30818
{
    internal class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EFCoreIssue30818;Integrated Security=True");
            optionsBuilder.AddInterceptors(new JsonDictionaryQueryInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDbFunction(DbFunctionsExtensions.JsonValueMethod, builder =>
            {
                builder.HasName("JSON_VALUE").IsBuiltIn().HasStoreType("nvarchar(4000)");
                // https://github.com/dotnet/efcore/issues/28393#issuecomment-1181498610
                builder.HasParameter("propertyReference").HasStoreType("nvarchar(max)");
                builder.HasParameter("path");
            });

            modelBuilder.Entity<Entity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Dictionary)
                    .HasConversion<JsonDictionaryValueConverter, JsonDictionaryValueComparer>();
            });
        }
    }
}
