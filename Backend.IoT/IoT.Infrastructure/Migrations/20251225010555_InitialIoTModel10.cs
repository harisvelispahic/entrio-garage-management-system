using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialIoTModel10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "TimeOfDay",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "DaysOfWeekMask",
                table: "Schedules",
                newName: "CommandType");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExecuteAtUtc",
                table: "Schedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TargetPercentage",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WasTriggered",
                table: "Schedules",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecuteAtUtc",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "TargetPercentage",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "WasTriggered",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "CommandType",
                table: "Schedules",
                newName: "DaysOfWeekMask");

            migrationBuilder.AddColumn<int>(
                name: "Action",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeOfDay",
                table: "Schedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
