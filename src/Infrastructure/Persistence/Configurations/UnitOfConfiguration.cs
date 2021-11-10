using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class UnitOfConfiguration : IEntityTypeConfiguration<UnitOf>
    {
        public void Configure(EntityTypeBuilder<UnitOf> builder)
        {
            
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(200);


        }
    }
}
