using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Todo.Models.Entities;
using Todo.Repository.User;

public class BasicAuthFilter : IAsyncAuthorizationFilter
{
    private readonly UserRepository _userRepository;

    public BasicAuthFilter(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var request = context.HttpContext.Request;
        string authHeader = request.Headers["Authorization"];

        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        string encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
        byte[] decodedBytes = Convert.FromBase64String(encodedCredentials);
        string decodedCredentials = System.Text.Encoding.UTF8.GetString(decodedBytes);

        string[] credentials = decodedCredentials.Split(new[] { ':' }, 2);
        string login = credentials[0];
        string password = credentials[1];

        // Проверка аутентификационных данных в базе данных
        UserEntity user = await _userRepository.GetUserByLoginAndPassword(login, password);
        if (user == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        // Создание объекта AuthenticatedUser с информацией о пользователе
        var authenticatedUser = new AuthenticatedUser
        {
            UserId = user.Id,
            Login = user.Login,
            UserRole = user.UserRole
        };
        // Добавление объекта AuthenticatedUser в контекст запроса
        context.HttpContext.Items["AuthenticatedUser"] = authenticatedUser;
    }
}

