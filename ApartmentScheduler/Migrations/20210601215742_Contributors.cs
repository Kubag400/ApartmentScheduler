using Microsoft.EntityFrameworkCore.Migrations;

namespace ApartmentScheduler.Migrations
{
    public partial class Contributors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributors_Apartments_ApartmentId",
                table: "Contributors");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributors_Apartments_ApartmentId",
                table: "Contributors",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributors_Apartments_ApartmentId",
                table: "Contributors");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributors_Apartments_ApartmentId",
                table: "Contributors",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
