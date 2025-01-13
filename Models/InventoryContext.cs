using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IMS.Models
{
    public partial class InventoryContext : DbContext
    {
        public InventoryContext()
        {
        }

        public InventoryContext(DbContextOptions<InventoryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code.
            // For example, using appsettings.json or environment variables
            optionsBuilder.UseSqlServer("Server=DESKTOP-UQ6MEMJ;Database=IMS;Integrated Security=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring Inventory entity
            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Inventor__F5FDE6D32A3AD93B");

                // Explicitly specify the schema here
                entity.ToTable("Inventory", "dbo");

                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Location).HasMaxLength(50);
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Inventory__Produ__44FF419A");
            });

            // Configuring Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED77F969CB");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");
                entity.Property(e => e.Category).HasMaxLength(50);
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.ProductName).HasMaxLength(100);
                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Products__Suppli__4222D4EF");
            });

            // Configuring Supplier entity
            modelBuilder.Entity<Supplier>(entity =>
            {

                entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE66694B464378C");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.ContactInfo).HasMaxLength(100);
                entity.Property(e => e.SupplierName).HasMaxLength(100);
            });

            // Configuring Transaction entity
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4B9772D00F");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
                entity.Property(e => e.Location).HasMaxLength(50);
                entity.Property(e => e.ProductId).HasColumnName("ProductID");
                entity.Property(e => e.TransactionDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.TransactionType).HasMaxLength(10);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Transacti__Produ__48CFD27E");
            });

            // Any additional configurations can be added here
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
