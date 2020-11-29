using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sem.Pages
{
	public class Forum : PageModel
	{
		private async Task OnPostAsync()
        {
            if (Request.Path == "/ws")
            {
                if (Request.HttpContext.WebSockets.IsWebSocketRequest)
                {
                    using (WebSocket webSocket = await Request.HttpContext.WebSockets.AcceptWebSocketAsync())
                    {
                        await Echo(webSocket);
                    }
                }
                else
                {
                    Request.HttpContext.Response.StatusCode = 400;
                }
            }
        }

        public async Task Echo(WebSocket webSocket)
        {
            string message;
            DataBase.socketsUsers.Add(webSocket);
            CancellationTokenSource cts = new CancellationTokenSource();
            while (webSocket.State == WebSocketState.Open)
            {
                byte[] receiveBuffer = new byte[1024];
                var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                if (receiveResult.MessageType == WebSocketMessageType.Close)
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                else
                {
                    var length = receiveBuffer.TakeWhile(b => b != 0).Count();
                    string result = Encoding.UTF8.GetString(receiveBuffer, 0, length);
                    var mesReg = result.Split('#');
                    if (mesReg.Length > 1)
                    {
                        message = "Connected login: " + mesReg[0];
                    }
                    else if (result[0] == '$')
                    {
                        message = "exit user: " + result.Substring(1, result.Length - 1);
                    }
                    else
                    {
                        message = result;
                    }
                    foreach (var e in DataBase.socketsUsers)
                        await SendMessage(e, message);
                }
            }
            cts.Cancel();
        }

        private static async Task SendMessage(WebSocket webSocket, string message)
        {
            try
            {
                byte[] output = Encoding.UTF8.GetBytes(message);
                await webSocket.SendAsync(new ArraySegment<byte>(output, 0, message.Length),
                                            WebSocketMessageType.Text,
                                            true,
                                            CancellationToken.None);
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }
    }
}