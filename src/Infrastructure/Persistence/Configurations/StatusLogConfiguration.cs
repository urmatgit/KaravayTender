using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class StatusLogConfiguration : IEntityTypeConfiguration<StatusLog>
    {
        public void Configure(EntityTypeBuilder<StatusLog> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            

        }
    }
}
