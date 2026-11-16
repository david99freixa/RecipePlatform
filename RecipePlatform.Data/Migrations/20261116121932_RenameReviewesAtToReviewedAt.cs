using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipePlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameReviewesAtToReviewedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReviewesAt",
                table: "Recipes",
                newName: "ReviewedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReviewedAt",
                table: "Recipes",
                newName: "ReviewesAt");
        }
    }
}
