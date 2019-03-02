using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkScheduler.Models;
using WorkScheduler.Models.Identity;
using WorkScheduler.Models.Monitoring;
using WorkScheduler.Models.Shared;

namespace WorkScheduler
{
    public class Context : IdentityDbContext<User>
    {
        //Scheduler
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<ActionUser> ActionUsers { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ConfirmationForm> ConfirmationForms { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }

        //Monitoring
        //public DbSet<Address> Addresses { get; set; }
        //public DbSet<Certificate> Certificates { get; set; }
        //public DbSet<Diploma> Diplomas { get; set; }
        //public DbSet<Experience> Experiences { get; set; }
        //public DbSet<Passport> Passports { get; set; }
        //public DbSet<RefCourse> RefCourses { get; set; }
        //public DbSet<Reward> Rewards { get; set; }
        //public DbSet<Student> Students { get; set; }
        //public DbSet<StudentAchivment> StudentAchivments { get; set; }
        //public DbSet<Vacation> Vacations { get; set; }
        //public DbSet<Worker> Workers { get; set; }
        //public DbSet<WorkerAchivment> WorkerAchivments { get; set; }
        //public DbSet<WorkHistory> WorkHistory { get; set; }
        //public DbSet<WorkPeriod> WorkPeriods { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ActionUser>()
            .HasKey(t => new { t.ActionId, t.UserId });

            modelBuilder.Entity<ActionUser>()
                .HasOne(au => au.Action)
                .WithMany(action => action.ActionUsers)
                .HasForeignKey(au => au.ActionId);

            modelBuilder.Entity<ActionUser>()
                .HasOne(actionResponsible => actionResponsible.User)
                .WithMany(responsible => responsible.ActionUsers)
                .HasForeignKey(au => au.UserId);
        }
    }
}
