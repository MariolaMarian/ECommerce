using System;
using Core.Entities.OrderAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShipToAdress, a => {
                a.WithOwner();
            });

            builder.Property(s => s.Status).HasConversion(
                o => o.ToString(),
                o => (OrderStatus) Enum.Parse(typeof(OrderStatus), o)
            );

            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.Property(i => i.Subtotal).HasColumnType("decimal(18,2)");
        }
    }
}