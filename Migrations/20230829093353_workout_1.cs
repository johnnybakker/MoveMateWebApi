using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoveMateWebApi.Migrations
{
    /// <inheritdoc />
    public partial class workout_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_WorkoutTypes_TypeEntityId",
                table: "Workouts");

            migrationBuilder.RenameColumn(
                name: "TypeEntityId",
                table: "Workouts",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Workouts_TypeEntityId",
                table: "Workouts",
                newName: "IX_Workouts_TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_WorkoutTypes_TypeId",
                table: "Workouts",
                column: "TypeId",
                principalTable: "WorkoutTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_WorkoutTypes_TypeId",
                table: "Workouts");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Workouts",
                newName: "TypeEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Workouts_TypeId",
                table: "Workouts",
                newName: "IX_Workouts_TypeEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_WorkoutTypes_TypeEntityId",
                table: "Workouts",
                column: "TypeEntityId",
                principalTable: "WorkoutTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
