using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDoer.Api.Domain;
using ToDoer.Api.Data;
using ToDoer.Api.Models;
using Newtonsoft.Json;
using ToDoer.Api.Repository;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using ToDoer.Api.Attributes;

namespace ToDoer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly ILogger<ToDoController> _logger;
        private readonly ITaskRepository _taskRepo;

        public ToDoController(ILogger<ToDoController> logger, ITaskRepository taskRepo)
        {
            _logger = logger;
            _taskRepo = taskRepo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Delete a task from filter critera</remarks>
        /// <param name="listDetails"></param>
        /// <response code="200">OK</response>
        /// <response code="401">Bad Request</response>
        /// <response code="404">NotFound</response>
        [HttpDelete]
        [Route("/ToDoTask/Delete")]
        [ValidateModelState]
        public virtual IActionResult Delete([FromQuery][Required()]TaskQuery taskCommand)
        {
            if(taskCommand.Equals(new TaskQuery()) || taskCommand.Equals(null))
            {
                _logger.LogError($"/ToDoTask/Delete - Bad Request - {taskCommand}");
                return BadRequest();
            }

            if(taskCommand.Id > 0)
            {
                var result = _taskRepo.DeleteTask(taskCommand.Id);

                if(result != null)
                {
                    return new OkObjectResult(result);
                }

                return new NotFoundObjectResult(taskCommand.Id);
            }

            var results = _taskRepo.DeleteTask(taskCommand);

            if(results != null)
            {
                return new OkObjectResult(results);
            }

            return new NotFoundObjectResult(results);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns all task from the filter criteria</remarks>
        /// <response code="200">OK</response>
        /// <response code="401">Bad Request</response>
        /// <response code="404">Not Found</response>
        [HttpGet]
        [Route("/ToDoTask/Get")]
        [ValidateModelState]
        public virtual IActionResult GetTasks([FromQuery]TaskQuery taskCommand)
        { 
            var response = _taskRepo.GetToDoTasks(taskCommand);
            return new OkObjectResult(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Create a new task within a list</remarks>
        /// <param name="taskName">Name of the task list</param>
        /// <response code="200">OK</response>
        /// <response code="401">Bad Request</response>
        [HttpPost]
        [Route("/ToDoTask/Create")]
        [ValidateModelState]
        public virtual IActionResult CreateTask([FromBody][Required()]TaskCreateCommand taskCreateCommand)
        {
            if(taskCreateCommand.Equals(new TaskCreateCommand()) || taskCreateCommand.Equals(null))
            {
                _logger.LogError($"/ToDoTask/Delete - Bad Request - {taskCreateCommand}");
                return BadRequest();
            }

            if(taskCreateCommand.Subject == taskCreateCommand.Description)
            {
                _logger.LogError($"/ToDoTask/Delete - Bad Request - Subject and Descrition same");
                return BadRequest("Subject and description cannot be same");
            }

            if(_taskRepo.FilterTask(new TaskQuery(){ListId = taskCreateCommand.ListId, Subject = taskCreateCommand.Subject}).Count() > 1)
            {
                _logger.LogError($"/ToDoTask/Delete - Bad Request - Same subject already exists twice");
                return BadRequest("Same subject already exists twice");
            }

            var result = _taskRepo.CreateTask(taskCreateCommand);

            return result != null ? new OkObjectResult(result) : new ObjectResult(StatusCode(500));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Update a task</remarks>
        /// <param name="UpdateListQuery"></param>
        /// <response code="200">OK</response>
        /// <response code="401">Bad Request</response>
        /// <response code="404">NotFound</response>
        [HttpPatch]
        [Route("/ToDoTask/Update")]
        [ValidateModelState]
        public virtual IActionResult UpdateTask([FromBody][Required()]TaskUpdateCommand taskUpdateCommand)
        { 
            if((taskUpdateCommand.Id == 0 && string.IsNullOrWhiteSpace(taskUpdateCommand.Description)))
            {
                _logger.LogError($"/ToDoTask/Delete - Bad Request - {taskUpdateCommand}");
                return BadRequest();
            }

            var result = _taskRepo.UpdateTask(taskUpdateCommand);

            if(result != null)
            {
                return new OkObjectResult(result);
            }

            return new NotFoundObjectResult(taskUpdateCommand);
        }
    }
}
