using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CarePrideSystem.Application.Interfaces.Services;

namespace CarePrideSystem.Infrastructure.Services
{
    public class FileSystemStorageService : IFileStorageService
    {
        private readonly string _rootPath;
        private readonly ILogger<FileSystemStorageService> _logger;

        public FileSystemStorageService(IConfiguration config, ILogger<FileSystemStorageService> logger)
        {
            _rootPath = config["FileStorage:RootPath"]
                        ?? Path.Combine(AppContext.BaseDirectory, "Files");
            if (!Directory.Exists(_rootPath)) Directory.CreateDirectory(_rootPath);
            _logger = logger;
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string subFolder)
        {
            var ext = Path.GetExtension(fileName);
            var unique = $"{Guid.NewGuid():N}{ext}";
            var folder = Path.Combine(_rootPath, subFolder, DateTime.UtcNow.Year.ToString());
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            var fullPath = Path.Combine(folder, unique);
            using (var output = new FileStream(fullPath, FileMode.Create))
            {
                await fileStream.CopyToAsync(output);
            }

            var relative = Path.Combine(subFolder, DateTime.UtcNow.Year.ToString(), unique);
            _logger.LogInformation("Saved file: {Path}", relative);
            return relative.Replace('\\', '/');
        }

        public Task<Stream> GetFileAsync(string filePath)
        {
            var full = Resolve(filePath);
            if (!File.Exists(full)) throw new FileNotFoundException($"File not found: {filePath}");
            Stream stream = new FileStream(full, FileMode.Open, FileAccess.Read);
            return Task.FromResult(stream);
        }

        public Task DeleteFileAsync(string filePath)
        {
            var full = Resolve(filePath);
            if (File.Exists(full)) File.Delete(full);
            return Task.CompletedTask;
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(Resolve(filePath));
        }

        private string Resolve(string filePath)
        {
            var normalized = filePath.Replace('/', Path.DirectorySeparatorChar);
            var full = Path.GetFullPath(Path.Combine(_rootPath, normalized));
            var rootFull = Path.GetFullPath(_rootPath);
            if (!full.StartsWith(rootFull, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Invalid file path.");
            return full;
        }
    }
}


