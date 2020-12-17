using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Sem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(4)
            };

            app.UseWebSockets(webSocketOptions);

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/wss")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
                        {
                            await Echo(webSocket);
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }

            });
        }

        static Dictionary<string, HashSet<WebSocket>> usersWS = new Dictionary<string, HashSet<WebSocket>>();
        public async Task Echo(WebSocket webSocket)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            string forumId = "";
            while (webSocket.State == WebSocketState.Open)
            {
                byte[] receiveBuffer = new byte[1024];
                var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                if (webSocket.State == WebSocketState.CloseReceived)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    usersWS[forumId].Remove(webSocket);
                }
                else
                {
                    var length = receiveBuffer.TakeWhile(b => b != 0).Count();
                    var message = Encoding.UTF8.GetString(receiveBuffer, 0, length);

                    var indexPars = message.IndexOf('&');

                    forumId = message.Substring(0, indexPars);

                    var answer = message.Substring(indexPars + 1, message.Length - indexPars - 1);
                    var result = HttpUtility.UrlEncode(answer);

                    if (!usersWS.ContainsKey(forumId))
                        usersWS[forumId] = new HashSet<WebSocket>();
                    if (!usersWS[forumId].Contains(webSocket))
                        usersWS[forumId].Add(webSocket);

                    foreach (var e in usersWS.FirstOrDefault(x => x.Key == forumId).Value)
                        await SendMessage(e, result);
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