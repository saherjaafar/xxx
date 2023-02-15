using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryDistrict>()
                .HasKey(bc => new { bc.CountryDistrictId, bc.CategoryId });

            modelBuilder.Entity<CategoryDistrict>()
                .HasOne(bc => bc.Category)
                .WithMany(b => b.CategoryDistricts)
                .HasForeignKey(bc => bc.CategoryId);

            modelBuilder.Entity<CategoryDistrict>()
                .HasOne(bc => bc.CountryDistrict)
                .WithMany(c => c.CategoryDistricts)
                .HasForeignKey(bc => bc.CountryDistrictId);

            modelBuilder.Entity<DealDistrict>()
                .HasKey(bc => new { bc.CountryDistrictId, bc.DealId });

            modelBuilder.Entity<DealDistrict>()
                .HasOne(bc => bc.Deal)
                .WithMany(b => b.DealDistricts)
                .HasForeignKey(bc => bc.DealId);

            modelBuilder.Entity<ApplicationFollowUp>()
                .HasOne<Application>(s => s.Application)
                .WithMany(g => g.ApplicationFollowUps)
                .HasForeignKey(s => s.ApplicationId);
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryDistrict> CountryDistricts { get; set; }
        public DbSet<DistrictState> DistrictStates { get; set; }
        public DbSet<Icon> Icons { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryDistrict> CategoryDistricts { get; set; }
        public DbSet<CategoryFilter> CategoryFilters { get; set; }
        public DbSet<AdminRole> AdminRoles { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<AdminVerification> AdminVerification { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationFollowUp> ApplicationFollowUps { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceGallery> ServiceGalleries { get; set; }
        public DbSet<ServiceSocialMedia> ServiceSocialMedias { get; set; }
        public DbSet<ServicePackage> ServicePackages { get; set; }
        public DbSet<ServicePackageAttribute> ServicePackageAttributes { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<DealDistrict> DealDistricts { get; set; }
        public DbSet<DealVariation> DealVariations { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<SponsorType> SponsorTypes { get; set; }
        public DbSet<ServicePayment> ServicePayments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFavorite> UserFavorites { get; set; }
    }


}
