using Grpc.Core;
using ProductCatalog;

namespace ProductCatalogServer.Services
{
    public class ProductService : ProductCatalogService.ProductCatalogServiceBase
    {
        private readonly ILogger<ProductService> _logger;
        private readonly List<ProductResponse> _products;

        public ProductService(ILogger<ProductService> logger)
        {
            _logger = logger;
            // Sample product data
            _products = new List<ProductResponse>
        {
            new() { Id = 1, Name = "Laptop", Price = 999.99, Description = "High-performance laptop" },
            new() { Id = 2, Name = "Phone", Price = 499.99, Description = "Latest smartphone" },
            new() { Id = 3, Name = "Tablet", Price = 299.99, Description = "Portable tablet" }
        };
        }

        // Unary call implementation
        public override Task<ProductResponse> GetProduct(ProductRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Fetching product with ID: {Id}", request.Id);
            var product = _products.FirstOrDefault(p => p.Id == request.Id)
                ?? throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
            return Task.FromResult(product);
        }

        // Server-streaming call implementation
        public override async Task GetAllProducts(EmptyRequest request, IServerStreamWriter<ProductResponse> responseStream, ServerCallContext context)
        {
            _logger.LogInformation("Streaming all products");
            foreach (var product in _products)
            {
                await responseStream.WriteAsync(product);
                await Task.Delay(500); // Simulate streaming delay
            }
        }
    }
}
