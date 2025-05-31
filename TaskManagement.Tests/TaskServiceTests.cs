using Microsoft.Extensions.Logging;
using Moq;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ILogger<TaskService>> _mockLogger;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILogger<TaskService>>();
            _taskService = new TaskService(_mockTaskRepository.Object, _mockUserRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateTaskAsync_ValidTask_ReturnsTaskDto()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto
            {
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(7),
                Priority = TaskPriority.Medium,
                AssignedUserId = 1
            };

            var createdTask = new Domain.Entities.Task
            {
                Id = 1,
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                DueDate = createTaskDto.DueDate,
                Priority = createTaskDto.Priority,
                Status = Domain.Entities.TaskStatus.ToDo,
                AssignedUserId = createTaskDto.AssignedUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _mockUserRepository.Setup(x => x.ExistsAsync(1)).ReturnsAsync(true);
            _mockTaskRepository.Setup(x => x.CreateAsync(It.IsAny<Domain.Entities.Task>())).ReturnsAsync(createdTask);

            // Act
            var result = await _taskService.CreateTaskAsync(createTaskDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createTaskDto.Title, result.Title);
            Assert.Equal(createTaskDto.Description, result.Description);
            Assert.Equal(Domain.Entities.TaskStatus.ToDo, result.Status);
        }

        [Fact]
        public async Task CreateTaskAsync_DueDateInPast_ThrowsArgumentException()
        {
            // Arrange
            var createTaskDto = new CreateTaskDto
            {
                Title = "Test Task",
                DueDate = DateTime.Now.AddDays(-1), // Past date
                Priority = TaskPriority.Medium
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _taskService.CreateTaskAsync(createTaskDto));
        }

        [Fact]
        public async Task GetAllTasksAsync_ReturnsAllTasks()
        {
            // Arrange
            var tasks = new List<Domain.Entities.Task>
            {
                new Domain.Entities.Task { Id = 1, Title = "Task 1", DueDate = DateTime.Now.AddDays(1), Priority = TaskPriority.High, Status = Domain.Entities.TaskStatus.ToDo, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Domain.Entities.Task { Id = 2, Title = "Task 2", DueDate = DateTime.Now.AddDays(2), Priority = TaskPriority.Low, Status = Domain.Entities.TaskStatus.InProgress, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };

            _mockTaskRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetAllTasksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task DeleteTaskAsync_ExistingTask_ReturnsTrue()
        {
            // Arrange
            var taskId = 1;
            _mockTaskRepository.Setup(x => x.ExistsAsync(taskId)).ReturnsAsync(true);
            _mockTaskRepository.Setup(x => x.DeleteAsync(taskId)).ReturnsAsync(true);

            // Act
            var result = await _taskService.DeleteTaskAsync(taskId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteTaskAsync_NonExistingTask_ThrowsArgumentException()
        {
            // Arrange
            var taskId = 999;
            _mockTaskRepository.Setup(x => x.ExistsAsync(taskId)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _taskService.DeleteTaskAsync(taskId));
        }
    }
}