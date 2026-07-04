using System.Net.Http.Headers;
using VikashERP.Mobile.Services.Interfaces;
using VikashERP.Mobile.State;

namespace VikashERP.Mobile.Services;

public class JwtDelegatingHandler : DelegatingHandler
{
    private readonly AppStateService _appStateService;
    private readonly IServiceProvider _serviceProvider;
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public JwtDelegatingHandler(AppStateService appStateService, IServiceProvider serviceProvider)
    {
        _appStateService = appStateService;
        _serviceProvider = serviceProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _appStateService.GetTokenAsync();
        
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // Only one thread should try to refresh the token at a time
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                // Check if the token was already refreshed by another thread while this one was waiting
                var newToken = await _appStateService.GetTokenAsync();
                
                if (newToken != null && newToken != token)
                {
                    // Token was already refreshed, retry request with new token
                    var clonedReq = await CloneHttpRequestMessageAsync(request);
                    clonedReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
                    return await base.SendAsync(clonedReq, cancellationToken);
                }

                // Try to refresh the token using AuthService
                var authService = _serviceProvider.GetRequiredService<IAuthService>();
                var success = await authService.RefreshTokenAsync();

                if (success)
                {
                    var freshToken = await _appStateService.GetTokenAsync();
                    if (!string.IsNullOrEmpty(freshToken))
                    {
                        // Clone the request because HttpRequestMessage cannot be sent twice
                        var clonedRequest = await CloneHttpRequestMessageAsync(request);
                        clonedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", freshToken);
                        
                        // Retry the request
                        return await base.SendAsync(clonedRequest, cancellationToken);
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        return response;
    }

    private async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage req)
    {
        HttpRequestMessage clone = new HttpRequestMessage(req.Method, req.RequestUri);
        
        if (req.Content != null)
        {
            // We need to copy the content stream
            var ms = new MemoryStream();
            await req.Content.CopyToAsync(ms);
            ms.Position = 0;
            
            clone.Content = new StreamContent(ms);

            // Copy headers from original content
            if (req.Content.Headers != null)
            {
                foreach (var h in req.Content.Headers)
                {
                    clone.Content.Headers.Add(h.Key, h.Value);
                }
            }
        }
        
        clone.Version = req.Version;
        
        // Ensure options are copied (Options replaced Properties in .NET 5+)
        foreach (var prop in req.Options)
        {
            clone.Options.Set(new HttpRequestOptionsKey<object?>(prop.Key), prop.Value);
        }
        
        foreach (var header in req.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
        
        return clone;
    }
}
