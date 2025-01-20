using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class orderCountryU : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryOrderId",
                table: "CountryOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountryOrders_CountryOrderId",
                table: "CountryOrders",
                column: "CountryOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CountryOrders_CountryOrders_CountryOrderId",
                table: "CountryOrders",
                column: "CountryOrderId",
                principalTable: "CountryOrders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CountryOrders_CountryOrders_CountryOrderId",
                table: "CountryOrders");

            migrationBuilder.DropIndex(
                name: "IX_CountryOrders_CountryOrderId",
                table: "CountryOrders");

            migrationBuilder.DropColumn(
                name: "CountryOrderId",
                table: "CountryOrders");
        }
    }
}
