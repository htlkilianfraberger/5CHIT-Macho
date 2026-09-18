using Microsoft.EntityFrameworkCore;
using Transactions.Models;

namespace Transactions.Data;

public sealed class TransactionsContext : DbContext
{
    public TransactionsContext(DbContextOptions<TransactionsContext> options)
        : base(options)
    {
    }

    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("tickets");

            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Available).HasColumnName("available");
            entity.Property(e => e.Version)
                .HasColumnName("version")
                .HasColumnType("char(36)")
                .IsConcurrencyToken();
        });
    }
}
