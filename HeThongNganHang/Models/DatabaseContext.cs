using Microsoft.EntityFrameworkCore;

namespace HeThongNganHang.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<TransactionDetail> TransactionDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccId).HasName("PK__Account__91CBC39847A843D3");

            entity.ToTable("Account");

            entity.Property(e => e.AccId).HasColumnName("AccID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TransactionDetail>(entity =>
        {
            entity.HasKey(e => e.TransId).HasName("PK__Transact__9E5DDB1CAD63C0D1");

            entity.Property(e => e.TransId).HasColumnName("TransID");
            entity.Property(e => e.AccId).HasColumnName("AccID");

            entity.HasOne(d => d.Acc).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.AccId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__AccID__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
