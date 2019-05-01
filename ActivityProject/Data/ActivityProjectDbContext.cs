using ActivityProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.Data
{
    public class ActivityProjectDbContext : DbContext
    {
        public ActivityProjectDbContext(DbContextOptions options)
: base(options)
        {

        }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Activity>().HasKey(t => new { t.StudentID, t.ProfessorID, t.Date, t.ActivityTypeID, t.CourseID });
            builder.Entity<ActivityType>().HasKey(t => new { t.ID, t.CourseID });
            builder.Entity<Person>().HasDiscriminator<string>("person_type").HasValue<Student>("student").HasValue<Professor>("professor");
            builder.Entity<CourseStudent>().HasKey(t => new { t.CourseID, t.StudentID });
            builder.Entity<Activity>().HasOne(t => t.Professor).WithMany(t => t.Activities).HasForeignKey(t => t.ProfessorID).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Activity>().HasOne(t => t.Student).WithMany(t => t.Activities).HasForeignKey(t => t.StudentID).OnDelete(DeleteBehavior.Restrict);

        }

    }
}
