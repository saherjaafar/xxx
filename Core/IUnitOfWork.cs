using Core.Repositories;
using System;

namespace Core
{
    public interface IUnitOfWork : IDisposable
    {
        ICountryRepository Countries { get; }
        IIconRepository Icons { get; }
        IAdminRepository Admins { get; }
        IAdminRoleRepository AdminRoles { get; }
        IAdminVerification AdminVerifications { get; }
        ICategoryRepository Categories { get; }
        IFilterRepository Filters { get; }
        IApplicationRepository Applications { get; }
        IServiceRepository Services { get; }
        IServiceGalleryRepository ServiceGalleries { get; }
        IServicePackageRepository ServicePackages { get; }
        IServicePackageAttributeRepository ServicePackageAttributes { get; }
        IServiceSocialMediaRepository ServiceSocialMedias { get; }
        IDealRepository Deals { get; }
        ISponsorRepository Sponsors { get; }
        IServicePaymentRepository ServicePayments { get; }
        IUserRepository Users { get; }
        IUserFavoriteRepository UserFavorites { get; }
        void SaveChanges();
    }
}
