using Microsoft.AspNetCore.Builder;

namespace AspNetCoreMentoring.UI.Middleware
{
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ImageCachingMiddlewareExtensions
    {
        public static IApplicationBuilder UseImageCachingMiddleware(this IApplicationBuilder builder, CachingOptions options)
        {
            return builder.UseMiddleware<ImageCachingMiddleware>(options);
        }
    }
}
