using ProductCatalogServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<ProductService>();

app.MapGet("/", () => "Use a gRPC client to interact with the ProductCatalogService.");

app.Run();
