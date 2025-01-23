namespace TechStore.Services
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file, string[] allowedExtensions);
       Tuple<int, string> SaveImage(IFormFile imageFile);
        public bool DeleteImage(string imageFileName);
    }
}