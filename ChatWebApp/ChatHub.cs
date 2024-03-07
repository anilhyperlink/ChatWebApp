using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChatWebApp
{
    public class ChatHub : Hub
    {
        //one way
        public async Task SendMessage(string user, string message, DateTime dateTime)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, dateTime);
        }

        //multi way
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        public async Task SendGroupMessage(string groupName, string username, string message, DateTime dateTime)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", username, message, dateTime);
        }
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
