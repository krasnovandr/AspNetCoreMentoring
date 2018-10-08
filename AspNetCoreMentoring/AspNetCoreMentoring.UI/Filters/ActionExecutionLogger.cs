using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace AspNetCoreMentoring.UI.Filters
{
    public class ActionExecutionLoggerFilter : IActionFilter
    {
        private readonly ILogger _logger;

        public bool LogOnStart { get; set; }
        public bool LogOnStop { get; set; }

        public ActionExecutionLoggerFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ActionExecutionLoggerFilter>();
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (LogOnStart)
            {
                Log("Action Started", context.RouteData);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (LogOnStop)
            {
                Log("Action Finished", context.RouteData);
            }
        }

        private void Log(string methodName, RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = $"{methodName} controller:{controllerName} action:{actionName}";

            _logger.LogInformation(message);
        }
    }
}
