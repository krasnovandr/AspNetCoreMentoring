using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.UI.ViewComponents
{
    public class BreadCrumpViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("BreadCrump");
        }
    }
}
