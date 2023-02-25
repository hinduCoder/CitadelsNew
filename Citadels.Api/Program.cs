using Citadels.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

app.UseGrpcWeb();
#pragma warning disable ASP0014
app.UseEndpoints(endpoints => { endpoints.MapGrpcService<CalculatorService>().EnableGrpcWeb(); });
#pragma warning restore ASP0014

app.MapGet("/", () => "Hello World!");

app.Run();