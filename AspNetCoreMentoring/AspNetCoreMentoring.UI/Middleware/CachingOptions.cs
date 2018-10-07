using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.UI.Middleware
{
    public class CachingOptions
    {
        public int MaxImages { get; set; }
        public string CacheDirectory { get; set; }
        public TimeSpan CacheDuration{ get; set; }
    }
}
