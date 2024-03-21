using Microsoft.AspNetCore.SignalR;

namespace pmbackend.Hub;

public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
{
    public async Task PingUser(string user)
    {
        Console.WriteLine("here");
        await Clients.All.SendAsync("ReceivePing", user);
    }
}