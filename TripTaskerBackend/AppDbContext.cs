using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripTaskerBackend;
using System.Data.Entity;

namespace TripTaskerBackend
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TaskItem>()
                .HasKey(t => t.TaskId);

            // Configura a relação com a entidade Trip (se existir)
            modelBuilder.Entity<TaskItem>()
                .HasRequired(t => t.Trip)
                .WithMany(tr => tr.TaskItems)
                .HasForeignKey(t => t.TripId);

            base.OnModelCreating(modelBuilder);
        }
    }

}


