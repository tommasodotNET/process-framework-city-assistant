using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace IntelligentCityApp.ProcessOrchestration;

public class ProcessFrameworkHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public async Task HandleEventAsync(string eventData)
    {
        var message = JsonSerializer.Deserialize<Message>(eventData);
        await Clients.All.SendAsync("ReceivePFEvents", message.Text);
    }
}