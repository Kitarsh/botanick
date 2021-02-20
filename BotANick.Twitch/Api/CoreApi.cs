using System.Collections.Generic;
using System.Threading.Tasks;

using TwitchLib.Api;
using TwitchLib.Api.V5.Models.Subscriptions;

namespace BotANick.Twitch.Api
{
    public static class CoreApi
    {
        /// <summary>
        /// La Twitch API.
        /// </summary>
        public static TwitchAPI ApiHandler { get; set; }

        /// <summary>
        /// Initialise la Twitch API.
        /// </summary>
        public static void InitApi()
        {
            ApiHandler = new TwitchAPI();
            ApiHandler.Settings.ClientId = Program.Configuration["tokens:Api:ClientId"];
            ApiHandler.Settings.AccessToken = Program.Configuration["tokens:Api:AccessToken"];
        }

        #region Example code

        //private async Task ExampleCallsAsync()
        //{
        //    //Checks subscription for a specific user and the channel specified.
        //    Subscription subscription = await api.V5.Channels.CheckChannelSubscriptionByUserAsync("channel_id", "user_id");

        //    //Gets a list of all the subscritions of the specified channel.
        //    List<Subscription> allSubscriptions = await api.V5.Channels.GetAllSubscribersAsync("channel_id");

        //    //Get channels a specified user follows.
        //    GetUsersFollowsResponse userFollows = await api.Helix.Users.GetUsersFollowsAsync("user_id");

        //    //Get Specified Channel Follows
        //    var channelFollowers = await api.V5.Channels.GetChannelFollowersAsync("channel_id");

        //    //Update Channel Title/Game
        //    await api.V5.Channels.UpdateChannelAsync("channel_id", "New stream title", "Stronghold Crusader");
        //}

        #endregion Example code
    }
}
