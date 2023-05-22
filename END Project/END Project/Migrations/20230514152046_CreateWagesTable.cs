using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace END_Project.Migrations
{
    /// <inheritdoc />
    public partial class CreateWagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: DateTime.UtcNow.AddHours(4)),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Money = table.Column<float>(type: "real", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wages_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Wages_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wages_AppUserId",
                table: "Wages",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wages_EmployeeId",
                table: "Wages",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wages");
        }
    }
}
