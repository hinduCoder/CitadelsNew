using Citadels.Api;
using Citadels.Api.Services;
using Citadels.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IGameHostStorage, GameHostStorage>();
builder.Services.AddSingleton<GameEventHandler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

app.UseGrpcWeb();
#pragma warning disable ASP0014
app.UseEndpoints(endpoints => { endpoints.MapGrpcService<CitadelsService>().EnableGrpcWeb(); });
#pragma warning restore ASP0014

app.MapGet("/", () => "Hello World!");

app.Run();