using OnlineAuction.WebUI.Models;

namespace OnlineAuction.WebUI.Clients
{
    public class BidClient
    {
        public HttpClient _client { get; }

        public BidClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("http://localhost:8001");
        }

        public async Task<List<BidViewModel>> GelAllBidsByAuctionId(string id)
        {
            var response = await _client.GetAsync("/api/v1/Bid/GetAllBidsByAuctionId?id=" + id);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<BidViewModel>>();
                if (result.Any())
                    return result;                
            }
            return null;
        }

        public async Task<string> SendBid(BidViewModel model)
        {            
            var response = await _client.PostAsJsonAsync("/api/v1/Bid", model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            return null;
        }
    }
}
