using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Infrastructure.Data
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public InMemoryUserRepository()
        {
            _users = new List<User>
            {
                new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
                new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" },
                new User { Id = 3, Name = "Bob Johnson", Email = "bob@example.com" }
            };
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            await Task.Delay(10);
            return _users.ToList();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            await Task.Delay(10);
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<User> CreateAsync(User user)
        {
            await Task.Delay(10);
            
            user.Id = _users.Max(u => u.Id) + 1;
            _users.Add(user);
            return user;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            await Task.Delay(10);
            return _users.Any(u => u.Id == id);
        }
    }
}
