using server.Dtos.Product;

namespace server.Service
{
    public class ProductHttpClientService
    {
        private readonly HttpClient _httpClient;

        public ProductHttpClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ProductService");
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"/server/product/{productId}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ProductDto>();
        }

        public async Task<bool> ReserveProductStockAsync(int productId, int quantity)
        {
            var content = JsonContent.Create(new { Quantity = -quantity }); // УМЕНЬШАЕМ stock
            var response = await _httpClient.PutAsync($"/server/product/{productId}/stock", content);
            return response.IsSuccessStatusCode;
        }
    }
}
