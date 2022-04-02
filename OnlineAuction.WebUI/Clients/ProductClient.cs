using OnlineAuction.WebUI.Models;

namespace OnlineAuction.WebUI.Clients
{
    public class ProductClient
    {
        private HttpClient _client { get; }

        public ProductClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("http://auctiondb:27017");
        }
        public async Task<List<ProductViewModel>> GetProducts()
        {
            var response = await _client.GetAsync("/api/v1/Product");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var products = await response.Content.ReadFromJsonAsync<List<ProductViewModel>>();
                    if (products.Any())
                    {
                        return products;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }               
            }
            return null;
        }
    }
}
