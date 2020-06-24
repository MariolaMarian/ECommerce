using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class ProductBrandConfiguration : IEntityTypeConfiguration<ProductBrand>
    {
        public void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).IsRequired();
            builder.Property(b => b.Name).IsRequired().HasMaxLength(100);
            builder.HasMany<Product>(b => b.Products).WithOne(p => p.ProductBrand).HasForeignKey(p => p.ProductBrandId);
        }
    }
}