using Bookshop.Models;
using System.IO;

namespace Bookshop.Utility
{
    public class ImageHelper
    {


        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string GetImagePath<T>(T obj)
        {
            string ProjectRootPath = _webHostEnvironment.WebRootPath;


            string ImagePath = typeof(T).Name switch
            {
                "Author" => WC.ImagePathAuthor,
                "Book" => WC.ImagePathBook,
                _ => throw new NotImplementedException()
            };



            return ProjectRootPath + ImagePath;
        }

        public string SaveImage<T>(T obj, string file, IFormFileCollection files)
        {
            string PathToImage = GetImagePath<T>(obj) + file;


            using (var fileStream = new FileStream(PathToImage, FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }

            return file;

        }

        public void DeleteImage<T>(T obj, string fileName)
        {
            string PathToImage = GetImagePath<T>(obj) + fileName;

            if (File.Exists(PathToImage))
            {
                File.Delete(PathToImage);
            }
        }

        public string CreateImageFileName(IFormFileCollection files)
        {
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);

            return fileName + extension;
        }

        


    }
}
