using System.Threading.Tasks;

namespace Demo.AspNetCore.Htmx.Services.SseNotifications
{
    public interface INotificationsService
    {
        Task SendNotificationAsync(string notification, string name, bool htmlEncode = true, string msgId = null);
    }
}
