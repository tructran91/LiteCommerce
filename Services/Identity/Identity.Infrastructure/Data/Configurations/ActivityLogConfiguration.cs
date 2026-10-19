using Identity.Core.Constants;
using Identity.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configurations
{
    public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
    {
        public void Configure(EntityTypeBuilder<ActivityLog> builder)
        {
            builder.Property(x => x.Action).HasConversion<string>().HasMaxLength(LogFieldLength.Action);
            builder.Property(x => x.FailureReason).HasConversion<string>().HasMaxLength(LogFieldLength.FailureReason);
            builder.Property(x => x.Email).HasMaxLength(LogFieldLength.Email);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(LogFieldLength.Description);
            builder.Property(x => x.IpAddress).HasMaxLength(LogFieldLength.IpAddress);
            builder.Property(x => x.UserAgent).HasMaxLength(LogFieldLength.UserAgent);
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(LogFieldLength.UserName);
            builder.Property(x => x.CorrelationId).IsRequired().HasMaxLength(LogFieldLength.CorrelationId);

            builder.HasIndex(x => x.Timestamp);
            builder.HasIndex(x => x.Email);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.CorrelationId);
        }
    }
}
