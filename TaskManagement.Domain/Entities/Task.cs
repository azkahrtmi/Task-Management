using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Entities
{
    public class Task
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public TaskPriority Priority { get; set; }
        [Required]
        public TaskStatus Status { get; set; }
        public int? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}