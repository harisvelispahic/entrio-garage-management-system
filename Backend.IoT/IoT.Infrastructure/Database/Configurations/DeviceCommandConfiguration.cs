using IoT.Domain.Entities.Devices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoT.Infrastructure.Database.Configurations;

public class DeviceCommandConfiguration : IEntityTypeConfiguration<DeviceCommandEntity>
{
    public void Configure(EntityTypeBuilder<DeviceCommandEntity> builder)
    {
        builder.ToTable("DeviceCommands");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CommandType)
            .IsRequired()
            .HasMaxLength(50);

    }
}
