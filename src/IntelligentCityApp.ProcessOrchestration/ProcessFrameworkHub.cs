using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.SemanticKernel;

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

    public async Task HandleEventAsync(KernelProcessProxyMessage eventData)
    {
        await Clients.All.SendAsync("ReceivePFEvents", eventData.EventData.Content);
    }
}