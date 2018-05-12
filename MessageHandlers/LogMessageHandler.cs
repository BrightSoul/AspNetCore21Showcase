using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AspNetCore21Showcase.MessageHandlers
{
    public class LogMessageHandler : DelegatingHandler
    {
        private ILogger logger;
        public LogMessageHandler(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<LogMessageHandler>();
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var response = await base.SendAsync(request, cancellationToken);
            stopwatch.Stop();
            logger.LogInformation($"La richiesta a {request.RequestUri} ha impiegato {stopwatch.ElapsedMilliseconds}ms e si è conclusa con lo status code {response.StatusCode}");
            return response;
        }
    }
}