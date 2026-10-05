namespace CarePrideSystem.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string subFolder);
        Task<Stream> GetFileAsync(string filePath);
        Task DeleteFileAsync(string filePath);
        bool FileExists(string filePath);
    }
}
