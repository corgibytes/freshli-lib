using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Freshli.Web.Migrations
{
    public partial class AddMetricsResultsAndRelatedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "metrics_results",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    analysis_request_id = table.Column<Guid>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    lib_year_result_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_metrics_results", x => x.id);
                    table.ForeignKey(
                        name: "fk_metrics_results_analysis_requests_analysis_request_id",
                        column: x => x.analysis_request_id,
                        principalTable: "analysis_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lib_year_results",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    metrics_result_id = table.Column<Guid>(nullable: false),
                    total = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lib_year_results", x => x.id);
                    table.ForeignKey(
                        name: "fk_lib_year_results_metrics_results_metrics_result_id1",
                        column: x => x.metrics_result_id,
                        principalTable: "metrics_results",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lib_year_package_results",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    version = table.Column<string>(nullable: true),
                    published_at = table.Column<DateTime>(nullable: false),
                    value = table.Column<double>(nullable: false),
                    lib_year_result_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lib_year_package_results", x => x.id);
                    table.ForeignKey(
                        name: "fk_lib_year_package_results_lib_year_results_lib_year_result_id",
                        column: x => x.lib_year_result_id,
                        principalTable: "lib_year_results",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_lib_year_package_results_lib_year_result_id",
                table: "lib_year_package_results",
                column: "lib_year_result_id");

            migrationBuilder.CreateIndex(
                name: "ix_lib_year_results_metrics_result_id",
                table: "lib_year_results",
                column: "metrics_result_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_metrics_results_analysis_request_id",
                table: "metrics_results",
                column: "analysis_request_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lib_year_package_results");

            migrationBuilder.DropTable(
                name: "lib_year_results");

            migrationBuilder.DropTable(
                name: "metrics_results");
        }
    }
}
