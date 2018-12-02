using System.Threading.Tasks;

namespace AspNetCoreMentoring.Notification
{
    public interface IRazorViewToStringRenderer
    {
        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }
}