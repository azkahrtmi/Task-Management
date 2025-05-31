using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;
        public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
    }
}