using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Todo.Models.Validation;
using Todo.Models.Entities;

namespace Todo.Models.Entities
{
    public class TaskApp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Автоинкрементация для уникальности в БД
        public int Id { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "Title lenght should be <= 25", MinimumLength = 3)]
        [Column(TypeName = "varchar(255)")]
        public string Title { get; set; }

        [EnumValueValidation(typeof(Status), ErrorMessage = "Invalid status value.")]
        [Required]
        //[JsonConverter(typeof(JsonStringEnumConverter))] // Сериализация в формат JSON
        public  string Status { get; set; }
        [ForeignKey("User")]
        [JsonIgnore] // Игнорирование при сериализации, скрываем из выдачи пользователю поле.
        [Required]
        public int Author { get; set; }

        [EnumValueValidation(typeof(Priority), ErrorMessage = "Invalid priority value.")]
        [Required]
        //[JsonConverter(typeof(JsonStringEnumConverter))] // Сериализация в формат JSON
        public string Priority { get; set; }

        [Column(TypeName = "varchar(255)")]
        [StringLength(250, ErrorMessage = "Description lenght should be <= 250", MinimumLength = 3)]
        [NotEmptyString(ErrorMessage = "The {0} field must not be empty or contain only whitespace.")]
        public string? Description { get; set; }

        public DateTime? Created { get; set; }
    }
}
