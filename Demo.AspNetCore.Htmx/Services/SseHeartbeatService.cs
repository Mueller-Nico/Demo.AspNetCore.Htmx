using Lib.AspNetCore.ServerSentEvents;

namespace Demo.AspNetCore.Htmx.Services
{
    internal class SseHeartbeatService : BackgroundService
    {
        private const string HEARTBEAT_MESSAGE_FORMAT = "Lib.AspNetCore.ServerSentEvents Heartbeat ({0} UTC)";
        private readonly IServerSentEventsService _serverSentEventsService;

        public SseHeartbeatService(IServerSentEventsService serverSentEventsService)
        {
            _serverSentEventsService = serverSentEventsService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _serverSentEventsService.SendEventAsync(System.Text.Json.JsonSerializer.Serialize(new { Message = $"{string.Format(HEARTBEAT_MESSAGE_FORMAT, DateTime.UtcNow)}" }));

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }
}
