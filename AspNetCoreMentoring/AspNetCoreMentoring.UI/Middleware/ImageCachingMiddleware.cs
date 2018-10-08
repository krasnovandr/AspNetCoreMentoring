using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreMentoring.UI.Middleware
{
    public class ImageCachingMiddleware
    {
        private readonly RequestDelegate _next;
        private CachingOptions _options;
        private IImageCacheHelper _cacheHelper;

        public ImageCachingMiddleware(
            RequestDelegate next,
            IHostingEnvironment env,
            IImageCacheHelper cacheHelper,
            CachingOptions options)
        {
            _next = next;
            _options = options;
            _cacheHelper = cacheHelper;
            _cacheHelper.InitializeCache(_options);
        }


        public async Task Invoke(HttpContext context, IImageCacheHelper cacheHelper)
        {
            var imageId = context.Request?.Path.Value.Split('/').Last();

            if (context.Request.Path.Value.Contains("Image"))
            {
                _cacheHelper.InvalidateCacheIfRequired();

                string fileExists = _cacheHelper.TryGetImage(imageId);

                if (string.IsNullOrEmpty(fileExists) == false)
                {
                    await context.Response.SendFileAsync($"{ _cacheHelper.CacheFolder}/{ imageId}.{ _cacheHelper.ImageExtension}");
                }
                else
                {
                    var imagesInCache = _cacheHelper.GetCachedImagesCount();

                    if (imagesInCache >= _options.MaxImages)
                    {
                        await _next(context);
                    }
                    else
                    {
                        await CacheImageAndProceed(context, imageId);
                    }
                }
            }
            else
            {
                await _next(context);
            }
        }



        private async Task CacheImageAndProceed(HttpContext context, string imageId)
        {
            using (var swapStream = new MemoryStream())
            {
                Stream originalBody = context.Response.Body;

                context.Response.Body = swapStream;

                await _next(context);

                if (context.Response != null &&
                     string.IsNullOrEmpty(context.Response.ContentType) == false &&
                     context.Response.ContentType.Contains("image"))
                {
                    _cacheHelper.CacheImage(imageId, swapStream.ToArray());
                    await ReturnOriginalResponse(context, swapStream, originalBody);
                }
            }
        }


        private static async Task ReturnOriginalResponse(HttpContext context, MemoryStream swapStream, Stream originalBody)
        {
            swapStream.Position = 0;
            await swapStream.CopyToAsync(originalBody);

            context.Response.Body = originalBody;
        }
    }
}



