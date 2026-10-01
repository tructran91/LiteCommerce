using Catalog.Core.Constants;
using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.Property(x => x.EntityName).HasMaxLength(FieldLength.LogEntityName).IsRequired();
            builder.Property(x => x.EntityId).HasMaxLength(FieldLength.LogEntityId).IsRequired();
            builder.Property(x => x.Action).HasConversion<string>().HasMaxLength(FieldLength.LogAction);
            builder.Property(x => x.Changes).IsRequired();
            builder.Property(x => x.UserName).HasMaxLength(FieldLength.LogUserName).IsRequired();
            builder.Property(x => x.CorrelationId).HasMaxLength(FieldLength.LogCorrelationId).IsRequired();

            builder.HasIndex(x => x.Timestamp);
            builder.HasIndex(x => new { x.EntityName, x.EntityId });
            builder.HasIndex(x => x.CorrelationId);
        }
    }
}
