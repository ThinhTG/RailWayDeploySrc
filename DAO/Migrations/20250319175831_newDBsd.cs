using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class newDBsd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_AspNetUsers_AccountId",
                table: "Wallet");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_AspNetUsers_ApplicationUserId",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_AccountId",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_ApplicationUserId",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Wallet");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Wallet",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "TypeSell",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "WalletId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WalletId",
                table: "AspNetUsers",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Wallet_WalletId",
                table: "AspNetUsers",
                column: "WalletId",
                principalTable: "Wallet",
                principalColumn: "WalletId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Wallet_WalletId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WalletId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TypeSell",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Wallet",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Wallet",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_AccountId",
                table: "Wallet",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_ApplicationUserId",
                table: "Wallet",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_AspNetUsers_AccountId",
                table: "Wallet",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_AspNetUsers_ApplicationUserId",
                table: "Wallet",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
