using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MQTTDatabaseApp.Entities
{
    public partial class MQTTContext : DbContext
    {
        public virtual DbSet<DeviceData> DeviceData { get; set; }

     
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#warning To protect potentially sensitive information in your connection string,you should move it out of source code.See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings
            optionsBuilder.UseSqlServer(@"Server = mosquittodatabase.cffo0eijbrmb.us - east - 1.rds.amazonaws.com,1433; Database = MQTT; Integrated Security = False; User ID = admin; Password = mosquitto; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False; ");

        }
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
