using Liga_IT.Domain.Entities;
using Liga_IT.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Liga_IT.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationIdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Club> Club { get; set; }
    public DbSet<Player> Player { get; set; }
    public DbSet<Match> Match { get; set; }
    public DbSet<Referee> Referee { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationIdentityUser>(entity =>
        {
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
            entity.Property(e => e.StadiumName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreateAt).IsRequired();

            entity.HasMany(e => e.Players).WithOne(e => e.Club).HasForeignKey(e => e.ClubId).OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.HomeMatches).WithOne(e => e.HomeClub).HasForeignKey(e => e.HomeClubId).OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.AwayMatches).WithOne(e => e.AwayClub).HasForeignKey(e => e.AwayClubId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Position).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Nationality).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Height).HasPrecision(5, 2);
            entity.Property(e => e.Weight).HasPrecision(5, 2);
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.DateOfBirth).IsRequired();
            entity.Property(e => e.JoinedClubDate).IsRequired();
            entity.Property(e => e.CreateAt).IsRequired();
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.MatchDate).IsRequired();
            entity.Property(e => e.CreateAt).IsRequired();

            entity.HasOne(e => e.Referee).WithMany(e => e.Matches).HasForeignKey(e => e.RefereeId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Referee>(entity =>
        {
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.Category).IsRequired();
            entity.Property(e => e.CreateAt).IsRequired();
        });
    }
}

