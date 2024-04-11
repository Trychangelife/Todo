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
            [Required(ErrorMessage = "������ ���� �����������")]

            public string Login { get; set; } = null!;

        [Required(ErrorMessage = "������ ���� �����������")]
            public string Password { get; set; } = null!;
    }

        public class RegisterViewModel
        {
        [Required(ErrorMessage = "������ ���� �����������")]

        public string Login { get; set; } = null!;

            [Required(ErrorMessage = "������ ���� �����������")]
            public string Password { get; set; } = null!;

        [Required(ErrorMessage = "������ ���� �����������")]
            [Compare("Password", ErrorMessage = "������ �� ���������")] // ������� ��������� �� ���� Password
            public string RepeatPassword { get; set; } = null!;
    }
}
