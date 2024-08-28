using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static TripTaskerBackend.Tasks;

namespace TripTaskerBackend
{
    public class Trip
    {
        public int TripId { get; set; }
        public string Title { get; set; }
        public ICollection<TaskItem> TaskItems { get; set; }
    }
}