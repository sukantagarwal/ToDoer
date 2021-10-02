using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToDoer.Api.Data
{
    public class ToDoTaskDb
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id {get;set;}
        public string ToDoListId {get;set;}
        public string Description {get;set;}
        public string Subject {get;set;}
        public DateTime CreateDate {get;set;}
        public string Status {get;set;}
    }
}