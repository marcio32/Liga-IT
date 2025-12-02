using Microsoft.AspNetCore.SignalR;

namespace Liga_IT.WEB.Hubs
{
    public class ChatHub(ILogger<ChatHub> logger) : Hub
    {

        public async Task SendMessage(string user, string message)
        {
            logger.LogInformation($"Mensaje de {user}: {message}");
            await Clients.Others.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageToRoom(string roomName, string user, string message)
        {
            logger.LogInformation($"Mensaje en la sala {roomName} de {user}: {message}");
            await Clients.OthersInGroup(roomName).SendAsync("ReceiveMessage", user, message, DateTime.Now);
        }

        public async Task NotifyTyping(string roomName, string user)
        {
            await Clients.OthersInGroup(roomName).SendAsync("UserTyping", user);
        }


        public async Task CanceledNotifyTyping(string roomName, string user)
        {
            await Clients.OthersInGroup(roomName).SendAsync("UserCanceledTyping", user);
        }

        public async Task JoinRoom(string roomName, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }


    }
}
