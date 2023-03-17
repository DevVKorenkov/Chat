using Chat.Models;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Hubs;

public class ChatHub : Hub
{
    private const string master = "Master";
    private readonly IDictionary<string, UserConnection> _connections;

    public ChatHub(
        IDictionary<string, UserConnection> connections)
    {
        _connections = connections;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            Clients
                .Group(userConnection.Room)
                .SendAsync("ReceiveMessage", master, $"{userConnection.Name} has left");
            _connections.Remove(Context.ConnectionId);
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task JoinClanRoom(UserConnection userConnection)
    {
        var connectionId = Context.ConnectionId;

        if(_connections.Any(u => u.Value.Name == userConnection.Name))
        {
            var user = _connections.FirstOrDefault(u => u.Value.Name == userConnection.Name).Value;

            await Clients
                .Caller
                .SendAsync("UserItRoom", master, $@"{user.Name} is in {user.Room} chat already");
            return;
        }

        await Groups.AddToGroupAsync(connectionId, userConnection.Room);

        _connections.Add(connectionId, userConnection);

        await Clients
            .Group(userConnection.Room)
            .SendAsync("ReceiveMessage", master, $@"{userConnection.Name} has joined");
    }

    public async Task SendMessage(string message)
    {
        if(_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            await Clients
                .Group(userConnection.Room)
                .SendAsync("ReceiveMessage", userConnection.Name, message);
        }
    }

    public async Task LeaveRoom()
    {
        if(_connections.TryGetValue(Context.ConnectionId, out UserConnection? userConnection))
        {
            await Clients.Group(userConnection.Room)
                .SendAsync("LeaveRoom", master, $@"{userConnection.Name} has left ""{userConnection.Room}""");

            await Groups
                .RemoveFromGroupAsync(Context.ConnectionId, userConnection.Room);

            _connections.Remove(Context.ConnectionId);
        }
    }
}
