using Microsoft.AspNetCore.Mvc;
using Todo.Models.Entities;
using Todo.Service_Layer.Task;

namespace Todo.Controllers
{
    [ApiController] // включение модельное состояние и обработку валидации по умолчанию для действий контроллера
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly TaskService _taskService; // Внедрение зависимости TaskService

        public TaskController(ILogger<TaskController> logger, TaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }


        // GET: Task/
        [HttpGet("")]
        [ServiceFilter(typeof(BasicAuthFilter))]
        [ServiceFilter(typeof(CheckTaskAuthorizationAttribute))]
        public async Task<IActionResult> All(string sortOrder = "asc", string sortBy = "priority", string searchNameTerm = null)
        {
            var authenticatedUser = HttpContext.Items["AuthenticatedUser"] as AuthenticatedUser;
            TaskApp[] result;
            result = await _taskService.GetAllTask(authenticatedUser.UserId, sortBy, sortOrder, searchNameTerm);
            return Json(result);
        }
        // GET Task/{taskId}
        [HttpGet("{taskId}")]
        [ServiceFilter(typeof(BasicAuthFilter))]
        [ServiceFilter(typeof(CheckTaskAuthorizationAttribute))]
        public async Task<ActionResult<TaskApp>> index(int taskId)
        {
            var authenticatedUser = HttpContext.Items["AuthenticatedUser"] as AuthenticatedUser;

            // Получение задачи
            var result = await _taskService.GetTaskById(taskId);

            // Проверка наличия задачи
            if (result == null)
            {
                // Возвращаем ошибку, если задача не найдена
                return NotFound("Запрошенная задача не найдена");
            }

            // Возвращаем задачу
            return result;
        }


        // POST: Task/Create
        [HttpPost("create")]
        [ServiceFilter(typeof(BasicAuthFilter))]
        public async Task<IActionResult> CreateTask([FromBody] TaskApp task)
        {
            var authenticatedUser = HttpContext.Items["AuthenticatedUser"] as AuthenticatedUser;

            if (ModelState.IsValid)
            {
                bool result = await _taskService.CreateTask(task, authenticatedUser.UserId);
                if (result)
                {
                    // Получаем созданную задачу
                    var createdTask = await _taskService.GetTaskById(task.Id, authenticatedUser.UserId);
                    // Возвращаем созданную задачу в формате JSON
                    return Json(createdTask);
                }
                else
                {
                    return BadRequest("Не удалось добавить задачу");
                }
            }
            return BadRequest("Отправленные Вами данные не соответствуют ожидаемой модели");
        }

        [HttpDelete("{taskId}")]
        [ServiceFilter(typeof(BasicAuthFilter))]
        [ServiceFilter(typeof(CheckTaskAuthorizationAttribute))]
        public async Task<ActionResult<TaskApp>> DeleteTaskById(int taskId)
        {
            bool result = await _taskService.DeleteTaskById(taskId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("{taskId}")]
        [ServiceFilter(typeof(BasicAuthFilter))]
        [ServiceFilter(typeof(CheckTaskAuthorizationAttribute))]
        public async Task<ActionResult<TaskApp>>  UpdateTaskById(int taskId, [FromBody] TaskUpdateModel taskUpdateModel)
        {
            var authenticatedUser = HttpContext.Items["AuthenticatedUser"] as AuthenticatedUser;
            var result = await _taskService.UpdateTaskById(taskId, taskUpdateModel, authenticatedUser.UserId);
            if (result.Success)
            {
                return Ok();
            }
            else
            {
                return NotFound(result.ErrorMessage);
            }
        }
    }
}
