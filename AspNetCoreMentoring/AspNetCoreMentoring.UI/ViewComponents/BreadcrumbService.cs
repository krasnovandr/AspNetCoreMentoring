using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;

namespace AspNetCoreMentoring.UI.ViewComponents
{
    public class BreadcrumbService : IViewContextAware
    {
        private IList<Breadcrumb> breadcrumbs;

        public void Contextualize(ViewContext viewContext)
        {
            breadcrumbs = new List<Breadcrumb>();

            string area = $"{viewContext.RouteData.Values["area"]}";
            string controller = $"{viewContext.RouteData.Values["controller"]}";
            string action = $"{viewContext.RouteData.Values["action"]}";
            object id = viewContext.RouteData.Values["id"];
            string title = $"{viewContext.ViewData["Title"]}";

            breadcrumbs.Add(new Breadcrumb(area, controller, action, title, id));
        }

        public IList<Breadcrumb> GetBreadcrumbs()
        {
            return breadcrumbs;
        }
    }
}
