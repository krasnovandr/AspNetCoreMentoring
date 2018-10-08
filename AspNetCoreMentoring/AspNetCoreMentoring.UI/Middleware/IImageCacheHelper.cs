using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.UI.Middleware
{
    public interface IImageCacheHelper
    {
        void CleanCache();
        int GetCachedImagesCount();

        string TryGetImage(string imageId);

        void CacheImage(string imageId, byte[] imageBytes);
        void InitializeCache(CachingOptions options);

        void InvalidateCacheIfRequired();

        string ImageExtension { get; }
        string CacheFolder { set; get; }
    }
}
