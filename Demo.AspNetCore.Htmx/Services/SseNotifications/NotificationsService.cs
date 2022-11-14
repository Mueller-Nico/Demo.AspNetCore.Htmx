using Lib.AspNetCore.ServerSentEvents;
using System.Text.Encodings.Web;

namespace Demo.AspNetCore.Htmx.Services.SseNotifications
{
    //If you want to use Lib.AspNetCore.ServerSentEvents for production,
    //check out the demo https://github.com/tpeczek/Demo.AspNetCore.ServerSentEvents
    internal class NotificationsService : INotificationsService
    {
        private IServerSentEventsService _notificationsServerSentEventsService;
        private readonly HtmlEncoder htmlEncoder;
        public NotificationsService(IServerSentEventsService notificationsServerSentEventsService, HtmlEncoder htmlEncoder)
        {
            _notificationsServerSentEventsService = notificationsServerSentEventsService;
            this.htmlEncoder = htmlEncoder;
        }

        public Task SendNotificationAsync(string notification, string name, bool htmlEncode = true, string msgId = null)
        {
            if (string.IsNullOrEmpty(msgId))
            {
                msgId = Guid.NewGuid().ToString();
            }

            string msgType = string.IsNullOrEmpty(name) ? null : name;

            IList<string> list = new List<string>(notification.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None));

            if (htmlEncode)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = htmlEncoder.Encode(list[i]);
                }
            }

            ServerSentEvent serverSentEvent = new ServerSentEvent
            {
                Id = msgId,
                Type = msgType,
                Data = list
            };

            return _notificationsServerSentEventsService.SendEventAsync(serverSentEvent);
        }
    }
}

