using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ActivityProject.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    ESPB = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    person_type = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    TranscriptNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ActivityTypes",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    CourseID = table.Column<string>(nullable: false),
                    MaxPoints = table.Column<double>(nullable: false),
                    MinPoints = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Required = table.Column<bool>(nullable: false),
                    WeightCoefficient = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTypes", x => new { x.ID, x.CourseID });
                    table.ForeignKey(
                        name: "FK_ActivityTypes_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseStudents",
                columns: table => new
                {
                    CourseID = table.Column<string>(nullable: false),
                    StudentID = table.Column<string>(nullable: false),
                    CompletionDate = table.Column<DateTime>(nullable: true),
                    EnrollmentDate = table.Column<DateTime>(nullable: false),
                    FinalGrade = table.Column<int>(nullable: true),
                    ProposedGrade = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStudents", x => new { x.CourseID, x.StudentID });
                    table.ForeignKey(
                        name: "FK_CourseStudents_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseStudents_People_StudentID",
                        column: x => x.StudentID,
                        principalTable: "People",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    StudentID = table.Column<string>(nullable: false),
                    ProfessorID = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ActivityTypeID = table.Column<string>(nullable: false),
                    CourseID = table.Column<string>(nullable: false),
                    ActivityTypeCourseID = table.Column<string>(nullable: true),
                    ActivityTypeID1 = table.Column<string>(nullable: true),
                    Points = table.Column<double>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Valid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => new { x.StudentID, x.ProfessorID, x.Date, x.ActivityTypeID, x.CourseID });
                    table.ForeignKey(
                        name: "FK_Activities_People_ProfessorID",
                        column: x => x.ProfessorID,
                        principalTable: "People",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activities_People_StudentID",
                        column: x => x.StudentID,
                        principalTable: "People",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activities_ActivityTypes_ActivityTypeID1_ActivityTypeCourseID",
                        columns: x => new { x.ActivityTypeID1, x.ActivityTypeCourseID },
                        principalTable: "ActivityTypes",
                        principalColumns: new[] { "ID", "CourseID" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ProfessorID",
                table: "Activities",
                column: "ProfessorID");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityTypeID1_ActivityTypeCourseID",
                table: "Activities",
                columns: new[] { "ActivityTypeID1", "ActivityTypeCourseID" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTypes_CourseID",
                table: "ActivityTypes",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudents_StudentID",
                table: "CourseStudents",
                column: "StudentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "CourseStudents");

            migrationBuilder.DropTable(
                name: "ActivityTypes");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
