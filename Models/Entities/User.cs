using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Todo.Models.Entities
{
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("UserId")] // Указываем новое имя столбца в базе данных
        public int Id { get; set; } // Поле остается с именем Id

        [Required]
        [StringLength(50, ErrorMessage = "Login length should be between 3 and 50 characters.", MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Password length should be between 6 and 100 characters.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
    }
}
