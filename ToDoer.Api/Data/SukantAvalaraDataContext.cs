using Microsoft.EntityFrameworkCore;  
using System.Collections.Generic;  
using System.Linq;  
  
namespace ToDoer.Api.Data  
{  
    public class ToDoerDataContext : DbContext  
    {  
        public DbSet<TaskList> TaskList { get; set; }  
        public DbSet<ToDoTaskDb> ToDoTaskDb { get; set; }  
  
        public ToDoerDataContext(DbContextOptions<ToDoerDataContext> options) : base(options)  
        {  
 
        }  
  
    }  
}  