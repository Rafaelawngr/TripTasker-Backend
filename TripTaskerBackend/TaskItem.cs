using System;
using static TripTaskerBackend.Tasks;

namespace TripTaskerBackend
{
    public class TaskItem
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskProgress Status { get; set; }
        public DateTime DueDate { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }


    }
    public static class TaskHelpers
    {
        public static string GetTaskStatusString(TaskProgress status)
        {
            switch (status)
            {
                
                case TaskProgress.InProgress:
                    return "Fazendo";
                case TaskProgress.Done:
                    return "Completo";
                case TaskProgress.ToDo:
                default:
                    return "A Fazer"; 
            }
        }
    }
}