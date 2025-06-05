using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LmsAndOnlineCoursesMarketplace.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ImageLink = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<decimal>(type: "numeric", nullable: false),
                    RatingsCnt = table.Column<int>(type: "integer", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    LastUpdate = table.Column<int>(type: "integer", nullable: false),
                    Views = table.Column<int>(type: "integer", nullable: false),
                    LikesCnt = table.Column<int>(type: "integer", nullable: false),
                    DislikesCnt = table.Column<int>(type: "integer", nullable: false),
                    SharedCnt = table.Column<int>(type: "integer", nullable: false),
                    Requirements = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Duration = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });
            
            migrationBuilder.InsertData(
            table: "Courses",
            columns: new[] { 
                "Id", 
                "Title", 
                "ShortDescription", 
                "UserId", 
                "ImageLink", 
                "Rating", 
                "RatingsCnt", 
                "Language", 
                "LastUpdate", 
                "Views", 
                "LikesCnt", 
                "DislikesCnt", 
                "SharedCnt", 
                "Requirements", 
                "Description",
                "Duration",
                "Category",
                "Price",
                "CreatedDate",
                "CreatedBy",
                "UpdatedDate",
                "UpdatedBy"
            },
            values: new object[,]
            {
                {
                    1, // Id
                    "Complete Python Bootcamp: Go from zero to hero in Python 3",
                    "Learn Python like a Professional! Start from the basics and go all the way to creating your own applications and games!",
                    1, // UserId
                    "/assets/images/courses/img-1.jpg",
                    4.5m, // Rating
                    1500, // RatingsCnt
                    "English",
                    2024, // LastUpdate
                    109000, // Views
                    15000, // LikesCnt
                    150, // DislikesCnt
                    5000, // SharedCnt
                    "No programming experience needed. Just a computer and internet connection!",
                    "Become a Python Programmer and learn one of employer's most requested skills!",
                    "25 hours",
                    "Development | Python",
                    10.99m, // Price
                    DateTime.UtcNow,
                    1,
                    null,
                    null
                },
                {
                    2,
                    "The Complete JavaScript Course 2024: From Zero to Expert!",
                    "The modern JavaScript course for everyone! Master JavaScript with projects, challenges and theory.",
                    2,
                    "/assets/images/courses/img-2.jpg",
                    4.8m,
                    2000,
                    "English",
                    2024,
                    150000,
                    18000,
                    200,
                    6000,
                    "No coding experience needed. Basic computer knowledge required.",
                    "Master JavaScript with the most complete course! Projects, challenges, quizzes, JavaScript ES6+, OOP, AJAX, Webpack",
                    "28 hours",
                    "Development | JavaScript",
                    12.99m,
                    DateTime.UtcNow,
                    2,
                    null,
                    null
                },
                {
                    3,
                    "Angular - The Complete Guide (2024 Edition)",
                    "Master Angular 13 and build awesome, reactive web apps with the successor of Angular.js",
                    3,
                    "/assets/images/courses/img-6.jpg",
                    4.7m,
                    1800,
                    "English",
                    2024,
                    95000,
                    12000,
                    180,
                    4500,
                    "Basic HTML, CSS and JavaScript knowledge is required",
                    "From Setup to Deployment, this course covers it all! You'll learn all about Components, Directives, Services, Forms, HTTP Access, Authentication, and more!",
                    "36 hours",
                    "Development | Angular",
                    15.99m,
                    DateTime.UtcNow,
                    3,
                    null,
                    null
                },
                {
                    4,
                    "React - The Complete Guide (incl Hooks, React Router, Redux)",
                    "Dive in and learn React.js from scratch! Learn React, Hooks, Redux, React Router, Next.js and way more!",
                    4,
                    "/assets/images/courses/img-4.jpg",
                    4.9m,
                    2200,
                    "English",
                    2024,
                    180000,
                    20000,
                    250,
                    7000,
                    "JavaScript + HTML + CSS fundamentals are absolutely required",
                    "This course will teach you React.js in a practice-oriented way, using all the latest patterns and best practices.",
                    "40 hours",
                    "Development | React",
                    14.99m,
                    DateTime.UtcNow,
                    4,
                    null,
                    null
                },
                {
                    5,
                    "The Complete Web Development Bootcamp",
                    "Become a Full-Stack Web Developer with just ONE course. HTML, CSS, Javascript, Node, React, MongoDB and more!",
                    5,
                    "/assets/images/courses/img-5.jpg",
                    4.6m,
                    3000,
                    "English",
                    2024,
                    200000,
                    25000,
                    300,
                    8000,
                    "No programming experience needed - I'll teach you everything you need to know",
                    "Learn web development from the ground up with this comprehensive bootcamp course.",
                    "55 hours",
                    "Development | Full-Stack",
                    19.99m,
                    DateTime.UtcNow,
                    5,
                    null,
                    null
                },
                {
                    6,
                    "Machine Learning A-Z™: Hands-On Python & R In Data Science",
                    "Learn to create Machine Learning Algorithms in Python and R from two Data Science experts.",
                    6,
                    "/assets/images/courses/img-8.jpg",
                    4.5m,
                    2500,
                    "English",
                    2024,
                    160000,
                    19000,
                    220,
                    6500,
                    "Just some high school mathematics level.",
                    "Master Machine Learning on Python & R. Have a great intuition of many Machine Learning models.",
                    "44 hours",
                    "Data Science | Machine Learning",
                    17.99m,
                    DateTime.UtcNow,
                    6,
                    null,
                    null
                },
                {
                    7,
                    "iOS & Swift - The Complete iOS App Development Bootcamp",
                    "From Beginner to iOS App Developer with Just One Course! Fully Updated with a Comprehensive Module Dedicated to SwiftUI!",
                    7,
                    "/assets/images/courses/img-7.jpg",
                    4.8m,
                    1900,
                    "English",
                    2024,
                    130000,
                    16000,
                    190,
                    5500,
                    "No programming experience needed - I'll teach you everything you need to know",
                    "Learn iOS app development from the ground up with the latest iOS 13 and Swift 5.1 technologies.",
                    "48 hours",
                    "Development | iOS",
                    16.99m,
                    DateTime.UtcNow,
                    7,
                    null,
                    null
                },
                {
                    8,
                    "The Complete Digital Marketing Course - 12 Courses in 1",
                    "Master Digital Marketing Strategy, Social Media Marketing, SEO, YouTube, Email, Facebook Marketing, Analytics & More!",
                    8,
                    "/assets/images/courses/img-3.jpg",
                    4.4m,
                    2800,
                    "English",
                    2024,
                    175000,
                    21000,
                    280,
                    7500,
                    "No prior knowledge required. Just a willingness to learn.",
                    "Learn digital marketing from scratch with this comprehensive course covering all major platforms.",
                    "38 hours",
                    "Marketing | Digital Marketing",
                    13.99m,
                    DateTime.UtcNow,
                    8,
                    null,
                    null
                },
                {
                    9,
                    "Advanced CSS and Sass: Flexbox, Grid, Animations",
                    "The most advanced and modern CSS course on the internet: master flexbox, CSS Grid, responsive design, and so much more.",
                    9,
                    "/assets/images/courses/img-12.jpg",
                    4.7m,
                    1600,
                    "English",
                    2024,
                    120000,
                    15000,
                    170,
                    5000,
                    "Basic HTML and CSS knowledge",
                    "Learn modern CSS from the beginning, get familiar with CSS Grid and Flexbox, and write better, more efficient CSS.",
                    "28 hours",
                    "Development | CSS",
                    11.99m,
                    DateTime.UtcNow,
                    9,
                    null,
                    null
                },
                {
                    10,
                    "Docker & Kubernetes: The Practical Guide",
                    "Learn Docker, Docker Compose, Multi-Container Projects, Deployment and all about Kubernetes from the ground up!",
                    10,
                    "/assets/images/courses/img-13.jpg",
                    4.6m,
                    1700,
                    "English",
                    2024,
                    140000,
                    17000,
                    200,
                    5800,
                    "Basic JavaScript knowledge is required",
                    "Learn Docker, Docker Compose and Kubernetes from the ground up with this practical guide to containerization.",
                    "32 hours",
                    "Development | DevOps",
                    15.99m,
                    DateTime.UtcNow,
                    10,
                    null,
                    null
                }
            });
            
            migrationBuilder.CreateIndex(
                name: "IX_Courses_UserId",
                table: "Courses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Users_UserId",
                table: "Courses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
