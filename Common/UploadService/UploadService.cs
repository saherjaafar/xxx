using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using Microsoft.AspNetCore.Http;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net;

namespace Common.UploadService
{
    public class UploadService
    {
        private IConfiguration _configuration;
        public UploadService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Upload Dynamic Image
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="extraPath"></param>
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

        /// <summary>
        /// Check File Extention
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
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

        public void SaveImage(string imageUrl, string filename, ImageFormat format)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imageUrl);
            Bitmap bitmap; bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                bitmap.Save(filename, format);
            }

            stream.Flush();
            stream.Close();
            client.Dispose();
        }
    }
}
