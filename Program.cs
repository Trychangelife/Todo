
using Microsoft.EntityFrameworkCore;
using Todo.Models;
using Todo.Repository.Task;
using Todo.Repository.User;
using Todo.Service_Layer.Task;
using Todo.Service_Layer.User;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<BasicAuthFilter>();
builder.Services.AddScoped<CheckTaskAuthorizationAttribute>();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TodoListContext>(opt => opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
} // Обработчик исключений и ошибок (https://learn.microsoft.com/ru-ru/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0)

app.UseHttpsRedirection(); // ПО промежуточного слоя перенаправления HTTPS (UseHttpsRedirection) перенаправляет запросы с HTTP на HTTPS.
app.UseStaticFiles(); // ПО промежуточного слоя статических файлов (UseStaticFiles) возвращает статические файлы и сокращает дальнейшую обработку запросов.
app.UseCors("AllowOrigin"); // ПО промежуточного слоя политики файлов Cookie (UseCookiePolicy) обеспечивает соответствие приложения нормам Общего регламента по защите данных (GDPR) ЕС.
app.UseRouting(); // ПО промежуточного слоя маршрутизации (UseRouting) для маршрутизации запросов.
                  // ПО промежуточного слоя проверки подлинности (UseAuthentication) пытается проверить подлинность пользователя, прежде чем предоставить ему доступ к защищенным ресурсам.
app.UseAuthorization(); // ПО промежуточного слоя авторизации (UseAuthorization) разрешает пользователю доступ к защищенным ресурсам.

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
var port = Environment.GetEnvironmentVariable("PORT") ?? "4000";
app.Logger.LogInformation("Starting the app");
app.Run($"http://localhost:{port}");
