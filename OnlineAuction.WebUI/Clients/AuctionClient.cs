using Newtonsoft.Json;
using OnlineAuction.WebUI.Models;
using System.Net.Http.Headers;

namespace OnlineAuction.WebUI.Clients
{
    public class AuctionClient
    {
        public HttpClient _client { get; }

        public AuctionClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("http://localhost:8001");
        }

        public async Task<List<AuctionViewModel>> GetAuctions()
        {
            var response = await _client.GetAsync("/api/v1/Auction");
            if (response.IsSuccessStatusCode)
            {
                var auctions = await response.Content.ReadFromJsonAsync<List<AuctionViewModel>>();
                if (auctions.Any())
                {
                    return auctions;
                }
            }
            return null;
        }

        public async Task<AuctionViewModel> CreateAuction(AuctionViewModel model)
        {
            //var response = await _client.PostAsJsonAsync("/api/v1/Auction", model);
            var dataAsString = JsonConvert.SerializeObject(model);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync("/api/v1/Auction", content);

            if (response.IsSuccessStatusCode)
            {
                var auction = await response.Content.ReadFromJsonAsync<AuctionViewModel>();
                if (auction != null)
                {
                    return auction;
                }
            }
            return null;
        }

        public async Task<AuctionViewModel> GetAuctionById(string id)
        {
            var response = await _client.GetAsync("/api/v1/Auction/" + id);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuctionViewModel>();
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public async Task<string> CompleteBid(string id)
        {
            var response = await _client.PostAsJsonAsync("/api/v1/Auction/CompleteAuction", id);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            return null;
        }
    }
}
