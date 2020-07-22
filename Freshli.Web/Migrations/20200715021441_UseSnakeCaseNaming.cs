using Microsoft.EntityFrameworkCore.Migrations;

namespace Freshli.Web.Migrations
{
    public partial class UseSnakeCaseNaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AnalysisRequests",
                table: "AnalysisRequests");

            migrationBuilder.RenameTable(
                name: "AnalysisRequests",
                newName: "analysis_requests");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "analysis_requests",
                newName: "url");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "analysis_requests",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "analysis_requests",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "analysis_requests",
                newName: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_analysis_requests",
                table: "analysis_requests",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_analysis_requests",
                table: "analysis_requests");

            migrationBuilder.RenameTable(
                name: "analysis_requests",
                newName: "AnalysisRequests");

            migrationBuilder.RenameColumn(
                name: "url",
                table: "AnalysisRequests",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "AnalysisRequests",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "AnalysisRequests",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AnalysisRequests",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnalysisRequests",
                table: "AnalysisRequests",
                column: "Id");
        }
    }
}
