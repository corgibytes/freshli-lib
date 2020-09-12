using Microsoft.EntityFrameworkCore.Migrations;

namespace Freshli.Web.Migrations
{
    public partial class AddAnalysisRequestState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "analysis_requests",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "state",
                table: "analysis_requests");
        }
    }
}
