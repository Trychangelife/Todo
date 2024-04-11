using System.ComponentModel.DataAnnotations;

namespace Todo.Models
{
        public class AccountViewModel1
        {
            public LoginViewModel LoginViewModel { get; set; } = null!;

            public RegisterViewModel RegisterViewModel { get; set; } = null!;

    }

        public class LoginViewModel
        {
            [Required(ErrorMessage = "Данное поле обязательно")]

            public string Login { get; set; } = null!;

        [Required(ErrorMessage = "Данное поле обязательно")]
            public string Password { get; set; } = null!;
    }

        public class RegisterViewModel
        {
        [Required(ErrorMessage = "Данное поле обязательно")]

        public string Login { get; set; } = null!;

            [Required(ErrorMessage = "Данное поле обязательно")]
            public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Данное поле обязательно")]
            [Compare("Password", ErrorMessage = "Пароли не совпадают")] // требуем равенства от поля Password
            public string RepeatPassword { get; set; } = null!;
    }
}
