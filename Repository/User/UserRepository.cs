using Microsoft.EntityFrameworkCore;
using System.Linq;
using Todo.Models;
using Todo.Models.Entities;

namespace Todo.Repository.User
{
    public class UserRepository
    {
        private readonly TodoListContext _context; // Подключаем контекст из БД для возможности обращения к нему

        public UserRepository(TodoListContext context)
        {
            _context = context;
        }
        public async Task<UserEntity> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(t => t.Id == id);
            return user;
        }

        public async Task<bool> CreateUser(UserEntity user)

        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync(); // Асинхронное сохранение изменений в базе данных
                return true; // Успешно добавлено
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении : {ex.Message}");
                return false; // Ошибка при добавлении
            }
        }
        public async Task<UserEntity> GetUserByLoginAndPassword(string login, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
            return user;
        }

        public async Task<bool> DeleteUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(t => t.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateUserById(UserEntity user)
        {
            using (var transaction = _context.Database.BeginTransaction())
             try
            {
                _context.Users.Update(user);
                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync(); // подтверждение транзакции
                return true;
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                // Обработка ошибки сохранения изменений в базе данных
                // Можно добавить логирование или другие действия
                return false;
            }
        }


    }
}
