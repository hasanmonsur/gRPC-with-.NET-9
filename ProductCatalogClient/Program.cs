// See https://aka.ms/new-console-template for more information
using Grpc.Core;
using Grpc.Net.Client;
using ProductCatalog;

Console.WriteLine("Hello, World!");

// Create a channel to the server
using var channel = GrpcChannel.ForAddress("https://localhost:8810");
var client = new ProductCatalogService.ProductCatalogServiceClient(channel);

// Unary call: Get a single product
Console.WriteLine("Fetching product with ID 1...");
try
{
    var response = await client.GetProductAsync(new ProductRequest { Id = 1 });
    Console.WriteLine($"Product: {response.Name}, Price: ${response.Price}, Description: {response.Description}");
}
catch (RpcException ex)
{
    Console.WriteLine($"Error: {ex.Status.Detail}");
}

// Server-streaming call: Get all products
Console.WriteLine("\nStreaming all products...");
using var streamingCall = client.GetAllProducts(new EmptyRequest());
await foreach (var product in streamingCall.ResponseStream.ReadAllAsync())
{
    Console.WriteLine($"Product: {product.Name}, Price: ${product.Price}");
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();