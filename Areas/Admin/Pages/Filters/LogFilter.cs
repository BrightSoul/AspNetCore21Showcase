using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AspNetCore21Showcase.Areas.Admin.Pages.Filters {
    [AttributeUsage(AttributeTargets.Class)]
    public class LogFilter : Attribute, IPageFilter
    {
        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            Log($"In base all'url, è stata selezionata la Razor Page {context.ActionDescriptor.AreaName}/{context.ActionDescriptor.DisplayName}", context.HttpContext);
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            Log($"Sto per eseguire la Razor Page {context.ActionDescriptor.AreaName}/{context.ActionDescriptor.DisplayName}", context.HttpContext);
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            Log($"Ho eseguito la Razor Page {context.ActionDescriptor.AreaName}/{context.ActionDescriptor.DisplayName}", context.HttpContext);
        }

        private ILogger logger;
        private void Log(string testo, HttpContext httpContext)
        {
            if (logger == null) {
                var loggerFactory = httpContext.RequestServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
                logger = loggerFactory.CreateLogger<LogFilter>();
            }
            logger.LogInformation(testo);
        }
    }
}