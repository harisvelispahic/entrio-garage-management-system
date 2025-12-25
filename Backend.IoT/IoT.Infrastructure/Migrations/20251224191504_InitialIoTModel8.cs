using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialIoTModel8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutoCloseSettings_Devices_DeviceEntityId",
                table: "AutoCloseSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AutoCloseSettings",
                table: "AutoCloseSettings");

            migrationBuilder.DropColumn(
                name: "AutoCloseDelay",
                table: "AutoCloseSettings");

            migrationBuilder.RenameColumn(
                name: "AutoCloseEnabled",
                table: "AutoCloseSettings",
                newName: "Enabled");

            migrationBuilder.RenameColumn(
                name: "DeviceEntityId",
                table: "AutoCloseSettings",
                newName: "DeviceId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AutoCloseSettings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "AfterSeconds",
                table: "AutoCloseSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClosePercentage",
                table: "AutoCloseSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AutoCloseSettings",
                table: "AutoCloseSettings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AutoCloseSettings_DeviceId",
                table: "AutoCloseSettings",
                column: "DeviceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoCloseSettings_Devices_DeviceId",
                table: "AutoCloseSettings",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutoCloseSettings_Devices_DeviceId",
                table: "AutoCloseSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AutoCloseSettings",
                table: "AutoCloseSettings");

            migrationBuilder.DropIndex(
                name: "IX_AutoCloseSettings_DeviceId",
                table: "AutoCloseSettings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AutoCloseSettings");

            migrationBuilder.DropColumn(
                name: "AfterSeconds",
                table: "AutoCloseSettings");

            migrationBuilder.DropColumn(
                name: "ClosePercentage",
                table: "AutoCloseSettings");

            migrationBuilder.RenameColumn(
                name: "Enabled",
                table: "AutoCloseSettings",
                newName: "AutoCloseEnabled");

            migrationBuilder.RenameColumn(
                name: "DeviceId",
                table: "AutoCloseSettings",
                newName: "DeviceEntityId");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AutoCloseDelay",
                table: "AutoCloseSettings",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AutoCloseSettings",
                table: "AutoCloseSettings",
                column: "DeviceEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AutoCloseSettings_Devices_DeviceEntityId",
                table: "AutoCloseSettings",
                column: "DeviceEntityId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
