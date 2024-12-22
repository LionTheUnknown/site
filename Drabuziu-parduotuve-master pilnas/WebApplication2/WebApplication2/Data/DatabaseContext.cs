using System.Collections.Generic;
using System.Data.Entity;
using WebApplication2.Models;
namespace WebApplication2.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=DatabaseContext")
        {
        }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Buyers> Buyers { get; set; }
        public DbSet<Sellers> Sellers { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseProduct> WarehouseProducts { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductCategory>()
            .ToTable("kategorijaProduktas")
            .HasKey(pc => new { pc.ProductId, pc.CategoryId });
            modelBuilder.Entity<ProductCategory>()
                .HasRequired(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasRequired(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);
            modelBuilder.Entity<Product>()
            .HasMany(p => p.ProductCategories)
            .WithRequired(pc => pc.Product);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.ProductCategories)
                .WithRequired(pc => pc.Category);
            /*modelBuilder.Entity<Category>()
            .HasMany(p => p.Products)
            .WithMany(c => c.Categories)
            .Map(m =>
            {
                m.ToTable("kategorijaProduktas");
                m.MapLeftKey("CategoryId");
                m.MapRightKey("CategoryId");
            });*/
            /*modelBuilder.Entity<Product>()
            .HasMany(p => p.Categories)
            .WithMany(c => c.Products)
            .UsingEntity<Dictionary<string, object>>("ProductCategory",
            j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
            j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId"));*/
            /*
            modelBuilder.Entity<ProductCategory>()
            .HasKey(pc => new { pc.ProductId, pc.CategoryId }).Has(); // Composite key
            */
            modelBuilder.Entity<WarehouseProduct>()
            .HasKey(pc => new { pc.ProductId, pc.WarehouseId}); // Composite key
            /*
            modelBuilder.Entity<ProductCategory>()
                .HasRequired(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);
            */
            /*
             modelBuilder.Entity<Product>()
             .HasMany(p => p.Categories)
             .WithMany(c => c.Products)
             .Map(m =>
             {
                 m.ToTable("produktas_kategorija");
                 m.MapLeftKey("ProductId");
                 m.MapRightKey("CategoryId");
             });
            */
            /*
            modelBuilder.Entity<ProductCategory>()
            .HasKey(pc => new { pc.ProductId, pc.CategoryId });  // Composite primary key
            */

            /*
            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);*/
            modelBuilder.Entity<OrderProduct>()
            .HasKey(pc => new { pc.ProductId, pc.OrderId }); // Composite key



        }

		public System.Data.Entity.DbSet<WebApplication2.Models.Coupon> Coupons { get; set; }

		/*
		// Configure the many-to-many relationship using the ProductCategory join table
		modelBuilder.Entity<ProductCategory>()
			.HasKey(pc => new { pc.ProductId, pc.CategoryId }); // Composite key

		modelBuilder.Entity<ProductCategory>()
			.HasRequired(pc => pc.Product)
			.WithMany(p => p.ProductCategories)
			.HasForeignKey(pc => pc.ProductId);

		modelBuilder.Entity<ProductCategory>()
			.HasRequired(pc => pc.Category)
			.WithMany(c => c.ProductCategories)
			.HasForeignKey(pc => pc.CategoryId);*/


	}
}
