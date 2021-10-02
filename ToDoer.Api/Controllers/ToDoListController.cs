using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDoer.Api.Domain;
using ToDoer.Api.Data;
using Newtonsoft.Json;
using ToDoer.Api.Repository;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace ToDoer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoListController : ControllerBase
    {
        private readonly ILogger<ToDoListController> _logger;
        private readonly ITaskListRepository _taskListRepo;

        public ToDoListController(ILogger<ToDoListController> logger, ITaskListRepository taskListRepo)
        {
            _logger = logger;
            _taskListRepo = taskListRepo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Delete a task list</remarks>
        /// <param name="listDetails"></param>
        /// <response code="200">OK</response>
        /// <response code="401">Bad Request</response>
        /// <response code="404">NotFound</response>
        [HttpDelete]
        [Route("/ToDoList/DeleteList")]
        [SwaggerResponse(statusCode: 200, type: typeof(ToDoList), description: "OK")]
        [SwaggerResponse(statusCode: 401, type: typeof(ListDetails), description: "Bad Request")]
        public virtual IActionResult Delete([FromQuery]ListDetails listDetails)
        {
            if(listDetails.Equals(new ListDetails()) || listDetails.Equals(null))
            {
                return BadRequest();
            }

            var result = _taskListRepo.DeleteList(listDetails);

            if(result != null)
            {
                return new OkObjectResult(result);
            }

            return new NotFoundObjectResult(listDetails);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns all task lists</remarks>
        /// <response code="200">OK</response>
        /// <response code="401">Bad Request</response>
        /// <response code="404">Not Found</response>
        [HttpGet]
        [Route("/ToDoList")]
        public virtual IActionResult GetLists()
        { 
            var response = _taskListRepo.GetToDoLists();
            return new OkObjectResult(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Get a task list</remarks>
        /// <param name="listDetails"></param>
        /// <response code="200">OK</response>
        /// <response code="401">Bad Request</response>
        /// <response code="404">NotFound</response>
        [HttpGet]
        [Route("/ToDoList/List")]
        public virtual IActionResult GetList([FromQuery]ListDetails listDetails)
        { 
            if(listDetails.Equals(new ListDetails()) || listDetails.Equals(null))
            {
                return BadRequest();
            }

            var result = _taskListRepo.GetTaskList(listDetails);

            if(result != null)
            {
                return new OkObjectResult(result);
            }

            return new NotFoundObjectResult(listDetails);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Create a new task list</remarks>
        /// <param name="taskName">Name of the task list</param>
        /// <response code="200">OK</response>
        /// <response code="401">Bad Request</response>
        [HttpPost]
        [Route("/ToDoList")]
        public virtual IActionResult CreateList([FromBody]ListDetails listDetails)
        {
            var taskList = new TaskList()
            {
                Name = listDetails.Name,
                CreateDate = DateTime.Now,
                Something = listDetails.Something
            };

            var result = _taskListRepo.CreateTaskList(taskList);

            return result != null ? new OkObjectResult(result) : new ObjectResult(StatusCode(500));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Update a task list - The query parameter input can be either name of the list or guid of the list or both. The request body should contain the new name of the list</remarks>
        /// <param name="UpdateListQuery"></param>
        /// <response code="200">OK</response>
        /// <response code="401">Bad Request</response>
        /// <response code="404">NotFound</response>
        [HttpPatch]
        [Route("/ToDoList/UpdateToDoList")]
        public virtual IActionResult UpdateList([FromQuery]UpdateListQuery updateListQuery)
        { 
            if((updateListQuery.Id == null && string.IsNullOrWhiteSpace(updateListQuery.CurrentName)) || (string.IsNullOrWhiteSpace(updateListQuery.NewName)))
            {
                return BadRequest();
            }

            var result = _taskListRepo.UpdateList(updateListQuery);

            if(result != null)
            {
                return new OkObjectResult(result);
            }

            return new NotFoundObjectResult(updateListQuery);
        }
    }
}
