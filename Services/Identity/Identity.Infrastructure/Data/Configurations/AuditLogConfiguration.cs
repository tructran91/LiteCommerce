using Identity.Core.Constants;
using Identity.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.Property(x => x.EntityName).IsRequired().HasMaxLength(LogFieldLength.EntityName);
            builder.Property(x => x.EntityId).IsRequired().HasMaxLength(LogFieldLength.EntityId);
            builder.Property(x => x.Action).HasConversion<string>().HasMaxLength(LogFieldLength.Action);
            builder.Property(x => x.Changes).IsRequired();
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(LogFieldLength.UserName);
            builder.Property(x => x.CorrelationId).IsRequired().HasMaxLength(LogFieldLength.CorrelationId);

            builder.HasIndex(x => x.Timestamp);
            builder.HasIndex(x => new { x.EntityName, x.EntityId });
            builder.HasIndex(x => x.CorrelationId);
        }
    }
}
