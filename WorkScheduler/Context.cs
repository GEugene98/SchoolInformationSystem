using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkScheduler.Models;
using WorkScheduler.Models.Identity;
using WorkScheduler.Models.Monitoring;
using WorkScheduler.Models.Monitoring.Shared;
using WorkScheduler.Models.Monitoring.TalentedChildren;
using WorkScheduler.Models.Scheduler;
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
        public DbSet<Checklist> Checklists { get; set; }
        public DbSet<Protocol> Protocols { get; set; }

        //Monitoring
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassStudent> ClassStudents { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupStudent> GroupStudents { get; set; }

        public DbSet<StudentAchivment> StudentAchivments { get; set; }
        public DbSet<AchivmentLevel> AchivmentLevels { get; set; }
        public DbSet<AchivmentResult> AchivmentResults { get; set; }

        public DbSet<StudentAction> StudentActions { get; set; }

        public DbSet<File> Files { get; set; }
        public DbSet<TicketFile> TicketFiles { get; set; }


        public DbSet<School> Schools { get; set; }
        public DbSet<Post> Posts { get; set; }

        //public DbSet<Address> Addresses { get; set; }
        //public DbSet<Certificate> Certificates { get; set; }
        //public DbSet<Diploma> Diplomas { get; set; }
        //public DbSet<Experience> Experiences { get; set; }
        //public DbSet<Passport> Passports { get; set; }
        //public DbSet<RefCourse> RefCourses { get; set; }
        //public DbSet<Reward> Rewards { get; set; }
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

            // Многие ко многим тикет - файл
            modelBuilder.Entity<TicketFile>()
            .HasKey(tf => tf.Id);

            modelBuilder.Entity<TicketFile>()
                .HasOne(tf => tf.Ticket)
                .WithMany(ticket => ticket.TicketFiles)
                .HasForeignKey(tf => tf.TicketId);

            modelBuilder.Entity<TicketFile>()
                .HasOne(ticketFile => ticketFile.File)
                .WithMany(file => file.TicketFiles)
                .HasForeignKey(tf => tf.FileId);

            // Связь для установки ответственных за мероприятия
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

            // У класса есть учебный год => нужна историчность учеников по классам => учебным годам
            modelBuilder.Entity<ClassStudent>()
            .HasKey(t => new { t.ClassId, t.StudentId });

            modelBuilder.Entity<ClassStudent>()
                .HasOne(cs => cs.Class)
                .WithMany(c => c.ClassStudents)
                .HasForeignKey(cs => cs.ClassId);

            modelBuilder.Entity<ClassStudent>()
                .HasOne(cs => cs.Student)
                .WithMany(s => s.ClassStudents)
                .HasForeignKey(cs => cs.StudentId);

            modelBuilder.Entity<GroupStudent>()
                .HasKey(gs => new { gs.GroupId, gs.StudentId });

            modelBuilder.Entity<GroupStudent>()
                .HasOne(gs => gs.Group)
                .WithMany(g => g.GroupStudents)
                .HasForeignKey(gs => gs.GroupId);

            modelBuilder.Entity<GroupStudent>()
                .HasOne(gs => gs.Student)
                .WithMany(s => s.GroupStudents)
                .HasForeignKey(gs => gs.StudentId);
        }
    }
}
