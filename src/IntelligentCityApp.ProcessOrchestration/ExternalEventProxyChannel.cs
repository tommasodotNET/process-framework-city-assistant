using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.SemanticKernel;

namespace IntelligentCityApp.ProcessOrchestration;

public class ExternalEventProxyChannel : IExternalKernelProcessMessageChannel
{
    private HubConnection? _hubConnection;

    public Task EmitExternalEventAsync(string externalTopicEvent, KernelProcessProxyMessage eventData)
    {
        if (this._hubConnection == null)
        {
            throw new InvalidOperationException("Hub connection is not initialized.");
        }

        return this._hubConnection.InvokeAsync("HandleEventAsync", eventData);
    }

    public async ValueTask Initialize()
    {
        this._hubConnection = new HubConnectionBuilder()
            .WithUrl(new Uri("https://localhost:7112/pfevents"))
            .Build();

        await this._hubConnection.StartAsync().ConfigureAwait(false);
    }

    public async ValueTask Uninitialize()
    {
        if (this._hubConnection == null)
        {
            throw new InvalidOperationException("Hub connection is not initialized.");
        }

        await this._hubConnection.StopAsync().ConfigureAwait(false);
    }
}