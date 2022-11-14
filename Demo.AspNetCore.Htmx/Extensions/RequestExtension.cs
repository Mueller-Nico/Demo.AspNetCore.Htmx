using System.Net.Http.Headers;

namespace Demo.AspNetCore.Htmx.Extensions
{
    public static class RequestExtension
    {
        public static bool IsHtmxRequest(this HttpRequest request)
        {
            bool isHtmx = request.Headers.TryGetValue("HX-Request", out var value) is true;
            if (isHtmx)
            {
                return !IsHtmxHistoryRestoreRequest(request);
            }
            return isHtmx;
        }
        public static bool IsHtmxHistoryRestoreRequest(this HttpRequest request)
        {
            return request.Headers.TryGetValue("HX-History-Restore-Request", out var value) is true;
        }
    }
}
