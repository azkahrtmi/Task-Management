namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Entities.Task>> GetAllAsync();
        Task<Entities.Task?> GetByIdAsync(int id);
        Task<IEnumerable<Entities.Task>> GetByUserIdAsync(int userId);
        Task<Entities.Task> CreateAsync(Entities.Task task);
        Task<Entities.Task> UpdateAsync(Entities.Task task);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}