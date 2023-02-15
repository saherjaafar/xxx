using Common.UploadService;
using Core.Dto.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace DataAccess
{
    public class UploadService : IUploadService
    {
        private IConfiguration _configuration;
        public UploadService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <returns></returns>
        public string UploadDynamicImage(IFormFile Image, string extraPath)
        {
            try
            {
                if (Image != null)
                {

                    //_googleDrive.UploadImageToGoogleDrive(Image, "ss", "ss");

                    //Getting FileName
                    var fileName = Path.GetFileName(Image.FileName);

                    //Assigning Unique Filename (Guid)
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);


                    // concatenating  FileName + FileExtension
                    var newFileName = String.Concat(myUniqueFileName, fileExtension);

                    //Create a Folder.

                    //string path = Path.Combine(@"D:\Saher\DeliveryPro\DeliveryProApiRepo\DeliveryApi\DeliveryApi\wwwroot\Upload", extraPath);
                    string path = Path.Combine(_configuration["ApiRootFolder"], extraPath);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string filePath = Path.Combine(path, newFileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        Image.CopyTo(stream);
                    }
                    return (extraPath + "\\" + newFileName).Replace('\\', '/');

                }
                else return "Can't insert null value";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public bool CheckFileExtention(IFormFile file)
        {
            if (file != null)
            {
                //Getting FileName
                var fileName = Path.GetFileName(file.FileName);

                //Getting file Extension
                var fileExtension = Path.GetExtension(fileName);

                if (fileExtension != ".jpg" && fileExtension != ".png")
                {
                    return false;
                }
                else
                    return true;
            }
            return false;
        }

        public bool CheckIfImageValidity(IFormFile image)
        {
            try
            {
                try
                {
                    string imageInBase64 = "";
                    using (var ms = new MemoryStream())
                    {
                        image.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        imageInBase64 = Convert.ToBase64String(fileBytes);
                    }
                    byte[] bytes = Convert.FromBase64String(imageInBase64);

                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        Image image1 = Image.FromStream(ms);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void DownloadImage(string uri,string path,string folder,string fileName,string extension)
        {
            //client.DownloadFile(new Uri(url), @"c:\temp\image35.png");
            //string combinedPath = Path.Combine("C:\\Wedcoo\\Wedcoo-Api-v2\\Wedcoo-Api\\wwwroot\\Uploads\\ServiceCategory", "test");

            string combinedPath = Path.Combine(path, folder);
            using (WebClient client = new WebClient())
            {
                if (!Directory.Exists(combinedPath))
                {
                    Directory.CreateDirectory(combinedPath);
                }
                string xx = @"" + path + "/" + folder + "/" + fileName + "." + extension;
                client.DownloadFile(new Uri(uri),@""+path+"/"+folder+"/"+fileName+"."+extension);
            }

        }

        public void DownloadImage1(string uri, string path, string folder, string fileName, string extension)
        {
            //client.DownloadFile(new Uri(url), @"c:\temp\image35.png");
            //string combinedPath = Path.Combine("C:\\Wedcoo\\Wedcoo-Api-v2\\Wedcoo-Api\\wwwroot\\Uploads\\ServiceCategory", "test");

            try
            {
                string combinedPath = Path.Combine(path, folder);
                using (WebClient client = new WebClient())
                {
                    if (!Directory.Exists(combinedPath))
                    {
                        Directory.CreateDirectory(combinedPath);
                    }
                    string xx = @"" + path + folder + "/" + fileName + "." + extension;
                    client.DownloadFile(new Uri(uri), @"" + path + folder + "/" + fileName + "." + extension);
                }
            }
            catch(Exception ex)
            {
                var x= uri.ToString();
            }

        }

    }
}
