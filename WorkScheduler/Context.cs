using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkScheduler.Models;
using WorkScheduler.Models.Identity;
using WorkScheduler.Models.Monitoring;
using WorkScheduler.Models.Monitoring.Shared;
using WorkScheduler.Models.Monitoring.TalentedChildren;
using WorkScheduler.Models.Register;
using WorkScheduler.Models.Scheduler;
using WorkScheduler.Models.Shared;
using WorkScheduler.Models.Workflow;

namespace WorkScheduler
{
    public class Context : IdentityDbContext<User>
    {
        //Shared
        public DbSet<AcademicYear> AcademicYears { get; set; }
        //public DbSet<AcademicPeriod> AcademicPeriods { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }


        //Scheduler
        public DbSet<Action> Actions { get; set; }
        public DbSet<ActionUser> ActionUsers { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ConfirmationForm> ConfirmationForms { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketFile> TicketFiles { get; set; }
        public DbSet<Checklist> Checklists { get; set; }
        public DbSet<Protocol> Protocols { get; set; }


        //Monitoring
        public DbSet<Student> Students { get; set; }
        public DbSet<ClassStudent> ClassStudents { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Family> Families { get; set; }


        //Register
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupStudent> GroupStudents { get; set; }
        public DbSet<Association> Associations { get; set; }
        public DbSet<AssociationGroup> AssociationGroups { get; set; }
        public DbSet<PlaningRecord> PlaningRecords { get; set; }
        public DbSet<RegisterRecord> RegisterRecords { get; set; }

        //Workflow
        public DbSet<IncomingDocument> IncomingDocuments { get; set; }
        public DbSet<OutgoingDocument> OutgoingDocuments { get; set; }
        public DbSet<IncomingDocumentFile> IncomingDocumentFiles { get; set; }
        public DbSet<OutgoingDocumentFile> OutgoingDocumentFiles { get; set; }


        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>().HasOne<Family>(s => s.Family).WithOne(f => f.Student).HasForeignKey<Family>(f => f.StudentId);

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



            modelBuilder.Entity<AssociationGroup>()
             .HasKey(ag => new { ag.GroupId, ag.AssociationId });

            modelBuilder.Entity<AssociationGroup>()
                .HasOne(ag => ag.Group)
                .WithMany(g => g.AssociationGroups)
                .HasForeignKey(ag => ag.GroupId);

            modelBuilder.Entity<AssociationGroup>()
                .HasOne(ag => ag.Association)
                .WithMany(a => a.AssociationGroups)
                .HasForeignKey(ag => ag.AssociationId);



            // Многие ко многим документ - файл
            modelBuilder.Entity<IncomingDocumentFile>()
            .HasKey(idf => idf.Id);

            modelBuilder.Entity<IncomingDocumentFile>()
                .HasOne(idf => idf.IncomingDocument)
                .WithMany(id => id.IncomingDocumentFiles)
                .HasForeignKey(idf => idf.IncomingDocumentId);

            modelBuilder.Entity<IncomingDocumentFile>()
                .HasOne(incomingDocumentFile => incomingDocumentFile.File)
                .WithMany(file => file.IncomingDocumentFiles)
                .HasForeignKey(idf => idf.FileId);

            modelBuilder.Entity<OutgoingDocumentFile>()
            .HasKey(odf => odf.Id);

            modelBuilder.Entity<OutgoingDocumentFile>()
                .HasOne(odf => odf.OutgoingDocument)
                .WithMany(od => od.OutgoingDocumentFiles)
                .HasForeignKey(odf => odf.OutgoingDocumentId);

            modelBuilder.Entity<OutgoingDocumentFile>()
                .HasOne(outgoingDocumentFile => outgoingDocumentFile.File)
                .WithMany(file => file.OutgoingDocumentFiles)
                .HasForeignKey(odf => odf.FileId);

        }
    }
}
