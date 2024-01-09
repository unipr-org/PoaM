using Microsoft.EntityFrameworkCore;
using UniprExample.Business;
using UniprExample.Repository;
using UniprExample.Business.Profiles;
using UniprExample.Business.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UniprExampleDbContext>(options => options.UseSqlServer("name=ConnectionStrings:UniprExampleDbContext"));
//string? connectionStrings = builder.Configuration["ConnectionStrings:UniprExampleDbContext"];
//string? connectionStrings1 = builder.Configuration.GetConnectionString("UniprExampleDbContext");
//string? connectionStrings2 = builder.Configuration.GetSection("ConnectionStrings")["UniprExampleDbContext"];

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IBusiness, Business>();

builder.Services.AddAutoMapper(typeof(AssemblyMarker));

builder.Services.AddKafkaProducerService<KafkaTopicsOutput, ProducerService>(builder.Configuration);

builder.Services.AddControllers();
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
