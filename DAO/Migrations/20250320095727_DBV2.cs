using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class DBV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Reviews_OrderDetailId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_BlindBoxId",
                table: "OrderDetail");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrderDetailId",
                table: "Reviews",
                column: "OrderDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_BlindBoxId",
                table: "OrderDetail",
                column: "BlindBoxId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_OrderDetail_OrderDetailId",
                table: "Reviews",
                column: "OrderDetailId",
                principalTable: "OrderDetail",
                principalColumn: "OrderDetailId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_OrderDetail_OrderDetailId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_OrderDetailId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_BlindBoxId",
                table: "OrderDetail");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_BlindBoxId",
                table: "OrderDetail",
                column: "BlindBoxId",
                unique: true,
                filter: "[BlindBoxId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Reviews_OrderDetailId",
                table: "OrderDetail",
                column: "OrderDetailId",
                principalTable: "Reviews",
                principalColumn: "ReviewId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
