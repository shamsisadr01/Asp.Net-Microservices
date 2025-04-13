using Discount.Grpc.Extentions;
using Discount.Grpc.Repositories;
using Discount.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.MigrateDatabase<Program>();
app.Run();
