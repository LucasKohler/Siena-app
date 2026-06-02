using Microsoft.EntityFrameworkCore;
using Siena.Infrastructure.Persistence.Entities;

namespace Siena.Infrastructure.Persistence;

public sealed class SienaDbContext : DbContext
{
    public SienaDbContext(DbContextOptions<SienaDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();

    public DbSet<EventEntity> Events => Set<EventEntity>();

    public DbSet<VideoEntity> Videos => Set<VideoEntity>();

    public DbSet<AttendanceEntity> Attendances => Set<AttendanceEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(user => user.Id);
            entity.Property(user => user.Id).HasMaxLength(64);
            entity.Property(user => user.PhoneNumber).HasMaxLength(32).IsRequired();
            entity.Property(user => user.DisplayName).HasMaxLength(128).IsRequired();
            entity.Property(user => user.Role).HasMaxLength(32).IsRequired();
            entity.Property(user => user.Position).HasMaxLength(32);
            entity.Property(user => user.IsActive).HasDefaultValue(true);
            entity.HasIndex(user => user.PhoneNumber).IsUnique();
        });

        modelBuilder.Entity<EventEntity>(entity =>
        {
            entity.ToTable("events");
            entity.HasKey(eventItem => eventItem.Id);
            entity.Property(eventItem => eventItem.Id).HasMaxLength(128);
            entity.Property(eventItem => eventItem.Title).HasMaxLength(256).IsRequired();
            entity.Property(eventItem => eventItem.Type).HasMaxLength(64).IsRequired();
            entity.Property(eventItem => eventItem.Category).HasMaxLength(32).IsRequired();
            entity.Property(eventItem => eventItem.Location).HasMaxLength(128).IsRequired();
            entity.Property(eventItem => eventItem.Opponent).HasMaxLength(128);
            entity.Property(eventItem => eventItem.Description).HasMaxLength(1024);
        });

        modelBuilder.Entity<VideoEntity>(entity =>
        {
            entity.ToTable("videos");
            entity.HasKey(video => video.Id);
            entity.Property(video => video.Id).HasMaxLength(128);
            entity.Property(video => video.Title).HasMaxLength(256).IsRequired();
            entity.Property(video => video.Url).HasMaxLength(512).IsRequired();
        });

        modelBuilder.Entity<AttendanceEntity>(entity =>
        {
            entity.ToTable("attendances");
            entity.HasKey(attendance => new { attendance.EventId, attendance.UserId });
            entity.Property(attendance => attendance.EventId).HasMaxLength(128);
            entity.Property(attendance => attendance.UserId).HasMaxLength(64);
            entity.Property(attendance => attendance.Status).HasMaxLength(16).IsRequired();
            entity.Property(attendance => attendance.ApprovalStatus).HasMaxLength(16).IsRequired();

            entity.HasOne(attendance => attendance.Event)
                .WithMany(eventItem => eventItem.Attendances)
                .HasForeignKey(attendance => attendance.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(attendance => attendance.User)
                .WithMany(user => user.Attendances)
                .HasForeignKey(attendance => attendance.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
