using ToDoer.Api.Domain;

namespace ToDoer.Api.Models
{
    public class TaskQuery
    {
        public int Id {get;set;}
        public string ListId {get;set;}
        public string Description {get;set;}
        public string Subject {get;set;}
        public TaskStatus? Status {get;set;}
    }
}