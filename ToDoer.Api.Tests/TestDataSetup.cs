using System;
using Xunit;
using ToDoer.Api.Domain;
using ToDoer.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace ToDoer.Api.Tests
{
    public class TestDataSetup : IDisposable
    {
        public ToDoerDataContext Context {get;set;}
        
        public TestDataSetup()
        {
            var builder = new DbContextOptionsBuilder<ToDoerDataContext>()
                        .UseInMemoryDatabase(databaseName: "TestDb")
                        .Options;
            Context = new ToDoerDataContext(builder);

            var list1Guid = Guid.NewGuid();
            Context.TaskList.Add(new TaskList(){
                Id = list1Guid,
                Name = "List1",
                CreateDate = DateTime.Now
            });
            
            var list2Guid = Guid.NewGuid();
            Context.TaskList.Add(new TaskList(){
                Id = list2Guid,
                Name = "List2",
                CreateDate = DateTime.Now
            });

            Context.ToDoTaskDb.Add(new ToDoTaskDb()
            {
                Id = 1,
                ToDoListId = list1Guid.ToString(),
                Description = "Sample Description",
                Subject = "Test",
                CreateDate = DateTime.Now,
                Status = TaskStatus.NotStarted.ToString()
            });

            Context.ToDoTaskDb.Add(new ToDoTaskDb()
            {
                Id = 2,
                ToDoListId = list2Guid.ToString(),
                Description = "Sample Description",
                Subject = "Test",
                CreateDate = DateTime.Now,
                Status = TaskStatus.NotStarted.ToString()
            });

            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}