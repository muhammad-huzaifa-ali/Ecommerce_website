using Microsoft.EntityFrameworkCore;
namespace Ecommerce_website.Models

{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }

        public DbSet<Admin> Tbl_Admin { get; set; }
        public DbSet<Customer> Tbl_Customer { get; set; }
        public DbSet<Category> Tbl_Category { get; set; }
        public DbSet<Product> Tbl_Product { get; set; }
        public DbSet<Cart> Tbl_Cart { get; set; }
        public DbSet<Order> Tbl_Order { get; set; }
        public DbSet<Feedback> Tbl_Feedback { get; set; }
      


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasOne(p => p.category)
                .WithMany(c => c.product)
                .HasForeignKey(p => p.category_id);


        }


    }
}