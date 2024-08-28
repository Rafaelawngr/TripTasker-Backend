using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static TripTaskerBackend.Tasks;

namespace TripTaskerBackend
{
    public class TaskItem
    {
        public int TaskId { get; set; } 
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Tasks.TaskProgress Status { get; set; }
        public DateTime DueDate { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}