using System;
using Xunit;
using ToDoer.Api.Repository;
using ToDoer.Api.Data;
using ToDoer.Api.Domain;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ToDoer.Api.Tests
{
    public class TaskListRepositoryTests : IClassFixture<TestDataSetup>
    {
        private readonly ToDoerDataContext _context;
        private readonly ITaskListRepository _taskListRepository;
        private readonly ITaskRepository _taskRepository;
        Mock<ILogger<ITaskListRepository>> _loggerMock;

        public TaskListRepositoryTests(TestDataSetup fixture)
        {
            _context = fixture.Context;
            _taskRepository = new TaskRepository(_context);

            _loggerMock = new Mock<ILogger<ITaskListRepository>>();
            _taskListRepository = new TaskListRepository(_context, _taskRepository, _loggerMock.Object);
        }

        [Fact]
        public void GetToDoLists_ReturnsValidResult()
        {
            //Act
            var result = _taskListRepository.GetToDoLists();

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateTaskList_AddsANewTask()
        {
            //Arrange
            var taskList = new TaskList()
            {
                Name = "abc",
                CreateDate = DateTime.Now
            };

            //Act
            var result = _taskListRepository.CreateTaskList(taskList);

            //Assert
            Assert.NotNull(result);
            var addedList = _taskListRepository.GetTaskList(new ListDetails(){Id = result.Id.ToString()});
            Assert.NotNull(addedList);
        }

        [Fact]
        public void DeleteList_DeletesTask_ThatExists()
        {
            //Arrange
            var taskList = new TaskList()
            {
                Name = "delete task list",
                CreateDate = DateTime.Now
            };
            var createdList = _taskListRepository.CreateTaskList(taskList);

            //Act
            var result = _taskListRepository.DeleteList(new ListDetails(){Id = createdList.Id.ToString()});

            //Assert
            Assert.NotNull(result);
            var deletedList = _taskListRepository.GetTaskList(new ListDetails(){Id = result.Id.ToString()});
            Assert.Null(deletedList);
        }

        [Fact]
        public void DeleteList_ReturnsNull_WhenTaskDoesNotExists()
        {
            //Arrange
            var guid = Guid.NewGuid();

            //Act
            var result = _taskListRepository.DeleteList(new ListDetails(){Id = guid.ToString()});

            //Assert
            Assert.Null(result);
            var deletedList = _taskListRepository.GetTaskList(new ListDetails(){Id = guid.ToString()});
            Assert.Null(deletedList);
        }

        [Fact]
        public void UpdateList_UpdatesTask_ThatExists()
        {
            //Arrange
            var taskList = new TaskList()
            {
                Name = "Update task list",
                CreateDate = DateTime.Now
            };
            var createdList = _taskListRepository.CreateTaskList(taskList);

            //Act
            var result = _taskListRepository.UpdateList(new UpdateListQuery(){Id = createdList.Id.ToString(), CurrentName = createdList.Name, NewName = "Update task list new"});

            //Assert
            Assert.NotNull(result);
            var updatedList = _taskListRepository.GetTaskList(new ListDetails(){Id = result.Id.ToString()});
            Assert.NotNull(updatedList);
            Assert.Equal("Update task list new",updatedList.Name);
        }

        [Fact]
        public void UpdateList_ReturnsNull_WhenTaskDoesNotExists()
        {
            //Arrange
            var guid = Guid.NewGuid();

            //Act
            var result = _taskListRepository.UpdateList(new UpdateListQuery(){Id = guid.ToString()});

            //Assert
            Assert.Null(result);
            var deletedList = _taskListRepository.GetTaskList(new ListDetails(){Id = guid.ToString()});
            Assert.Null(deletedList);
        }
    }
}
