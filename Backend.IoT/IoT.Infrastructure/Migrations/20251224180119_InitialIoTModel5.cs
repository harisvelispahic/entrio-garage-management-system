using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialIoTModel5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoCloseSettings");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AutoCloseDelay",
                table: "Devices",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "AutoCloseEnabled",
                table: "Devices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoCloseDelay",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "AutoCloseEnabled",
                table: "Devices");

            migrationBuilder.CreateTable(
                name: "AutoCloseSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DelaySeconds = table.Column<int>(type: "int", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoCloseSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutoCloseSettings_DeviceId",
                table: "AutoCloseSettings",
                column: "DeviceId",
                unique: true);
        }
    }
}
