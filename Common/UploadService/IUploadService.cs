using Microsoft.AspNetCore.Http;

namespace Common.UploadService
{
    public interface IUploadService
    {
        string UploadDynamicImage(IFormFile Image, string extraPath);
        bool CheckFileExtention(IFormFile file);
        bool CheckIfImageValidity(IFormFile image);
        void DownloadImage(string uri, string path, string folder, string fileName, string extension);
        void DownloadImage1(string uri, string path, string folder, string fileName, string extension);
    }
}
