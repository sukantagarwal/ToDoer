using ToDoer.Api.Domain;
using System.ComponentModel.DataAnnotations;

namespace ToDoer.Api.Models
{
    public class TaskUpdateCommand
    {
        [Required]
        public int Id {get;set;}
        [Required]
        [MaxLength(length: 30)]
        public string Description {get;set;}
        [Required]
        [MaxLength(length: 30)]
        public string Subject {get;set;}
        [Required]
        public TaskStatus Status {get;set;}
    }
}