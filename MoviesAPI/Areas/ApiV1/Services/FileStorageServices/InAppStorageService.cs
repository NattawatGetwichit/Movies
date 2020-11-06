using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MoviesAPI.Areas.ApiV1.Services.FileStorageServices
{
    public class InAppStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContext;

        public InAppStorageService(IWebHostEnvironment env, IHttpContextAccessor httpContext)
        {
            _env = env;
            _httpContext = httpContext;
        }

        public Task DeleteFile(string fileRoute, string containerName)
        {
            var fileName = Path.GetFileName(fileRoute);
            string fileDirectory = Path.Combine(_env.WebRootPath, containerName, fileName);

            if (File.Exists(fileDirectory))
            {
                File.Delete(fileDirectory);
            }

            return Task.FromResult(0);
        }

        public async Task<string> EditFile(byte[] content, string extension, string containerName, string fileRoute, string contentType)
        {
            if (!string.IsNullOrEmpty(fileRoute))
            {
                await DeleteFile(fileRoute, containerName);
            }

            return await SaveFile(content, extension, containerName, contentType);
        }

        public async Task<string> SaveFile(byte[] content, string extension, string containerName, string contentType)
        {
            var fileName = $"{Guid.NewGuid()}{ extension}";
            string folder = Path.Combine(_env.WebRootPath, containerName);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string savingPath = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(savingPath, content);

            var currentUrl = $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}";
            var pathForDatabase = Path.Combine(currentUrl, containerName, fileName);

            return pathForDatabase;
        }
    }
}