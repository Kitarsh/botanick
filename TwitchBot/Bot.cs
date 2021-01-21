﻿using System;
using System.Timers;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchBot.Services;
using TwitchBot.Modules;

namespace TwitchBot
{
    public class Bot
    {
        public TwitchClient Client { get; set; }
        
        public Bot()
        {
            ConnectionCredentials credentials = new ConnectionCredentials("bot_a_nick", Program.Configuration["tokens:oauth"]);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            Client = new TwitchClient(customClient);
            Client.Initialize(credentials, "Kitarsh");
            
            Client.OnLog += Client_OnLog;
            Client.OnJoinedChannel += Client_OnJoinedChannel;
            Client.OnMessageReceived += Client_OnMessageReceived;
            Client.OnWhisperReceived += Client_OnWhisperReceived;
            Client.OnNewSubscriber += Client_OnNewSubscriber;
            Client.OnConnected += Client_OnConnected;
            Client.OnMessageReceived += LogInDiscord.Log;

            Client.Connect();

            // Timer to write in chat every X time.
            var timer = new Timer(new TimeSpan(0, 30, 0).TotalMilliseconds);
            // Hook up the Elapsed event for the timer.
            //timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"Twitch Bot {e.DateTime}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine("Hey guys! I am a bot connected via TwitchLib!");
            Client.SendMessage(e.Channel, "Hey guys! I am a bot connected via TwitchLib!");
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.Contains("badword"))
                Client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(30), "Bad word! 30 minute timeout!");
            if (e.ChatMessage.Message.Contains("toto"))
            {
                Client.SendMessage(e.ChatMessage.Channel, "Votre langage est très évolué.");
            } else if (e.ChatMessage.Message.Contains("!discord"))
            {
                Pub.PubDiscord(this.Client);
            }
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            if (e.WhisperMessage.Username == "my_friend")
                Client.SendWhisper(e.WhisperMessage.Username, "Hey! Whispers are so cool!!");
        }

        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
                Client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
            else
                Client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
        }
        
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
            Pub.PubDiscord(this.Client);
        }
    }
}
