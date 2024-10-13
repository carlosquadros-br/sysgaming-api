using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace SysgamingApi.Src.Application.Utils
{
    public interface INotifcationService
    {
        Task CreateConnection(WebSocket client);

        Task SendNotificationAsync(string message);
    }

    public class NotificationServiceImpl : INotifcationService
    {
        private Dictionary<string, WebSocket> Clients = new();
        public async Task CreateConnection(WebSocket client)
        {
            var id = Guid.NewGuid().ToString();
            Clients[id] = client;

            System.Console.WriteLine("Client connected");
            while (client.State == WebSocketState.Open)
            {
                var buffer = new byte[1024 * 4];
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Clients.Remove(id);
                    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
                }
            }
        }

        public async Task SendNotificationAsync(string message)
        {
            System.Console.WriteLine("Sending notification");
            System.Console.WriteLine(Clients.Count);
            System.Console.WriteLine(message);
            var buffer = System.Text.Encoding.UTF8.GetBytes(message);
            var tasks = Clients.Values
                .Where(client => client.State == WebSocketState.Open)
                .Select(client => client.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None));

            await Task.WhenAll(tasks);
        }

    }
}
