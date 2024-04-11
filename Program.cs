
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
} // ���������� ���������� � ������ (https://learn.microsoft.com/ru-ru/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0)

app.UseHttpsRedirection(); // �� �������������� ���� ��������������� HTTPS (UseHttpsRedirection) �������������� ������� � HTTP �� HTTPS.
app.UseStaticFiles(); // �� �������������� ���� ����������� ������ (UseStaticFiles) ���������� ����������� ����� � ��������� ���������� ��������� ��������.
app.UseCors("AllowOrigin"); // �� �������������� ���� �������� ������ Cookie (UseCookiePolicy) ������������ ������������ ���������� ������ ������ ���������� �� ������ ������ (GDPR) ��.
app.UseRouting(); // �� �������������� ���� ������������� (UseRouting) ��� ������������� ��������.
                  // �� �������������� ���� �������� ����������� (UseAuthentication) �������� ��������� ����������� ������������, ������ ��� ������������ ��� ������ � ���������� ��������.
app.UseAuthorization(); // �� �������������� ���� ����������� (UseAuthorization) ��������� ������������ ������ � ���������� ��������.

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
var port = Environment.GetEnvironmentVariable("PORT") ?? "4000";
app.Logger.LogInformation("Starting the app");
app.Run($"http://localhost:{port}");
