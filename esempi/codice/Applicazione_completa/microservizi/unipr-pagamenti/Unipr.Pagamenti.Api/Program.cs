using Microsoft.EntityFrameworkCore;
using Pagamenti.Business;
using Pagamenti.Business.Abstraction;
using Pagamenti.Business.Kafka;
using Pagamenti.Business.Profiles;
using Pagamenti.Repository;
using Pagamenti.Repository.Abstraction;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<PagamentiDbContext>(options => options.UseSqlServer("name=ConnectionStrings:PagamentiDbContext", b => b.MigrationsAssembly("Unipr.Pagamenti.Api")));

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IBusiness, Business>();

object value = builder.Services.AddAutoMapper(typeof(AssemblyMarker));

builder.Services.AddKafkaProducerService<KafkaTopicsOutput, ProducerService>(builder.Configuration);

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
