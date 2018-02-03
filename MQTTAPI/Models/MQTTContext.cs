using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MQTTAPI.Models
{
    public partial class MQTTContext : DbContext
    {
        public MQTTContext(DbContextOptions<MQTTContext> options):base(options)
        {

        }
        public virtual DbSet<DeviceData> DeviceData { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeviceData>(entity =>
            {
                entity.HasKey(e => e.DeviceId);

                entity.Property(e => e.DeviceId)
                    .HasColumnName("DeviceID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasColumnType("char(10)");

                entity.Property(e => e.Timestamp).HasColumnType("smalldatetime");
            });
        }
    }
}
