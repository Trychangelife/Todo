using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Todo.Models.Entities;
using Todo.Repository.Task;

public class CheckTaskAuthorizationAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var authenticatedUser = context.HttpContext.Items["AuthenticatedUser"] as AuthenticatedUser;
        if (authenticatedUser == null)
        {
            context.Result = new StatusCodeResult(401); // Возвращаем 401 (Unauthorized), если пользователь не аутентифицирован
            return;
        }

        if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
        {
            //Task<ActionResult<TaskApp>> - Если метод контроллера возвращает данный тип, то он попадает под это условие, как следствие идет проверка на право управлять Task.
            if (controllerActionDescriptor.MethodInfo.ReturnType == typeof(Task<ActionResult<TaskApp>>))
            {
                // Это запрос на одну задачу
                if (!context.ActionArguments.TryGetValue("taskId", out var taskIdObj) || !(taskIdObj is int taskId))
                {   
                    context.Result = new BadRequestObjectResult("TaskId must be provided as a route parameter.");
                    return;
                }
                var taskRepository = (TaskRepository)context.HttpContext.RequestServices.GetService(typeof(TaskRepository));
                var task = await taskRepository.GetTaskById(taskId);

                if (task == null)
                {
                    context.Result = new NotFoundResult(); // Возвращаем 404 (Not Found), если задача не найдена
                    return;
                }
                else if (task.Author != authenticatedUser.UserId) {
                    context.Result = new StatusCodeResult(403); // Возвращаем 403 (Forbidden), если задача не принадлежит пользователю
                    return;
                }
            }
            else if (controllerActionDescriptor.MethodInfo.ReturnType == typeof(TaskApp[]))
            {
                // Это запрос на все задачи пользователя
                var taskRepository = (TaskRepository)context.HttpContext.RequestServices.GetService(typeof(TaskRepository));
                var tasks = await taskRepository.GetAllTask(authenticatedUser.UserId);

                if (tasks == null || tasks.Length == 0)
                {
                    context.Result = new NotFoundResult(); // Возвращаем 404 (Not Found), если задачи не найдены
                    return;
                }
            }
        }

        await next(); // Продолжаем выполнение цепочки фильтров
    }
}
