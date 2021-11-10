using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class VatConfiguration : IEntityTypeConfiguration<Vat>
    {
        public void Configure(EntityTypeBuilder<Vat> builder)
        {
            
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(t => t.Value)
                .IsRequired();
            


        }
    }
}
