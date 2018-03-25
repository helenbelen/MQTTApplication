using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebAPI.Models
{
    public partial class HomeAutomationContext : DbContext
    {
        public virtual DbSet<ConnectionInfo> ConnectionInfo { get; set; }
        public virtual DbSet<DeviceData> DeviceData { get; set; }
        public virtual DbSet<DeviceList> DeviceList { get; set; }

        public HomeAutomationContext(DbContextOptions<HomeAutomationContext> context): base(context)
        {
            var created = Database.EnsureCreated();
            if (created)
            {
                var ip = new Models.ConnectionInfo()
                {
                    InfoName = "mosquitto",
                    InfoString = MQTTCommon.Resources.brokerUrl,
                };
                ConnectionInfo.Add(ip);
                SaveChanges();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConnectionInfo>(entity =>
            {
                entity.HasKey(e => e.InfoId);

                entity.Property(e => e.InfoId)
                    .HasColumnName("InfoID")
                    .ValueGeneratedNever();

                entity.Property(e => e.InfoName)
                    .IsRequired()
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.InfoString)
                    .IsRequired()
                    .HasColumnType("text");
            });

            modelBuilder.Entity<DeviceData>(entity =>
            {
                entity.HasKey(e => e.DataId);

                entity.Property(e => e.DataId)
                    .HasColumnName("DataID")
                    .UseSqlServerIdentityColumn();

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.TimeStamp).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<DeviceList>(entity =>
            {
                entity.HasKey(e => e.DeviceId);

                entity.Property(e => e.DeviceId)
                    .HasColumnName("DeviceID")
                    .UseSqlServerIdentityColumn();

                entity.Property(e => e.DeviceLocation).HasMaxLength(60);

                entity.Property(e => e.DeviceName).HasMaxLength(60);
            });
        }
    }
}
