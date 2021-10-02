using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ToDoer.Api.Domain
{
    [DataContract]
    public class ToDoTask
    {
        public int Id {get;set;}
        public string ToDoListId {get;set;}
        public string Description {get;set;}
        public string Subject {get;set;}
        public DateTime CreateDate {get;set;}
        public TaskStatus Status {get;set;}
    }
}