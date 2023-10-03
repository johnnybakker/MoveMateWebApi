namespace MoveMate.API.Services
{
    public interface INotificationService
    {

        Task BroadcastAsync(string title, string body, IEnumerable<string> identifiers);
    }
}
