using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialIoTModel7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    DeviceEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AutoCloseEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AutoCloseDelay = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoCloseSettings", x => x.DeviceEntityId);
                    table.ForeignKey(
                        name: "FK_AutoCloseSettings_Devices_DeviceEntityId",
                        column: x => x.DeviceEntityId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
