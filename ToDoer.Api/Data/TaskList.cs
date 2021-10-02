using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToDoer.Api.Data
{
    public class TaskList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id {get;set;}
        public string Name {get;set;}
        public DateTime CreateDate {get;set;}
        public string Something {get;set;}
    }
}