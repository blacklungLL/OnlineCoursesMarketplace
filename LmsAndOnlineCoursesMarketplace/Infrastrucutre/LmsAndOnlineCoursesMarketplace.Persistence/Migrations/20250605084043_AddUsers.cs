using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LmsAndOnlineCoursesMarketplace.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new 
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    JobPosition = table.Column<string>(type: "text", nullable: false),
                    EnrollStudents = table.Column<int>(type: "integer", nullable: false),
                    CoursesCnt = table.Column<int>(type: "integer", nullable: false),
                    ReviewsCnt = table.Column<int>(type: "integer", nullable: false),
                    SubscriptionsCnt = table.Column<int>(type: "integer", nullable: false),
                    IdentityUserId = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

                // Вставка данных через InsertData (без SQL)
                for (int i = 1; i <= 11; i++)
                {
                    migrationBuilder.InsertData(
                    table: "Users",
                    columns: new[] { "Id", "Name", "Email", "JobPosition", "EnrollStudents", "CoursesCnt", "ReviewsCnt", "SubscriptionsCnt", "IdentityUserId" },
                    values: new object[] { 
                        i, 
                        $"User {i}", 
                        $"user{i}@example.com", 
                        "Teacher", 
                        0, 
                        0, 
                        0, 
                        0, 
                        null 
                    });
                }

                // Добавляем внешний ключ только после создания Users
                migrationBuilder.AddForeignKey(
                    name: "FK_Courses_Users_UserId",
                    table: "Courses",
                    column: "UserId",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);

                // migrationBuilder.CreateIndex(
                //     name: "IX_Courses_UserId",
                //     table: "Courses",
                //     column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Users_UserId",
                table: "Courses");

            migrationBuilder.DropTable("Users");
        }
    }
}
