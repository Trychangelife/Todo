using System.Text.RegularExpressions;
using Todo.Models.Entities;
using Todo.Repository.Task;
using Todo.Repository.User;

namespace Todo.Service_Layer.User
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public class UpdateResult
        {
            public bool Success { get; set; }
            public string ErrorMessage { get; set; } = null!;
        }

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private const string EmailRegexPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        private const string LoginRegexPattern = @"^[a-zA-Z0-9_-]*$";

        public bool ValidateUser(UserEntity user)
        {
            // Проверка email
            if (!Regex.IsMatch(user.Email, EmailRegexPattern))
            {
                return false;
            }

            // Проверка логина
            if (!Regex.IsMatch(user.Login, LoginRegexPattern))
            {
                return false;
            }

            return true;
        }

        public async Task<UserEntity> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }
        public async Task<bool> CreateUser(UserEntity user)
        {
            if (!ValidateUser(user))
            {
                return false;
            }
            if (user != null)
            {
                DateTime now = DateTime.Now;
                string formattedDateTime = now.ToString("yyyy-MM-dd HH:mm:ss");


                //Обработка значений перед присвоением объекту
                user.Password = user.Password?.Trim(); // Проверка на null и применение Trim()
                user.Login = user.Login?.Trim();
                user.CreatedAt = DateTime.Parse(formattedDateTime);
                user.Email = user.Email?.Trim();

                bool result = await _userRepository.CreateUser(user);
                return result;
            }
            else
            {
                return false;
            }

        }


        // Запрос не уходит в CATCH блок, нужно проверить чтобы срабатывала ошибка т.к постоянно возвращается 200 код
        public async Task<UpdateResult> UpdateUserById(int id, UserEntity userUpdate)
        {
            var result = new UpdateResult();

            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                result.Success = false;
                result.ErrorMessage = "Пользователь не найден";
                return result;
            }

            user.Login = userUpdate.Login?.Trim();
            user.Password = userUpdate.Password.Trim();
            user.Email = userUpdate.Email.Trim();

            try
            {
                await _userRepository.UpdateUserById(user);
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