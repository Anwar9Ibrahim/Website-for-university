using Microsoft.EntityFrameworkCore.Migrations;

namespace GroupC.Uni.Infrastructure.Migrations
{
    public partial class addMArkToQuestionExam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Mark",
                table: "ExamQuestions",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mark",
                table: "ExamQuestions");
        }
    }
}
