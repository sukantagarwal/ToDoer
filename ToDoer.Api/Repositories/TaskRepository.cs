using System;
using ToDoer.Api.Data;
using ToDoer.Api.Domain;
using ToDoer.Api.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ToDoer.Api.Repository
{

    public interface ITaskRepository
    {
        List<ToDoTask> GetToDoTasks(TaskQuery taskCommand);
        ToDoTask CreateTask(TaskCreateCommand taskCommand);
        ToDoTask DeleteTask(int taskId);
        List<ToDoTask> DeleteTask(TaskQuery taskCommand);
        ToDoTask UpdateTask(TaskUpdateCommand taskCommand);
        List<ToDoTaskDb> FilterTask(TaskQuery taskCommand);
    }

    public class TaskRepository : ITaskRepository
    {
        private readonly ToDoerDataContext _context;

        public TaskRepository(ToDoerDataContext context)
        {
            _context = context;
        }

        public List<ToDoTask> GetToDoTasks(TaskQuery taskCommand)
        {
            var taskCollection = FilterTask(taskCommand)
                                .Select(x => new ToDoTask()
                                {
                                    Id = x.Id, 
                                    ToDoListId = x.ToDoListId, 
                                    Description = x.Description,
                                    Subject = x.Subject,
                                    CreateDate = x.CreateDate,
                                    Status = (TaskStatus)Enum.Parse(typeof(TaskStatus), x.Status)
                                });
            return taskCollection.ToList();
        }

        public ToDoTask CreateTask(TaskCreateCommand taskCommand)
        {
            var todoTask = new ToDoTaskDb()
            {
                Subject = taskCommand.Subject,
                Description = taskCommand.Description,
                ToDoListId = taskCommand.ListId,
                CreateDate = DateTime.Now,
                Status = TaskStatus.NotStarted.ToString()
            };

            _context.ToDoTaskDb.Add(todoTask);
            _context.SaveChanges();

            return new ToDoTask()
            {
                Id = todoTask.Id, 
                ToDoListId = todoTask.ToDoListId, 
                Description = todoTask.Description,
                Subject = todoTask.Subject,
                CreateDate = todoTask.CreateDate,
                Status = (TaskStatus)Enum.Parse(typeof(TaskStatus), todoTask.Status)
            };
        }

        public ToDoTask DeleteTask(int taskId)
        {
            var list = FilterTask(new TaskQuery(){Id = taskId}).FirstOrDefault();

            if (list != null)  
            {  
                _context.ToDoTaskDb.Remove(list);  
                _context.SaveChanges();  

                return new ToDoTask()
                {
                    Id = list.Id, 
                    ToDoListId = list.ToDoListId, 
                    Description = list.Description,
                    Subject = list.Subject,
                    CreateDate = list.CreateDate,
                    Status = (TaskStatus)Enum.Parse(typeof(TaskStatus), list.Status)

                };
            }

            return null;
        }

        public List<ToDoTask> DeleteTask(TaskQuery taskCommand)
        {
            var list = FilterTask(taskCommand);

            if (list.Any())  
            {  
                _context.ToDoTaskDb.RemoveRange(list);
                _context.SaveChanges();
                  
                var result = new List<ToDoTask>();
                list.ForEach(item => result.Add(new ToDoTask()
                {
                    Id = item.Id, 
                    ToDoListId = item.ToDoListId, 
                    Description = item.Description,
                    Subject = item.Subject,
                    CreateDate = item.CreateDate,
                    Status = (TaskStatus)Enum.Parse(typeof(TaskStatus), item.Status)
                }));

                return result;
            }

            return null;
        }

        public ToDoTask UpdateTask(TaskUpdateCommand taskCommand)
        {
            var list = FilterTask(new TaskQuery(){Id = taskCommand.Id}).FirstOrDefault();

            if (list != null)  
            {
                if(list.Status!=taskCommand.Status.ToString()){
                    list.Status = taskCommand.Status.ToString();
                }

                if(list.Description!=taskCommand.Description){
                    list.Description = taskCommand.Description;
                }

                if(list.Subject!= taskCommand.Subject)
                {
                    if(FilterTask(new TaskQuery(){ListId = list.ToDoListId, Subject = taskCommand.Subject}).Count() < 2)
                    {
                        list.Subject = taskCommand.Subject;
                    }
                }

                _context.ToDoTaskDb.Update(list);  
                _context.SaveChanges();  

                return new ToDoTask()
                {
                    Id = list.Id, 
                    ToDoListId = list.ToDoListId, 
                    Description = list.Description,
                    Subject = list.Subject,
                    CreateDate = list.CreateDate,
                    Status = (TaskStatus)Enum.Parse(typeof(TaskStatus), list.Status)

                };
            }

            return null;
        }

        public List<ToDoTaskDb> FilterTask(TaskQuery taskCommand)
        {   
            IQueryable<ToDoTaskDb> query = _context.ToDoTaskDb;

            if(taskCommand.Id > 0)
            {
                return query.Where(t => t.Id == taskCommand.Id).ToList();
            }

            if(taskCommand.Description != null)
            {
                query = query.Where(t => t.Description == taskCommand.Description);
            }

            if(taskCommand.ListId != null)
            {
                query =query.Where(t => t.ToDoListId == taskCommand.ListId);
            }

            if(taskCommand.Status != null)
            {
                query =query.Where(t => t.Status == taskCommand.Status.ToString());
            }

            if(taskCommand.Subject != null)
            {
                query =query.Where(t => t.Subject == taskCommand.Subject);
            }

            var result = query.ToList();
            return result;
        }
    }

    
}