using CoffeeShopWebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<CityRepository>();
builder.Services.AddScoped<CountryRepository>();
builder.Services.AddScoped<StateRepository>();
builder.Services.AddScoped<UserRepository>();         // Add UserRepository
builder.Services.AddScoped<ProductRepository>();      // Add ProductRepository
builder.Services.AddScoped<OrderRepository>();        // Add OrderRepository
builder.Services.AddScoped<CustomerRepository>();     // Add CustomerRepository
builder.Services.AddScoped<BillRepository>();         // Add BillRepository
builder.Services.AddScoped<OrderDetailsRepository>();  // Add OrderDetailsRepository
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
