using Microsoft.EntityFrameworkCore;
using UserAuthAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// API Controller desteği
builder.Services.AddControllers();

// Veritabanı bağlantısı
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger + endpoint explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 💡 CORS ayarları eklendi
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger sadece development ortamında
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 💡 CORS middleware aktif edildi
app.UseCors("AllowAll");

app.UseAuthorization();

// API Controller'ları eşle
app.MapControllers();

app.Run();
