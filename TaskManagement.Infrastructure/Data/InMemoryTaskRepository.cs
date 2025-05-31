using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Infrastructure.Data
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly List<Domain.Entities.Task> _tasks;
        private readonly List<User> _users;
        private int _nextTaskId = 1;

        public InMemoryTaskRepository()
        {
            _users = new List<User>
            {
                new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
                new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" },
                new User { Id = 3, Name = "Bob Johnson", Email = "bob@example.com" }
            };

            _tasks = new List<Domain.Entities.Task>
            {
                new Domain.Entities.Task
                {
                    Id = _nextTaskId++,
                    Title = "Setup Development Environment",
                    Description = "Install required tools and configure development environment",
                    DueDate = DateTime.Now.AddDays(7),
                    Priority = TaskPriority.High,
                    Status = Domain.Entities.TaskStatus.ToDo,
                    AssignedUserId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Domain.Entities.Task
                {
                    Id = _nextTaskId++,
                    Title = "Write Unit Tests",
                    Description = "Create comprehensive unit tests for core functionality",
                    DueDate = DateTime.Now.AddDays(5),
                    Priority = TaskPriority.Medium,
                    Status = Domain.Entities.TaskStatus.InProgress,
                    AssignedUserId = 2,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            // Set navigation properties
            foreach (var task in _tasks)
            {
                task.AssignedUser = _users.FirstOrDefault(u => u.Id == task.AssignedUserId);
            }
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetAllAsync()
        {
            await Task.Delay(10); // Simulate async operation
            return _tasks.ToList();
        }

        public async Task<Domain.Entities.Task?> GetByIdAsync(int id)
        {
            await Task.Delay(10);
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetByUserIdAsync(int userId)
        {
            await Task.Delay(10);
            return _tasks.Where(t => t.AssignedUserId == userId).ToList();
        }

        public async Task<Domain.Entities.Task> CreateAsync(Domain.Entities.Task task)
        {
            await Task.Delay(10);
            
            task.Id = _nextTaskId++;
            task.AssignedUser = _users.FirstOrDefault(u => u.Id == task.AssignedUserId);
            
            _tasks.Add(task);
            return task;
        }

        public async Task<Domain.Entities.Task> UpdateAsync(Domain.Entities.Task task)
        {
            await Task.Delay(10);
            
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask != null)
            {
                var index = _tasks.IndexOf(existingTask);
                task.AssignedUser = _users.FirstOrDefault(u => u.Id == task.AssignedUserId);
                _tasks[index] = task;
            }
            
            return task;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await Task.Delay(10);
            
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _tasks.Remove(task);
                return true;
            }
            
            return false;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            await Task.Delay(10);
            return _tasks.Any(t => t.Id == id);
        }
    }
}