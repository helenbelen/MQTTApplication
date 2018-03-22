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

        public HomeAutomationContext(DbContextOptions<HomeAutomationContext> options): base(options)
        {
            Database.EnsureCreated();
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

                entity.Property(e => e.DeviceId)
                    .ValueGeneratedNever();

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.Timestamp).HasColumnType("smalldatetime");
            });

            modelBuilder.Entity<DeviceList>(entity =>
            {
                entity.HasKey(e => e.DeviceId);

                entity.Property(e => e.DeviceId)
                    .HasColumnName("DeviceID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DeviceLocation).HasColumnType("varchar").HasMaxLength(60);

                entity.Property(e => e.DeviceName).HasColumnType("varchar").HasMaxLength(60);

                entity.HasOne(d => d.Device)
                    .WithOne(p => p.InverseDevice)
                    .HasForeignKey<DeviceList>(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceList_DeviceList");
            });
        }
    }
}
