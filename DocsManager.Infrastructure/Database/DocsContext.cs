using DocsManager.Core.Domain.Documents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsManager.Infrastructure.Database
{
    public class DocsContext(DbContextOptions<DocsContext> options) : DbContext(options)
    {
        public virtual DbSet<Document> Documents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(d => d.Guid);

                entity.HasIndex(d => d.Code).IsUnique();
                entity.Property(d => d.Code).IsRequired();

                entity.Property(d => d.Title).IsRequired();

                entity.Property(d => d.Value)
                      .HasPrecision(18, 2);

                entity.Property(d => d.Revision)
                      .HasConversion<string>().IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
