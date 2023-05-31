using AutoMapper;
using ElectronicAssistantWebAPI.BLL;
using ElectronicAssistantWebAPI.BLL.Models;
using ElectronicAssistantWebAPI.BLL.Services;
using ElectronicAssistantWebAPI.DAL.EF;
using ElectronicAssistantWebAPI.DAL.Extentions;
using ElectronicAssistantWebAPI.DAL.Models;
using ElectronicAssistantWebAPI.DAL.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var mapperConfig = new MapperConfiguration((v) =>
{
    v.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddUnitOfWork();
builder.Services.AddCustomRepository<RecommendedPrescription, RecommendedPrescriptionRepository>();
builder.Services.AddCustomRepository<PrescriptionProtocol, PrescriptionProtocolRepository>();

builder.Services.AddScoped<IRecommendedPrescriptionService, RecommendedPrescriptionService>();
builder.Services.AddScoped<IPrescriptionProtocolService, PrescriptionProtocolService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHsts();

app.UseAuthorization();

app.MapControllers();

app.Run();
