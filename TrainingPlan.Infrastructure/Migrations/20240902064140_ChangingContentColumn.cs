using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingPlan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangingContentColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BlobId",
                table: "Contents",
                newName: "Data");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data",
                table: "Contents",
                newName: "BlobId");
        }
    }
}
