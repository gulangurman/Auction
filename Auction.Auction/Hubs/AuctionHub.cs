using Microsoft.AspNetCore.SignalR;

namespace Auction.Auction.Hubs
{
    public class AuctionHub : Hub
    {
        /*
         * Add clients to an auction group where only members can send/receive bids
         */
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        /*
         * Send user's bid to all group members
         *  i.e. Invoke "Bids" method with user and bid parameters
         *       on all clients in this group
         */
        public async Task SendBid(string groupName, string user, string bid)
        {
            await Clients.Group(groupName).SendAsync("Bids", user, bid);
        }
    }
}
