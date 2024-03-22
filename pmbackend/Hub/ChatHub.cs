using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using pmbackend.Models;

namespace pmbackend.Hub;

[Authorize]
public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly UserManager<PmUser> _userManager;
    private readonly IChatRepository _chatRepository;

    public ChatHub(UserManager<PmUser> manager, IChatRepository chatRepository)
    {
        _userManager = manager;
        _chatRepository = chatRepository;
    }

    public override Task OnConnectedAsync()
    {
        var user = _userManager.FindByNameAsync(Context.User!.FindFirst(ClaimTypes.Name)?.Value)
            .GetAwaiter().GetResult();

        if (user is null) return base.OnConnectedAsync();

        user.IsOnline = true;
        _userManager.UpdateAsync(user).GetAwaiter().GetResult();

        Clients.Others.SendAsync("ReceivePing", "*", user.UserName);

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var user = _userManager.FindByNameAsync(Context.User!.FindFirst(ClaimTypes.Name)?.Value)
            .GetAwaiter().GetResult();

        if (user is null) return base.OnDisconnectedAsync(exception);

        user.IsOnline = false;
        _userManager.UpdateAsync(user).GetAwaiter().GetResult();

        Clients.Others.SendAsync("ReceivePing", "*", user.UserName);

        return base.OnDisconnectedAsync(exception);
    }

    public async Task PingUser(string user) =>
        await Clients.Others.SendAsync("ReceivePing", user);
}