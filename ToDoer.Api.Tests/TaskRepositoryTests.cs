using System;
using Xunit;
using ToDoer.Api.Repository;
using ToDoer.Api.Data;
using ToDoer.Api.Domain;
using ToDoer.Api.Models;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ToDoer.Api.Tests
{
    public class TaskRepositoryTests : IClassFixture<TestDataSetup>
    {
        private readonly ToDoerDataContext _context;
        private readonly ITaskRepository _taskRepository;

        public TaskRepositoryTests(TestDataSetup fixture)
        {
            _context = fixture.Context;
            _taskRepository = new TaskRepository(_context);
        }

        [Fact]
        public void GetToDoTasks_ReturnsTasks()
        {
            //Arrange
            var query = new TaskQuery()
            {
                Id = 1
            };

            var expectedResult = _context.ToDoTaskDb.Where(i => i.Id == query.Id);

            //Act
            var result = _taskRepository.GetToDoTasks(query);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Count(), result.Count());
            Assert.Equal(expectedResult.First().Description, result.First().Description);
            Assert.Equal(expectedResult.First().Subject, result.First().Subject);
            Assert.Equal(expectedResult.First().ToDoListId, result.First().ToDoListId);
            Assert.Equal(expectedResult.First().Status, result.First().Status.ToString());
            Assert.Equal(expectedResult.First().CreateDate, result.First().CreateDate);
            Assert.Equal(expectedResult.First().Id, result.First().Id);

            //Arrange
            query = new TaskQuery()
            {
                Subject = "Test"
            };
            expectedResult = _context.ToDoTaskDb.Where(i => i.Subject == query.Subject);

            //Act
            result = _taskRepository.GetToDoTasks(query);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Count(), result.Count());
        }

        [Fact]
        public void CreateTask_AddsANewTask()
        {
            //Arrange
            var createCommand = new TaskCreateCommand()
            {
                ListId = Guid.NewGuid().ToString(),
                Description = "Unit Test Description",
                Subject = "Unit Test"
            };

            //Act
            var result = _taskRepository.CreateTask(createCommand);

            //Assert
            Assert.NotNull(result);
            var addedTask = _taskRepository.GetToDoTasks(new TaskQuery(){Id = result.Id});
            Assert.NotNull(addedTask);
            Assert.Equal(createCommand.Description, addedTask.First().Description);
            Assert.NotNull(result.CreateDate);
        }

        [Fact]
        public void DeleteTask_DeletesTask_ThatExists()
        {
            //Arrange
            var createCommand = new TaskCreateCommand()
            {
                ListId = Guid.NewGuid().ToString(),
                Description = "Delete Unit Test Description",
                Subject = "Unit Test"
            };
            var addedTask = _taskRepository.CreateTask(createCommand);

            //Act
            var result = _taskRepository.DeleteTask(addedTask.Id);

            //Assert
            Assert.NotNull(result);
            var deletedTask = _taskRepository.GetToDoTasks(new TaskQuery(){Id = result.Id});
            Assert.Null(deletedTask.FirstOrDefault());
        }

        [Fact]
        public void DeleteTask_ReturnsNull_WhenTaskDoesNotExists()
        {
            //Arrange

            //Act
            var result = _taskRepository.DeleteTask(1001);

            //Assert
            Assert.Null(result);
            var deletedList = _taskRepository.GetToDoTasks(new TaskQuery(){Id = 1001});
            Assert.Null(deletedList.FirstOrDefault());
        }

        [Fact]
        public void DeleteTask_DeletesTaskInAList_ThatExists()
        {
            //Arrange
            var listId = Guid.NewGuid().ToString();
            var createCommand = new TaskCreateCommand()
            {
                ListId = listId,
                Description = "Delete Unit Test Description",
                Subject = "UnitTest-Sub1"
            };
            var addedTask = _taskRepository.CreateTask(createCommand);
            createCommand = new TaskCreateCommand()
            {
                ListId = listId,
                Description = "Delete Unit Test Description",
                Subject = "UnitTest-Sub1"
            };
            addedTask = _taskRepository.CreateTask(createCommand);

            var addedTasks = _taskRepository.GetToDoTasks(new TaskQuery(){ListId = listId, Subject = "UnitTest-Sub1"});

            //Act
            var result = _taskRepository.DeleteTask(new TaskQuery(){ListId = listId, Subject = "UnitTest-Sub1"});

            //Assert
            Assert.NotNull(result);
            Assert.Equal(addedTasks.Count(), result.Count());
            var deletedTask = _taskRepository.GetToDoTasks(new TaskQuery(){ListId = listId, Subject = "UnitTest-Sub1"});
            Assert.Null(deletedTask.FirstOrDefault());
        }

        [Fact]
        public void UpdateTask_UpdatesTask_ThatExists()
        {
            //Arrange
            var listId = Guid.NewGuid().ToString();
            var createCommand = new TaskCreateCommand()
            {
                ListId = listId,
                Description = "Update Unit Test Description",
                Subject = "UnitTest-Sub3"
            };
            var addedTask = _taskRepository.CreateTask(createCommand);

            //Act
            var result = _taskRepository.UpdateTask(new TaskUpdateCommand(){Id = addedTask.Id, Status = TaskStatus.InProgress});

            //Assert
            Assert.NotNull(result);
            var updatedTask = _taskRepository.GetToDoTasks(new TaskQuery(){Id = result.Id}).First();
            Assert.NotNull(updatedTask);
            Assert.Equal(TaskStatus.InProgress,updatedTask.Status);
        }
    }
}
