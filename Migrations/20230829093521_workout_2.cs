using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoveMateWebApi.Migrations
{
    /// <inheritdoc />
    public partial class workout_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutDatas_Workouts_WorkoutId",
                table: "WorkoutDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutDatas",
                table: "WorkoutDatas");

            migrationBuilder.RenameTable(
                name: "WorkoutDatas",
                newName: "WorkoutData");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutDatas_WorkoutId",
                table: "WorkoutData",
                newName: "IX_WorkoutData_WorkoutId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutData",
                table: "WorkoutData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutData_Workouts_WorkoutId",
                table: "WorkoutData",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutData_Workouts_WorkoutId",
                table: "WorkoutData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutData",
                table: "WorkoutData");

            migrationBuilder.RenameTable(
                name: "WorkoutData",
                newName: "WorkoutDatas");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutData_WorkoutId",
                table: "WorkoutDatas",
                newName: "IX_WorkoutDatas_WorkoutId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutDatas",
                table: "WorkoutDatas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutDatas_Workouts_WorkoutId",
                table: "WorkoutDatas",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
