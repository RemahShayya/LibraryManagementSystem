
namespace LibraryManagementSystem.API.Services
{
    public class ImageServiceAuthor
    {
        private readonly IConfiguration _configuration;

        public ImageServiceAuthor(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string SaveImage(IFormFile file, string fileName)
        {
            List<string> allowedExtensions = _configuration
                .GetSection("ImageSettings:AllowedExtensions")
                .Get<List<string>>() ?? new List<string>();

            string extension = Path.GetExtension(file.FileName);
            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                return "Invalid file type.";
            }

            long size = file.Length;
            long maxFileSize = _configuration.GetValue<long>("ImageSettings:MaxFileSize");
            if (size > maxFileSize)
            {
                return "File size exceeds the limit.";
            }

            string folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..",
                "LibraryManagementSystem.Data",
                _configuration["ImageSettings:AuthorImageFolderPath"]);

            string filePath = Path.Combine(folderPath, fileName);

            using FileStream stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            return filePath;
        }

        public bool DeleteImageById(string fileName)
        {
            string folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..",
                "LibraryManagementSystem.Data",
                _configuration["ImageSettings:AuthorImageFolderPath"]);

            string filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }

        public string UpdateImage(IFormFile newFile, string oldFileName, Guid imageId)
        {
            DeleteImageById(oldFileName);

            string extension = Path.GetExtension(newFile.FileName);
            string newFileName = imageId.ToString() + extension;

            return SaveImage(newFile, newFileName);
        }

    }
}
