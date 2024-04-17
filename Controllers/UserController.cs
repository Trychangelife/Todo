using Microsoft.AspNetCore.Mvc;
using Todo.Models.Entities;
using Todo.Service_Layer.User;

namespace Todo.Controllers
{

    [ApiController] // включение модельное состояние и обработку валидации по умолчанию для действий контроллера
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        // GET: User/{id}
        [HttpGet("{id}")]
        [ServiceFilter(typeof(BasicAuthFilter))]
        [ServiceFilter(typeof(SuperAdminFilter))]
        public async Task<ActionResult<UserEntity>> GetUserById(int id)
        {
            _logger.LogInformation("About page visited at {DT}",
            DateTime.UtcNow.ToLongTimeString());

            var result = await _userService.GetUserById(id);
            if (result == null)
            {
                return NotFound("Запрошенный User не найден");
            }
            else
            {
                return Json(result);
            }

        }

        // POST: User/Create
        [HttpPost("create")]
        public async Task<IActionResult> СreateUser([FromBody] UserEntity user)
        {
            if (ModelState.IsValid)
            {
                bool result = await _userService.CreateUser(user);
                if (result)
                {
                    // Получаем созданного юзера
                    var createdUser = await _userService.GetUserById(user.Id);
                    // Возвращаем созданного юзера в формате JSON
                    return Json(createdUser);

                }
                else
                {
                    return BadRequest("Не удалось создать пользователя");
                }

            }
            return BadRequest("Отправленные Вами данные не соответствуют ожидаемой модели");
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(BasicAuthFilter))]
        [ServiceFilter(typeof(SuperAdminFilter))]
        public async Task<ActionResult<UserEntity>> UpdateUserById(int id, [FromBody] UserEntity userUpdate)
        {
            Console.WriteLine(userUpdate);
            var result = await _userService.UpdateUserById(id, userUpdate);
            if (result.Success)
            {
                return Ok();
            }
            else
            {
                return NotFound(result.ErrorMessage);
            }
        }
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(BasicAuthFilter))]
        [ServiceFilter(typeof(SuperAdminFilter))]
        public async Task<ActionResult<UserEntity>> DeleteUserById(int id)
        {
            var result = await _userService.DeleteUserById(id);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}
