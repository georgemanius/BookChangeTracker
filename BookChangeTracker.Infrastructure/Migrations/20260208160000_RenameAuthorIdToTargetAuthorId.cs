using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookChangeTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameAuthorIdToTargetAuthorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "BookChangeLogs",
                newName: "TargetAuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetAuthorId",
                table: "BookChangeLogs",
                newName: "AuthorId");
        }
    }
}
