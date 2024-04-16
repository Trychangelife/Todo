using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json;
using Todo.Models;
using Todo.Models.Entities;

namespace Todo.Repository.Task
{
    public class TaskRepository //: IRepository<TaskApp> В теории здесь можно сделать Interface для определения методов которые должны быть реализованы (под вопросом необходимость)
    {
        private readonly TodoListContext _context; // Подключаем контекст из БД для возможности обращения к нему

        public TaskRepository(TodoListContext context)
        {
            _context = context;
        }

        public async Task<TaskApp[]> GetAllTask(int userId, string sortBy = null, string sortOrder = null, string searchNameTerm = null)
        {
            sortBy = sortBy?.ToLower(); // Приведение sortBy к нижнему регистру
            sortOrder = sortOrder?.ToLower(); // Приведение sortOrder к нижнему регистру

            IQueryable<TaskApp> query = _context.Tasks.Where(task => task.Author == userId);
            if (!string.IsNullOrEmpty(searchNameTerm))
            {
                // Пример: поиск по названию задачи или описанию
                query = query.Where(task => task.Status.Contains(searchNameTerm) || task.Priority.Contains(searchNameTerm));
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "priority":
                        if (sortOrder == "desc")
                            query = query.OrderByDescending(task => task.Priority);
                        else
                            query = query.OrderBy(task => task.Priority);
                        break;
                    case "status":
                        if (sortOrder == "desc")
                            query = query.OrderByDescending(task => task.Status);
                        else
                            query = query.OrderBy(task => task.Status);
                        break;
                    case "created":
                        if (sortOrder == "desc")
                            query = query.OrderByDescending(task => task.Created);
                        else
                            query = query.OrderBy(task => task.Created);
                        break;
                    default:
                        // Если sortBy имеет недопустимое значение, сортировать по умолчанию по приоритету
                        query = query.OrderBy(task => task.Priority);
                        break;
                }
            }

            return await query.ToArrayAsync();
        }
        public async Task<TaskApp> GetTaskById(int TaskId, int userId = -1)
        {
            if (userId == -1)
            {
                // В случае если нам необходимо получить Task для дальнейшей логике где будет своя проверка на соответствие userID
                var taskWithOutUserId = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == TaskId);
                return taskWithOutUserId;
            }
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == TaskId && t.Author == userId);
            return task;
        }

        public async Task<bool> CreateTask(TaskApp task)

        {
            try
            {
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync(); // Асинхронное сохранение изменений в базе данных вместо Await
                return true; // Успешно добавлено
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении задачи: {ex.Message}");
                return false; // Ошибка при добавлении
            }
        }

        public async Task<bool> DeleteTaskById(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateTaskById(TaskApp task)
        {
            try
            {
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                // Обработка ошибки сохранения изменений в базе данных
                // Можно добавить логирование или другие действия
                return false;
            }
        }


    }
}
