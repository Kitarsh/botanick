﻿using System;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using BotANick.Twitch.Services;
using BotANick.Twitch.Modules;
using BotANick.Twitch.Api;

namespace BotANick.Twitch
{
    public class Bot
    {
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
            Client.OnMessageReceived += Services.Discord.Log;

            Client.Connect();

            var timeSpanPub = new TimeSpan(0, 30, 0);
            _ = new Clock(Pub.PubDiscord, timeSpanPub, "Pub Discord");

            var timeSpanStreamStarted = new TimeSpan(0, 5, 0);
            _ = new Clock(StreamInfo.CheckStreamStarted, timeSpanStreamStarted, "Stream Start");
        }

        public static TwitchClient Client { get; set; }

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
            CommandService.DoAction(e, Client);
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
    }
}
