using System.Runtime.CompilerServices;

namespace EdgeCut.Services
{
    public interface IFileService
    {
        Task<(int, string)> FileUpload(string folder, IFormFile file);
        void DeleteFile(string folder, string file);
    }
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public void DeleteFile(string folder, string file)
        {
            throw new NotImplementedException();
        }

        public async Task<(int, string)> FileUpload(string folder, IFormFile file)
        {
            string folderPath = Path.Combine(_env.WebRootPath, "uploads", folder);
            string fileName = Guid.NewGuid().ToString() + file.FileName;
            string fullPath = Path.Combine(folderPath, fileName);
            if (file.Length / 1024 > 300)
            {
                return (0, "File's length must be less than 300 KB.");
            }

            if (!file.ContentType.Contains("image"))
            {
                return (0, "File's format must be an image.");
            }

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (FileStream stream = new(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return (1, fileName);
        }
    }
}
