using Catalog.Core.Constants;
using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
    {
        public void Configure(EntityTypeBuilder<ActivityLog> builder)
        {
            builder.Property(x => x.Action).HasConversion<string>().HasMaxLength(FieldLength.LogAction);
            builder.Property(x => x.EntityName).HasMaxLength(FieldLength.LogEntityName).IsRequired();
            builder.Property(x => x.EntityId).HasMaxLength(FieldLength.LogEntityId);
            builder.Property(x => x.EntityDisplayName).HasMaxLength(FieldLength.LogEntityDisplayName);
            builder.Property(x => x.Description).HasMaxLength(FieldLength.LogDescription).IsRequired();
            builder.Property(x => x.RequestName).HasMaxLength(FieldLength.LogRequestName).IsRequired();
            builder.Property(x => x.UserName).HasMaxLength(FieldLength.LogUserName).IsRequired();
            builder.Property(x => x.CorrelationId).HasMaxLength(FieldLength.LogCorrelationId).IsRequired();

            builder.HasIndex(x => x.Timestamp);
            builder.HasIndex(x => new { x.EntityName, x.EntityId });
            builder.HasIndex(x => x.CorrelationId);
        }
    }
}
