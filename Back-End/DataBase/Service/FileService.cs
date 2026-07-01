using Library.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DataBase.Repository
{
    public class FileService(IWebHostEnvironment _env) : IFileService
    {
        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            var rootPath = _env.WebRootPath;

            if (string.IsNullOrWhiteSpace(rootPath))
                rootPath = Path.Combine(_env.ContentRootPath, "wwwroot");

            Directory.CreateDirectory(rootPath);

            var uploadsFolder = Path.Combine(rootPath, "images", folderName);
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/images/{folderName}/{uniqueFileName}";
        }

        public void DeleteFile(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return;

            var rootPath = _env.WebRootPath;

            if (string.IsNullOrWhiteSpace(rootPath))
            {
                rootPath = Path.Combine(_env.ContentRootPath, "wwwroot");
            }

            var relativePath = fileUrl.TrimStart('/')
                                      .Replace('/', Path.DirectorySeparatorChar);

            var filePath = Path.Combine(rootPath, relativePath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}