﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WorkScheduler;

namespace WorkScheduler.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("WorkScheduler.Models.AcademicYear", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("End");

                    b.Property<string>("Name");

                    b.Property<DateTime>("Start");

                    b.HasKey("Id");

                    b.ToTable("AcademicYears");
                });

            modelBuilder.Entity("WorkScheduler.Models.Action", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ConfirmationFormId");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<int>("Status");

                    b.Property<int>("WorkScheduleId");

                    b.HasKey("Id");

                    b.HasIndex("ConfirmationFormId");

                    b.HasIndex("WorkScheduleId");

                    b.ToTable("Actions");
                });

            modelBuilder.Entity("WorkScheduler.Models.ActionUser", b =>
                {
                    b.Property<int>("ActionId");

                    b.Property<string>("UserId");

                    b.HasKey("ActionId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ActionUsers");
                });

            modelBuilder.Entity("WorkScheduler.Models.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Color");

                    b.Property<string>("Name");

                    b.Property<int>("Priority");

                    b.HasKey("Id");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("WorkScheduler.Models.ConfirmationForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ConfirmationForms");
                });

            modelBuilder.Entity("WorkScheduler.Models.Identity.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<bool>("CanAccept");

                    b.Property<bool>("CanConfirm");

                    b.Property<bool>("CanSeeAllChecklists");

                    b.Property<bool>("CanSeeAllProtocols");

                    b.Property<bool>("CanSeeAllSchedules");

                    b.Property<bool>("CanUseChecklists");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<bool>("GetNotifications");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<int?>("SchoolId");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("SurName");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("SchoolId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Contract", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<DateTime>("ControlDate");

                    b.Property<string>("Number");

                    b.Property<int>("OrganizationId");

                    b.Property<int>("SchoolId");

                    b.Property<string>("SignedById");

                    b.Property<DateTime>("SigningDate");

                    b.Property<int?>("Status");

                    b.Property<string>("Subject");

                    b.Property<decimal>("Sum");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("SchoolId");

                    b.HasIndex("SignedById");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Family", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BirthCertificate");

                    b.Property<int>("ClarifyFamilyСomposition");

                    b.Property<int>("FamilyNumberChildren");

                    b.Property<int>("FamilyСomposition");

                    b.Property<string>("FullNameFather");

                    b.Property<string>("FullNameMather");

                    b.Property<string>("PassportNumber");

                    b.Property<string>("PhoneFather");

                    b.Property<string>("PhoneMother");

                    b.Property<int>("PhysicalGroup");

                    b.Property<string>("RegistrAddres");

                    b.Property<int>("Registration");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("ResidAddres");

                    b.Property<int>("StudentId");

                    b.Property<string>("WorkFather");

                    b.Property<string>("WorkMother");

                    b.HasKey("Id");

                    b.HasIndex("StudentId")
                        .IsUnique();

                    b.ToTable("Families");
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("SchoolId");

                    b.HasKey("Id");

                    b.HasIndex("SchoolId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Shared.AchivmentLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("AchivmentLevel");
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Shared.AchivmentResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("AchivmentResult");
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Shared.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AcademicYearId");

                    b.Property<string>("Name");

                    b.Property<int>("SchoolId");

                    b.HasKey("Id");

                    b.HasIndex("AcademicYearId");

                    b.HasIndex("SchoolId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Shared.ClassStudent", b =>
                {
                    b.Property<int>("ClassId");

                    b.Property<int>("StudentId");

                    b.HasKey("ClassId", "StudentId");

                    b.HasIndex("StudentId");

                    b.ToTable("ClassStudents");
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Shared.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AcademicYearId");

                    b.Property<string>("Name");

                    b.Property<int>("SchoolId");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("AcademicYearId");

                    b.HasIndex("SchoolId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Shared.GroupStudent", b =>
                {
                    b.Property<int>("GroupId");

                    b.Property<int>("StudentId");

                    b.Property<int>("Id");

                    b.HasKey("GroupId", "StudentId");

                    b.HasIndex("StudentId");

                    b.ToTable("GroupStudents");
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Shared.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("Birthday");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName");

                    b.Property<string>("Number");

                    b.Property<int>("SchoolId");

                    b.Property<string>("SurName");

                    b.HasKey("Id");

                    b.HasIndex("SchoolId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Shared.StudentAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ActionId");

                    b.Property<DateTime?>("EndDate");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("ActionId");

                    b.ToTable("StudentAction");
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.TalentedChildren.StudentAchivment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AchivmentLevelId");

                    b.Property<int>("AchivmentResultId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Name");

                    b.Property<int>("StudentActionId");

                    b.HasKey("Id");

                    b.HasIndex("AchivmentLevelId");

                    b.HasIndex("AchivmentResultId");

                    b.HasIndex("StudentActionId");

                    b.ToTable("StudentAchivment");
                });

            modelBuilder.Entity("WorkScheduler.Models.Protocol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActionId");

                    b.Property<string>("Attended");

                    b.Property<string>("Chairman");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Header");

                    b.Property<string>("Name");

                    b.Property<string>("Number");

                    b.Property<string>("ProtocolContentJSON");

                    b.Property<string>("Secretary");

                    b.HasKey("Id");

                    b.HasIndex("ActionId");

                    b.ToTable("Protocols");
                });

            modelBuilder.Entity("WorkScheduler.Models.Register.Association", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AcademicYearId");

                    b.Property<string>("Name");

                    b.Property<int>("SchoolId");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("AcademicYearId");

                    b.HasIndex("SchoolId");

                    b.ToTable("Associations");
                });

            modelBuilder.Entity("WorkScheduler.Models.Register.AssociationGroup", b =>
                {
                    b.Property<int>("GroupId");

                    b.Property<int>("AssociationId");

                    b.HasKey("GroupId", "AssociationId");

                    b.HasIndex("AssociationId");

                    b.ToTable("AssociationGroups");
                });

            modelBuilder.Entity("WorkScheduler.Models.Register.PlaningRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AcademicYearId");

                    b.Property<int>("AssociationId");

                    b.Property<string>("Comment");

                    b.Property<DateTime?>("Date");

                    b.Property<int>("GroupId");

                    b.Property<int?>("Hours");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AcademicYearId");

                    b.HasIndex("AssociationId");

                    b.HasIndex("GroupId");

                    b.ToTable("PlaningRecords");
                });

            modelBuilder.Entity("WorkScheduler.Models.Register.RegisterRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<long>("PlaningRecordId");

                    b.Property<int>("StudentId");

                    b.HasKey("Id");

                    b.HasIndex("PlaningRecordId");

                    b.HasIndex("StudentId");

                    b.ToTable("RegisterRecords");
                });

            modelBuilder.Entity("WorkScheduler.Models.Scheduler.Checklist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("Deadline");

                    b.Property<string>("Name");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Checklists");
                });

            modelBuilder.Entity("WorkScheduler.Models.Scheduler.TicketFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("FileId");

                    b.Property<long>("TicketId");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketFiles");
                });

            modelBuilder.Entity("WorkScheduler.Models.Shared.File", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Extension");

                    b.Property<string>("Name");

                    b.Property<string>("Path");

                    b.Property<double>("SizeMb");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("WorkScheduler.Models.Shared.LoginLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LoggedOn");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("LoginLogs");
                });

            modelBuilder.Entity("WorkScheduler.Models.Shared.Post", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Color");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Text");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("WorkScheduler.Models.Shared.School", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActionNamesToMakeProtocolJSON");

                    b.Property<string>("DocumentHeaderHTML");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("ShortName");

                    b.HasKey("Id");

                    b.ToTable("Schools");
                });

            modelBuilder.Entity("WorkScheduler.Models.Ticket", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ActionId");

                    b.Property<string>("AssignmentComment");

                    b.Property<bool>("AutoGenerated");

                    b.Property<int?>("ChecklistId");

                    b.Property<string>("Comment");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime?>("Date");

                    b.Property<bool>("Done");

                    b.Property<byte?>("Hours");

                    b.Property<bool>("Important");

                    b.Property<byte?>("Minutes");

                    b.Property<string>("Name");

                    b.Property<bool>("Notify");

                    b.Property<string>("ResponseComment");

                    b.Property<int?>("Status");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ActionId");

                    b.HasIndex("ChecklistId");

                    b.HasIndex("UserId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("WorkScheduler.Models.WorkSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AcademicYearId");

                    b.Property<int>("ActivityId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AcademicYearId");

                    b.HasIndex("ActivityId");

                    b.HasIndex("UserId");

                    b.ToTable("WorkSchedules");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WorkScheduler.Models.Identity.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WorkScheduler.Models.Identity.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Identity.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WorkScheduler.Models.Identity.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Action", b =>
                {
                    b.HasOne("WorkScheduler.Models.ConfirmationForm", "ConfirmationForm")
                        .WithMany("Actions")
                        .HasForeignKey("ConfirmationFormId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.WorkSchedule", "WorkSchedule")
                        .WithMany("Actions")
                        .HasForeignKey("WorkScheduleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.ActionUser", b =>
                {
                    b.HasOne("WorkScheduler.Models.Action", "Action")
                        .WithMany("ActionUsers")
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Identity.User", "User")
                        .WithMany("ActionUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Identity.User", b =>
                {
                    b.HasOne("WorkScheduler.Models.Shared.School", "School")
                        .WithMany("Users")
                        .HasForeignKey("SchoolId");
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Contract", b =>
                {
                    b.HasOne("WorkScheduler.Models.Monitoring.Organization", "Organization")
                        .WithMany("Contracts")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Shared.School", "School")
                        .WithMany("Contracts")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Identity.User", "SignedBy")
                        .WithMany("Contracts")
                        .HasForeignKey("SignedById");
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Family", b =>
                {
                    b.HasOne("WorkScheduler.Models.Monitoring.Shared.Student", "Student")
                        .WithOne("Family")
                        .HasForeignKey("WorkScheduler.Models.Monitoring.Family", "StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Organization", b =>
                {
                    b.HasOne("WorkScheduler.Models.Shared.School", "School")
                        .WithMany()
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Shared.Class", b =>
                {
                    b.HasOne("WorkScheduler.Models.AcademicYear", "AcademicYear")
                        .WithMany("Classes")
                        .HasForeignKey("AcademicYearId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Shared.School", "School")
                        .WithMany("Classes")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Shared.ClassStudent", b =>
                {
                    b.HasOne("WorkScheduler.Models.Monitoring.Shared.Class", "Class")
                        .WithMany("ClassStudents")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Monitoring.Shared.Student", "Student")
                        .WithMany("ClassStudents")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Shared.Group", b =>
                {
                    b.HasOne("WorkScheduler.Models.AcademicYear", "AcademicYear")
                        .WithMany("Groups")
                        .HasForeignKey("AcademicYearId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Shared.School", "School")
                        .WithMany("Groups")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Shared.GroupStudent", b =>
                {
                    b.HasOne("WorkScheduler.Models.Monitoring.Shared.Group", "Group")
                        .WithMany("GroupStudents")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Monitoring.Shared.Student", "Student")
                        .WithMany("GroupStudents")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Shared.Student", b =>
                {
                    b.HasOne("WorkScheduler.Models.Shared.School", "School")
                        .WithMany("Students")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.Shared.StudentAction", b =>
                {
                    b.HasOne("WorkScheduler.Models.Action", "Action")
                        .WithMany("StudentActions")
                        .HasForeignKey("ActionId");
                });

            modelBuilder.Entity("WorkScheduler.Models.Monitoring.TalentedChildren.StudentAchivment", b =>
                {
                    b.HasOne("WorkScheduler.Models.Monitoring.Shared.AchivmentLevel", "AchivmentLevel")
                        .WithMany("StudentAchivments")
                        .HasForeignKey("AchivmentLevelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Monitoring.Shared.AchivmentResult", "AchivmentResult")
                        .WithMany("StudentAchivments")
                        .HasForeignKey("AchivmentResultId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Monitoring.Shared.StudentAction", "StudentAction")
                        .WithMany("StudentAchivments")
                        .HasForeignKey("StudentActionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Protocol", b =>
                {
                    b.HasOne("WorkScheduler.Models.Action", "Action")
                        .WithMany("Protocols")
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Register.Association", b =>
                {
                    b.HasOne("WorkScheduler.Models.AcademicYear", "AcademicYear")
                        .WithMany("Associations")
                        .HasForeignKey("AcademicYearId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Shared.School", "School")
                        .WithMany("Associations")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Register.AssociationGroup", b =>
                {
                    b.HasOne("WorkScheduler.Models.Register.Association", "Association")
                        .WithMany("AssociationGroups")
                        .HasForeignKey("AssociationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Monitoring.Shared.Group", "Group")
                        .WithMany("AssociationGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Register.PlaningRecord", b =>
                {
                    b.HasOne("WorkScheduler.Models.AcademicYear", "AcademicYear")
                        .WithMany("PlaningRecords")
                        .HasForeignKey("AcademicYearId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Register.Association", "Association")
                        .WithMany("PlaningRecords")
                        .HasForeignKey("AssociationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Monitoring.Shared.Group", "Group")
                        .WithMany("PlaningRecords")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Register.RegisterRecord", b =>
                {
                    b.HasOne("WorkScheduler.Models.Register.PlaningRecord", "PlaningRecord")
                        .WithMany("RegisterRecords")
                        .HasForeignKey("PlaningRecordId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Monitoring.Shared.Student", "Student")
                        .WithMany("RegisterRecords")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Scheduler.Checklist", b =>
                {
                    b.HasOne("WorkScheduler.Models.Identity.User", "User")
                        .WithMany("Checklists")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WorkScheduler.Models.Scheduler.TicketFile", b =>
                {
                    b.HasOne("WorkScheduler.Models.Shared.File", "File")
                        .WithMany("TicketFiles")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Ticket", "Ticket")
                        .WithMany("TicketFiles")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WorkScheduler.Models.Shared.LoginLog", b =>
                {
                    b.HasOne("WorkScheduler.Models.Identity.User", "User")
                        .WithMany("LoginLogs")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WorkScheduler.Models.Shared.Post", b =>
                {
                    b.HasOne("WorkScheduler.Models.Identity.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WorkScheduler.Models.Ticket", b =>
                {
                    b.HasOne("WorkScheduler.Models.Action", "Action")
                        .WithMany()
                        .HasForeignKey("ActionId");

                    b.HasOne("WorkScheduler.Models.Scheduler.Checklist", "Checklist")
                        .WithMany("Tickets")
                        .HasForeignKey("ChecklistId");

                    b.HasOne("WorkScheduler.Models.Identity.User", "User")
                        .WithMany("Tickets")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WorkScheduler.Models.WorkSchedule", b =>
                {
                    b.HasOne("WorkScheduler.Models.AcademicYear", "AcademicYear")
                        .WithMany("WorkSchedules")
                        .HasForeignKey("AcademicYearId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Activity", "Activity")
                        .WithMany("WorkSchedules")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WorkScheduler.Models.Identity.User", "User")
                        .WithMany("WorkSchedules")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
