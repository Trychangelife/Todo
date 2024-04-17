using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Models.Entities;
using Todo.Repository.Task;

namespace Todo.Service_Layer.Task
{
    public class TaskService
    {
        private readonly TaskRepository _taskRepository;

        public class UpdateResult
        {
            public bool Success { get; set; }
            public string ErrorMessage { get; set; } = null!;
        }

        public TaskService(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskApp> GetTaskById(int TaskId, int userId = -1)
        {
            return await _taskRepository.GetTaskById(TaskId, userId);
        }
        public async Task<bool> CreateTask(TaskApp task, int UserId)
        {
            if (task != null)
            {
                DateTime now = DateTime.Now;
                string formattedDateTime = now.ToString("yyyy-MM-dd HH:mm:ss");


                // Обработка значений перед присвоением объекту
                task.Title = task.Title?.Trim(); // Проверка на null и применение Trim()
                task.Description = task.Description?.Trim();
                task.Created = DateTime.Parse(formattedDateTime);
                task.Author = UserId;
                task.Status = "Open";

                bool result = await _taskRepository.CreateTask(task);
                return result;
            }
            else
            {
                return false;
            }
            
        }

        public async Task<TaskApp[]> GetAllTask(int userId, string sortBy = null, string sortOrder = null, string searchNameTerm = null)
        {
            TaskApp[] result = await _taskRepository.GetAllTask(userId, sortBy, sortOrder, searchNameTerm);
            return result;
        }
        public async Task<bool> DeleteTaskById(int id)
        {
            return await _taskRepository.DeleteTaskById(id);
        }

        public async Task<UpdateResult> UpdateTaskById(int TaskId, TaskUpdateModel taskUpdateModel, int UserId)
        {
            var result = new UpdateResult();

            var task = await _taskRepository.GetTaskById(TaskId, UserId);
            if (task == null)
            {
                result.Success = false;
                result.ErrorMessage = "Задача не найдена";
                return result;
            }

            DateTime now = DateTime.Now;
            string formattedDateTime = now.ToString("yyyy-MM-dd HH:mm:ss");

            task.Title = taskUpdateModel.Title?.Trim();
            task.Priority = taskUpdateModel.Priority.ToString();
            task.Status = taskUpdateModel.Status.ToString();
            task.Description = taskUpdateModel.Description?.Trim();
            task.LastUpdated = DateTime.Parse(formattedDateTime);

            try
            {
                await _taskRepository.UpdateTaskById(task);
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return result;
            }
        }

    }
}