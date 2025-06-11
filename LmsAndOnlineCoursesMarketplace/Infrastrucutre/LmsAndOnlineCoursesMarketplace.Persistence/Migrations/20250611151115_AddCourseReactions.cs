using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LmsAndOnlineCoursesMarketplace.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseReactions",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    IsLike = table.Column<bool>(type: "boolean", nullable: false),
                    IsDislike = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseReactions", x => new { x.CourseId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CourseReactions_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseReactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseReactions_CourseId_UserId",
                table: "CourseReactions",
                columns: new[] { "CourseId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseReactions_UserId",
                table: "CourseReactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseReactions");
        }
    }
}
