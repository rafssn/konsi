using konsi_api.Models.Interfaces;
using konsi_api.Repositories;
using konsi_api.Services.ElasticServuces;
using konsi_api.Services.HttpServices;
using konsi_api.Services.Messaging;
using konsi_api.Services.Messaging.Consumers;
using konsi_api.Services.Messaging.Publishers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("DefaultConnection");
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<ConsumerCpfService>();

builder.Services.AddSingleton<IBenefitsCache, BenefitsCache>();
builder.Services.AddSingleton<IAuthHttpService, AuthHttpService>();
builder.Services.AddSingleton<IBenefitsHttpService, BenefitsHttpService>();
builder.Services.AddSingleton<IPublishCpfService, PublishCpfService>();
builder.Services.AddSingleton<IRabbitService, RabbitService>();
builder.Services.AddSingleton<IElasticService, ElasticService>();

builder.Services.AddHttpClient("konsi", (serviceProvider, client) =>
{
    client.BaseAddress = new Uri("http://teste-dev-api-dev-140616584.us-east-1.elb.amazonaws.com");
});

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
