using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.UI.Helpers
{
    [HtmlTargetElement("a", Attributes = "northwind-id")]
    public class ImageLinkTagHelper : TagHelper
    {
        public int NorthwindId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("href", $"/images/{NorthwindId}");
        }
    }
}
