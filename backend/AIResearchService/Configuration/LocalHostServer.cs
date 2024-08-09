namespace AIResearchService.Configuration;

public class LocalHostServer(string url) : HttpClientHandler
{
    private readonly Uri uri = new Uri(url);
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.RequestUri = uri;
        return base.SendAsync(request, cancellationToken);
    }
}
