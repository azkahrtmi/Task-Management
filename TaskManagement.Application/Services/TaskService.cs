using Microsoft.Extensions.Logging;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TaskService> _logger;

        public TaskService(
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            _logger.LogInformation("Retrieving all tasks");
            
            var tasks = await _taskRepository.GetAllAsync();
            return tasks.Select(MapToDto);
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving task with ID: {TaskId}", id);
            
            var task = await _taskRepository.GetByIdAsync(id);
            return task != null ? MapToDto(task) : null;
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(int userId)
        {
            _logger.LogInformation("Retrieving tasks for user ID: {UserId}", userId);
            
            var tasks = await _taskRepository.GetByUserIdAsync(userId);
            return tasks.Select(MapToDto);
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            _logger.LogInformation("Creating new task: {TaskTitle}", createTaskDto.Title);
            
            // Validation
            if (createTaskDto.DueDate <= DateTime.Now)
            {
                throw new ArgumentException("Due date cannot be in the past");
            }

            if (createTaskDto.AssignedUserId.HasValue)
            {
                var userExists = await _userRepository.ExistsAsync(createTaskDto.AssignedUserId.Value);
                if (!userExists)
                {
                    throw new ArgumentException("Assigned user does not exist");
                }
            }

            var task = new Domain.Entities.Task
            {
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                DueDate = createTaskDto.DueDate,
                Priority = createTaskDto.Priority,
                Status = Domain.Entities.TaskStatus.ToDo,
                AssignedUserId = createTaskDto.AssignedUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdTask = await _taskRepository.CreateAsync(task);
            
            _logger.LogInformation("Task created successfully with ID: {TaskId}", createdTask.Id);
            
            return MapToDto(createdTask);
        }

        public async Task<TaskDto> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto)
        {
            _logger.LogInformation("Updating task with ID: {TaskId}", id);
            
            var existingTask = await _taskRepository.GetByIdAsync(id);
            if (existingTask == null)
            {
                throw new ArgumentException("Task not found");
            }

            // Validation
            if (updateTaskDto.DueDate.HasValue && updateTaskDto.DueDate <= DateTime.Now)
            {
                throw new ArgumentException("Due date cannot be in the past");
            }

            if (updateTaskDto.AssignedUserId.HasValue)
            {
                var userExists = await _userRepository.ExistsAsync(updateTaskDto.AssignedUserId.Value);
                if (!userExists)
                {
                    throw new ArgumentException("Assigned user does not exist");
                }
            }

            // Update properties
            if (!string.IsNullOrWhiteSpace(updateTaskDto.Title))
                existingTask.Title = updateTaskDto.Title;
            
            if (updateTaskDto.Description != null)
                existingTask.Description = updateTaskDto.Description;
            
            if (updateTaskDto.DueDate.HasValue)
                existingTask.DueDate = updateTaskDto.DueDate.Value;
            
            if (updateTaskDto.Priority.HasValue)
                existingTask.Priority = updateTaskDto.Priority.Value;
            
            if (updateTaskDto.Status.HasValue)
                existingTask.Status = updateTaskDto.Status.Value;
            
            if (updateTaskDto.AssignedUserId.HasValue)
                existingTask.AssignedUserId = updateTaskDto.AssignedUserId.Value;

            existingTask.UpdatedAt = DateTime.UtcNow;

            var updatedTask = await _taskRepository.UpdateAsync(existingTask);
            
            _logger.LogInformation("Task updated successfully with ID: {TaskId}", updatedTask.Id);
            
            return MapToDto(updatedTask);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            _logger.LogInformation("Deleting task with ID: {TaskId}", id);
            
            var exists = await _taskRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new ArgumentException("Task not found");
            }

            var result = await _taskRepository.DeleteAsync(id);
            
            if (result)
            {
                _logger.LogInformation("Task deleted successfully with ID: {TaskId}", id);
            }
            
            return result;
        }

        private static TaskDto MapToDto(Domain.Entities.Task task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status,
                AssignedUserId = task.AssignedUserId,
                AssignedUserName = task.AssignedUser?.Name,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }
    }
}