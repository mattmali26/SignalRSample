using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Pages
{
    public partial class Index : IAsyncDisposable
    {
        private HubConnection hubConnection;
        private List<string> messages = new List<string>();
        private string messageToSend = string.Empty;

        [Inject]
        private ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44394/samplehub")
                .Build();

            hubConnection.On<string>("SendMessage", (message) =>
            {
                Snackbar.Add("New message received", Severity.Success);
                messages.Add(message);
                StateHasChanged();
            });

            await hubConnection.StartAsync();
        }

        private async Task SendMessage()
        {
            await hubConnection.SendAsync("SendMessage", messageToSend);
            messageToSend = string.Empty;
        }

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}