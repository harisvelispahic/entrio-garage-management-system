using IoT.Domain.Entities.Devices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoT.Infrastructure.Database.Configurations;

public class DeviceEventConfiguration : IEntityTypeConfiguration<DeviceEventEntity>
{
    public void Configure(EntityTypeBuilder<DeviceEventEntity> builder)
    {
        builder.ToTable("DeviceEvents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EventType)
            .IsRequired()
            .HasMaxLength(50);
    }
}
