using System.Net.Http.Headers;

namespace CarePrideSystem.Web.Services
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;
        public AuthTokenHandler(IHttpContextAccessor accessor) { _accessor = accessor; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            var token = _accessor.HttpContext?.User.FindFirst("access_token")?.Value;
            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, ct);
        }
    }
}
