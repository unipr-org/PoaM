using Imu.Business;
using Imu.Business.Abstraction;
using Imu.Business.Kafka.MessageHandlers;
using Imu.Business.Kafka;
using Imu.Repository;
using Imu.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Imu.Business.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ImuDbContext>(options => options.UseSqlServer("name=ConnectionStrings:ImuDbContext", b => b.MigrationsAssembly("Unipr.Imu.Api")));

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IBusiness, Business>();

builder.Services.AddAutoMapper(typeof(AssemblyMarker));

builder.Services.AddKafkaConsumerService<KafkaTopicsInput, MessageHandlerFactory>(builder.Configuration);


builder.Services.AddHttpClient<Anagrafiche.ClientHttp.Abstraction.IClientHttp, Anagrafiche.ClientHttp.ClientHttp>("AnagraficheClientHttp", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration.GetSection("AnagraficheClientHttp:BaseAddress").Value);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
