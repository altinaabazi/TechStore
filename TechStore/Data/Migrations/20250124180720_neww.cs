using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class neww : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_CountryOrders_CountryOrderId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "CountryOrderId",
                table: "Order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_CountryOrders_CountryOrderId",
                table: "Order",
                column: "CountryOrderId",
                principalTable: "CountryOrders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_CountryOrders_CountryOrderId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "CountryOrderId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_CountryOrders_CountryOrderId",
                table: "Order",
                column: "CountryOrderId",
                principalTable: "CountryOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
