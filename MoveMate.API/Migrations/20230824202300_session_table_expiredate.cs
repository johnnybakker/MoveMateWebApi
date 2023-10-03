using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoveMate.Migrations
{
    /// <inheritdoc />
    public partial class session_table_expiredate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expired",
                table: "Sessions");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "Sessions",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirebaseToken",
                table: "Sessions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Sessions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "FirebaseToken",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Sessions");

            migrationBuilder.AddColumn<bool>(
                name: "Expired",
                table: "Sessions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
