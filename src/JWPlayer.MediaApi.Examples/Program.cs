using JWPlayer.ApiGateway.Services;
using JWPlayer.ApiGateway.Services.Abstractions;
using JWPlayer.MediaApi;
using JWPlayer.MediaApi.Examples;
using JWPlayer.MediaApi.Resources;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AuthTokenFactory>();
builder.Services.AddSingleton<IAuthTokenFactory>(x => x.GetRequiredService<AuthTokenFactory>());
builder.Services.AddSingleton<IExampleAuthTokenFactory>(x => x.GetRequiredService<AuthTokenFactory>());
builder.Services.AddSingleton<IRestClientFactory, RestClientFactory>();

builder.Services.AddOptions<MediaApiOptions>().Bind(configuration.GetSection("MediaApiOptions"));
builder.Services.AddSingleton<IMediaResource, MediaResource>();
builder.Services.AddSingleton<IBclMediaResource, BclMediaResource>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
