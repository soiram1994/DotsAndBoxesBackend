
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Protocol;
using System;

namespace DotsAndBoxes.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var hubConnection = new HubConnection("http://localhost:51925/");
            IHubProxy hubProxy = hubConnection.CreateHubProxy("gamehub");
            int i;
            hubProxy.On("NewGame", (d) => { i = d; });
            
            
        }
    }
}
