using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AspNetCoreMentoring.UI.ViewComponents
{
    public class Breadcrumb
    {
        public Breadcrumb(string area, string controller, string action, string title,object id)
        {
            Area = area;
            Controller = controller;
            Action = action;
            Id = id;

            if (string.IsNullOrWhiteSpace(title))
            {
                var result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                        string.Equals(action, "Index", StringComparison.OrdinalIgnoreCase) ? controller : action);
                Title = Regex.Replace(result, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
            }
            else
            {
                Title = title;
            }
        }

        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public object Id { get; set; }
        public string Title { get; set; }
    }
}
