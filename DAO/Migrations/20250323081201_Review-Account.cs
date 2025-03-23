using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class ReviewAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "AspNetUsers");

        
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<Guid>(
                name: "ReviewId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

          
        }
    }
}
