﻿// <auto-generated />
using ActivityProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace ActivityProject.Migrations
{
    [DbContext(typeof(ActivityProjectDbContext))]
    [Migration("20190428232532_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ActivityProject.Models.Activity", b =>
                {
                    b.Property<string>("StudentID");

                    b.Property<string>("ProfessorID");

                    b.Property<DateTime>("Date");

                    b.Property<string>("ActivityTypeID");

                    b.Property<string>("CourseID");

                    b.Property<string>("ActivityTypeCourseID");

                    b.Property<string>("ActivityTypeID1");

                    b.Property<double>("Points");

                    b.Property<bool>("Status");

                    b.Property<bool>("Valid");

                    b.HasKey("StudentID", "ProfessorID", "Date", "ActivityTypeID", "CourseID");

                    b.HasIndex("ProfessorID");

                    b.HasIndex("ActivityTypeID1", "ActivityTypeCourseID");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("ActivityProject.Models.ActivityType", b =>
                {
                    b.Property<string>("ID");

                    b.Property<string>("CourseID");

                    b.Property<double>("MaxPoints");

                    b.Property<double>("MinPoints");

                    b.Property<string>("Name");

                    b.Property<bool>("Required");

                    b.Property<double>("WeightCoefficient");

                    b.HasKey("ID", "CourseID");

                    b.HasIndex("CourseID");

                    b.ToTable("ActivityTypes");
                });

            modelBuilder.Entity("ActivityProject.Models.Course", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ESPB");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("ActivityProject.Models.CourseStudent", b =>
                {
                    b.Property<string>("CourseID");

                    b.Property<string>("StudentID");

                    b.Property<DateTime?>("CompletionDate");

                    b.Property<DateTime>("EnrollmentDate");

                    b.Property<int?>("FinalGrade");

                    b.Property<int?>("ProposedGrade");

                    b.HasKey("CourseID", "StudentID");

                    b.HasIndex("StudentID");

                    b.ToTable("CourseStudents");
                });

            modelBuilder.Entity("ActivityProject.Models.Person", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("person_type")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("People");

                    b.HasDiscriminator<string>("person_type").HasValue("Person");
                });

            modelBuilder.Entity("ActivityProject.Models.Professor", b =>
                {
                    b.HasBaseType("ActivityProject.Models.Person");

                    b.Property<string>("Password");

                    b.Property<string>("Position");

                    b.Property<string>("Username");

                    b.ToTable("Professor");

                    b.HasDiscriminator().HasValue("professor");
                });

            modelBuilder.Entity("ActivityProject.Models.Student", b =>
                {
                    b.HasBaseType("ActivityProject.Models.Person");

                    b.Property<string>("TranscriptNumber");

                    b.ToTable("Student");

                    b.HasDiscriminator().HasValue("student");
                });

            modelBuilder.Entity("ActivityProject.Models.Activity", b =>
                {
                    b.HasOne("ActivityProject.Models.Professor", "Professor")
                        .WithMany("Activities")
                        .HasForeignKey("ProfessorID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ActivityProject.Models.Student", "Student")
                        .WithMany("Activities")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ActivityProject.Models.ActivityType", "ActivityType")
                        .WithMany()
                        .HasForeignKey("ActivityTypeID1", "ActivityTypeCourseID");
                });

            modelBuilder.Entity("ActivityProject.Models.ActivityType", b =>
                {
                    b.HasOne("ActivityProject.Models.Course", "Course")
                        .WithMany("ActivityTypes")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ActivityProject.Models.CourseStudent", b =>
                {
                    b.HasOne("ActivityProject.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ActivityProject.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
