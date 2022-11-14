
namespace Demo.AspNetCore.Htmx.Extensions
{
    public static class ResponseExtension
    {
        public static void AddHxHeader(this HttpResponse response, string id, string mode, string data = null, int delay = 0)
        // https://htmx.org/reference/#response_headers
        {
            response.Headers.Add("HX-Trigger-After-Settle", "{\"evtHtmxHeaderTrigger\":" +
                System.Text.Json.JsonSerializer.Serialize(new
                {
                    id = id,
                    mode = mode,
                    data = data,
                    delay = delay
                }) +
            "}");
        }
    }
}
