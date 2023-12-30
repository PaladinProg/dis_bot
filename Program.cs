using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using Nito.AsyncEx.Synchronous;

namespace dis_bot
{
    public class Pic
    {
        public string url{ get; set; }
    }

    class Program
    {
        DiscordSocketClient client;
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };

            client = new DiscordSocketClient(config);
            client.MessageReceived += CommandsHandler;
            client.Log += Log;


            /*var _client = new DiscordClient();

            _client.UsingAudio(x => // Opens an AudioConfigBuilder so we can configure our AudioService
            {
                x.Mode = AudioMode.Outgoing; // Tells the AudioService that we will only be sending audio
            });*/


            var token = "MTA2OTM4NDgwMTQwMTk4MzAwNg.GYvi8a.YgwzZeKm1cQdE6BitPm4hJLueAmm8NvPBbIMnY";
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            Console.ReadLine();
        }

        /*private async Task OnVoiceStateUpdated(SocketUser user, SocketVoiceState state1, SocketVoiceState state2)
        {
            if (user.IsBot)
                return;

            if (state1.VoiceChannel == null && state2.VoiceChannel != null)
            {
                ConnectToVoice(state2.VoiceChannel).Start();
            }
        }

        private async Task ConnectToVoice(SocketVoiceChannel voiceChannel)
        {
            if (voiceChannel == null)
                return;

            Console.WriteLine($"Connecting to channel {voiceChannel.Id}");
            var connection = await voiceChannel.ConnectAsync();
            Console.WriteLine($"Connected to channel {voiceChannel.Id}");
        }*/

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private readonly CommandService _commands;


        public async Task<string> Anime(string arg, bool issnfw = false)
        {
            string ass = "sfw";
            if (issnfw) ass = "nsfw";

            WebClient client = new WebClient();
            var json = client.DownloadString("https://api.waifu.pics/"+ ass +"/" + arg);//берём данные по API

            Pic xyi = JsonSerializer.Deserialize<Pic>(json);

            return xyi.url;
        }


        private Task CommandsHandler(SocketMessage msg)
        {

            if (!msg.Author.IsBot)
            {
                if (!msg.Content.StartsWith("!")) 
                    return Task.CompletedTask;

                string[] temp= msg.Content.Split(" ");
                string cmd =temp[0];
                cmd = cmd.Substring(1,cmd.Length-1);
                
                List<string> args = new List<string>();
                for(int i=1; i<temp.Length; i++)
                {
                    args.Add(temp[i]);
                    
                }

                switch (cmd)
                {
                    case "radio":
                        {
                            
                            //ConnectToVoice(a);
                            msg.Channel.SendMessageAsync($"Ща радио настрою, {msg.Author}");
                            break;
                        }
                    case "anime":
                        {
                            try
                            {
                                string param = args[0];
                                string result = Anime(param).WaitAndUnwrapException();
                                msg.Channel.SendMessageAsync(result);
                            }
                            catch (Exception)
                            {
                                string result = Anime("waifu").WaitAndUnwrapException();
                                msg.Channel.SendMessageAsync(result);
                            };
                            break;
                        }
                    case "porn":
                        {
                            try
                            {
                                string param = args[0];
                                string result = Anime(param, true).WaitAndUnwrapException();
                                msg.Channel.SendMessageAsync(result);
                            }
                            catch (Exception)
                            {
                                string result = Anime("waifu", true).WaitAndUnwrapException();
                                msg.Channel.SendMessageAsync(result);
                            };
                            break;
                        }
                     
                }
            }
            return Task.CompletedTask;
            // Don't process the command if it was a system message
            /*var message = msg as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;


            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(client, message);
            Console.WriteLine(msg.Content);
            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.*/
          
            
        }
    }
}
