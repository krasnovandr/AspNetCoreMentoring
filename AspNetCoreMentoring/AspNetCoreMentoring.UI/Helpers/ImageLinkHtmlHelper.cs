using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.UI.Helpers
{
    public static class ImageLinkHtmlHelper
    {
        public static HtmlString NorthwindImageLink(this IHtmlHelper html, int imageId, string linkName)
        {
            string result = $"<a href='images/{imageId}'>{linkName}</a>";

            return new HtmlString(result);
        }
    }
}
