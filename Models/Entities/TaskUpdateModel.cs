using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Todo.Models.Validation;

namespace Todo.Models.Entities
{
    public class TaskUpdateModel
    {
        [Required]
        [StringLength(25, ErrorMessage = "Title lenght should be <= 25", MinimumLength = 3)]
        public string Title { get; set; }

        [EnumValueValidation(typeof(Priority), ErrorMessage = "Invalid priority value.")]
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Priority Priority { get; set; }

        [EnumValueValidation(typeof(Status), ErrorMessage = "Invalid status value.")]
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }

    // Перечисления для приоритета и статуса
    public enum Priority
    {
        [EnumMember(Value = "Low")]
        Low,

        [EnumMember(Value = "Medium")]
        Medium,

        [EnumMember(Value = "High")]
        High
    }

    public enum Status
    {
        [EnumMember(Value = "Open")]
        Open,

        [EnumMember(Value = "InProcess")]
        InProcess,

        [EnumMember(Value = "Done")]
        Done
    }
}
