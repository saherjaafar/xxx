using Core.Dto;
using Core.Dto.TResponse;
using Core.Models;

namespace Core.Repositories
{
    public interface IServiceGalleryRepository : IBaseRepository<ServiceGallery>
    {
        TResponseVM<GalleryImagesListDto> GetGallery(long serviceId);
        TResponseVM<ResponseVM> UploadGalleryImage(UploadImageDetailsDto details, GalleryImageDto image);
        TResponseVM<ResponseVM> Delete(long imageId);
    }
}
