using System;
using Xunit;
using Moq;
using ToDoer.Api.Controllers;
using ToDoer.Api.Repository;
using ToDoer.Api.Domain;
using Microsoft.Extensions.Logging;
using ToDoer.Api.Models;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ToDoer.Api.Tests
{
    public class ToDoControllerTests
    {
        Mock<ILogger<ToDoController>> _loggerMock;
        Mock<ITaskRepository> _taskRepoMock;
        ToDoController _controller;
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<ToDoController>>();
            _taskRepoMock = new Mock<ITaskRepository>();

            _controller = new ToDoController(_loggerMock.Object, _taskRepoMock.Object);
        }

        [Fact]
        public void GetTasks_ReturnsTasks_WhenCalled()
        {
            //Arrange
            Setup();
            var responseContent = new List<ToDoTask>()
            {
                new ToDoTask(){
                    Id = 1,
                    ToDoListId = Guid.NewGuid().ToString(),
                    Description = "some description"
                },
                new ToDoTask(){
                    Id = 2,
                    ToDoListId = Guid.NewGuid().ToString(),
                    Description = "some description"
                },
            };
            _taskRepoMock.Setup(x => x.GetToDoTasks(It.IsAny<TaskQuery>())).Returns(responseContent);

            //Act
            var response = _controller.GetTasks(new Models.TaskQuery());

            //Assert
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(200, (response as OkObjectResult).StatusCode);
            Assert.Equal(responseContent, (response as OkObjectResult).Value);
        }

    }
}