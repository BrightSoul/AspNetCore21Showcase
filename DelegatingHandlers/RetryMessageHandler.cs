using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore21Showcase.Services
{
    public class RetryMessageHandler : DelegatingHandler
    {
        private const int maximumRetries = 5;
        private const int delayBetweenRetries = 500; //ms
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            var retries = maximumRetries;
            while(true) {
                try {
                    var response = await base.SendAsync(request, cancellationToken);
                    //Solleviamo un'eccezione quando l'errore è >= 500 (colpa del server)
                    if ((int) response.StatusCode >= 500) {
                        throw new HttpRequestException();
                    }
                    return response;
                } catch (HttpRequestException) when (retries > 0) {
                    //Catturiamo l'eccezione finché abbiamo ancora tentativi disponibile
                    retries--;
                    await Task.Delay(delayBetweenRetries);
                }
            };
        }
    }
}