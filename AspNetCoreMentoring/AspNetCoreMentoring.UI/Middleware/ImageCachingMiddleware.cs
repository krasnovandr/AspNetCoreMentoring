using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace AspNetCoreMentoring.UI.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ImageCachingMiddleware
    {
        private readonly RequestDelegate _next;
        private string _cacheDirectoryName = "CachedImages";
        private string _imageExtension = "Jpeg";
        private readonly string _wwwrootFolder;
        private CachingOptions _options;

        public ImageCachingMiddleware(
            RequestDelegate next,
            IHostingEnvironment env,
           CachingOptions options
            )
        {
            _next = next;
            _options = options;
            _wwwrootFolder = env.WebRootPath;
        }

        public async Task Invoke(HttpContext context)
        {
            var folderPath = Path.Combine(_wwwrootFolder, _cacheDirectoryName);
            var imageId = context.Request?.Path.Value.Split('/').Last();
            if (context.Request.Path.Value.Contains("Image"))
            {
                string fileExists = TryGetImage(imageId, folderPath);
                if (string.IsNullOrEmpty(fileExists) == false)
                {
                    await context.Response.SendFileAsync($"{ folderPath}/{ imageId}.{ _imageExtension}");
                }
                else
                {
                    using (var swapStream = new MemoryStream())
                    {
                        Stream originalBody = context.Response.Body;

                        context.Response.Body = swapStream;

                        await _next(context);

                        //swapStream.Position = 0;
                        //string responseBody = new StreamReader(swapStream).ReadToEnd();

                        if (context.Response != null &&
                             string.IsNullOrEmpty(context.Response.ContentType) == false &&
                             context.Response.ContentType.Contains("image"))
                        {
                            File.WriteAllBytes($"{folderPath}/{imageId}.{_imageExtension}", swapStream.ToArray());

                            await ReturnOriginalResponse(context, swapStream, originalBody);
                        }
                    }
                }
            }
            else
            {
                await _next(context);
            }
        }

        private string TryGetImage(string imageId, string folderPath)
        {
            if (Directory.Exists(folderPath) == false)
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileExists = Directory.EnumerateFiles(folderPath)
                .FirstOrDefault(fileName => Path.GetFileName(fileName) == $"{ imageId}.{_imageExtension}");
            return fileExists;
        }

        private static async Task ReturnOriginalResponse(HttpContext context, MemoryStream swapStream, Stream originalBody)
        {
            swapStream.Position = 0;
            await swapStream.CopyToAsync(originalBody);

            context.Response.Body = originalBody;
        }
    }
}



