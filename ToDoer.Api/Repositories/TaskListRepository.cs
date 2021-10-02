using System;
using ToDoer.Api.Data;
using ToDoer.Api.Domain;
using ToDoer.Api.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace ToDoer.Api.Repository
{
    public interface ITaskListRepository
    {
        List<ToDoList> GetToDoLists();
        ToDoList CreateTaskList(TaskList taskList);
        TaskList DeleteList(ListDetails listDetails);
        TaskList UpdateList(UpdateListQuery updateListQuery);
        TaskList GetTaskList(ListDetails listDetails);
    }

    public class TaskListRepository : ITaskListRepository
    {
        private readonly ToDoerDataContext _context;
        private readonly ITaskRepository _taskRepo;
        private readonly ILogger<ITaskListRepository> _logger;

        public TaskListRepository(ToDoerDataContext context, ITaskRepository taskRepo, ILogger<ITaskListRepository> logger)
        {
            _context = context;
            _taskRepo = taskRepo;
            _logger = logger;
        }

        public List<ToDoList> GetToDoLists()
        {
            var taskListCollection = _context.TaskList.ToList().Select(x => new ToDoList(){Id = x.Id, Name = x.Name, CreateDate = x.CreateDate });
            return taskListCollection.ToList();
        }

        public ToDoList CreateTaskList(TaskList taskList)
        {
            try
            {
                _context.TaskList.Add(taskList);
                _context.SaveChanges();

                return new ToDoList()
                {
                    Id = taskList.Id,
                    Name = taskList.Name,
                    CreateDate = taskList.CreateDate,
                    Something = taskList.Something
                };
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex.ToString());
            }

            return null;
        }

        public TaskList DeleteList(ListDetails listDetails)
        {
            try
            {
                var list = FilterTask(listDetails);

                if (list != null)  
                {  
                    _taskRepo.DeleteTask(new TaskQuery(){ListId = list.Id.ToString()});
                    
                    _context.TaskList.Remove(list);  
                    _context.SaveChanges();  
                    return list;
                }
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex.ToString());
            }
            
            return null;
        }

        public TaskList UpdateList(UpdateListQuery updateListQuery)
        {
            try
            {
                var list = FilterTask(new ListDetails(){Id = updateListQuery.Id, Name = updateListQuery.CurrentName});

                if (list != null)  
                {
                    list.Name = updateListQuery.NewName;  
                    _context.TaskList.Update(list);  
                    _context.SaveChanges();  
                    
                    return list;
                }
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex.ToString());
            }

            return null;
        }

        public TaskList GetTaskList(ListDetails listDetails)
        {
            return FilterTask(listDetails);
        }

        private TaskList FilterTask(ListDetails listDetails)
        {   
            if(listDetails.Id != null && listDetails.Name != null)
            {
                return _context.TaskList.Where(x => x.Id == new Guid(listDetails.Id) && x.Name == listDetails.Name).FirstOrDefault(); 
            }

            if(listDetails.Id != null)
            {
                return _context.TaskList.Where(x => x.Id == new Guid(listDetails.Id)).FirstOrDefault(); 
            }

            if(listDetails.Name != null)
            {
                return _context.TaskList.Where(x => x.Name == listDetails.Name).FirstOrDefault(); 
            }

            if(listDetails.Something != null)
            {
                return _context.TaskList.Where(x =>x.Something == listDetails.Something).FirstOrDefault();
            }

            return null;
        }
    }
}