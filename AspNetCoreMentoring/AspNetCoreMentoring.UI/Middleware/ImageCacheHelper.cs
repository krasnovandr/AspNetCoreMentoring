using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.UI.Middleware
{
    public class ImageCacheHelper : IImageCacheHelper
    {
        private IHostingEnvironment _env;
        private CachingOptions _cachingOptions;
        private DateTime _cacheInvalidateTime;

        public ImageCacheHelper(IHostingEnvironment env)
        {
            _env = env;
        }

        public string ImageExtension { get; } = "Jpeg";
        public string CacheFolder { set; get; }

        public void InitializeCache(CachingOptions options)
        {
            _cachingOptions = options;
            CacheFolder = Path.Combine(_env.WebRootPath, _cachingOptions.CacheDirectory);

            if (Directory.Exists(CacheFolder) == false)
            {
                Directory.CreateDirectory(CacheFolder);
            }

            RefreshCacheInvalidationPeriod();

        }

        public int GetCachedImagesCount()
        {
            if (Directory.Exists(CacheFolder))
            {
                return Directory.GetFiles(CacheFolder).Length;
            }

            return 0;
        }

        public string TryGetImage(string imageId)
        {
            var fileExists = Directory.EnumerateFiles(CacheFolder)
                .FirstOrDefault(fileName => Path.GetFileName(fileName) == $"{ imageId}.{ImageExtension}");

            return fileExists;
        }

        public void CleanCache()
        {
            if (Directory.Exists(CacheFolder))
            {
                var directory = new DirectoryInfo(CacheFolder);

                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
            }
        }

        public void CacheImage(string imageId, byte[] imageBytes)
        {
            File.WriteAllBytes($"{CacheFolder}/{imageId}.{ImageExtension}", imageBytes);
        }


        public void RefreshCacheInvalidationPeriod()
        {
            _cacheInvalidateTime = DateTime.Now.Add(_cachingOptions.CacheDuration);
        }


        public void InvalidateCacheIfRequired()
        {
            if (DateTime.Now > _cacheInvalidateTime)
            {
                CleanCache();
                RefreshCacheInvalidationPeriod();
            }
        }
    }
}
