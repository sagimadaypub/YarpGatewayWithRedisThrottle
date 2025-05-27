var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("allowReverseProxy", policy =>
    {
        policy.WithOrigins("https://localhost:7214");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("MyClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7214");
});

var app = builder.Build();

app.UseCors("allowReverseProxy");

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
