using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkScheduler.Models;
using WorkScheduler.Models.Identity;

namespace WorkScheduler
{
    public class Context : IdentityDbContext<User>
    {
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<ActionUser> ActionUsers { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ConfirmationForm> ConfirmationForms { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

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
