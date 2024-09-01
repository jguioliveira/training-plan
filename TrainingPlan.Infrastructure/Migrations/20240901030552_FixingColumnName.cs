using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TrainingPlan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixingColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Person_AhtleteId",
                table: "Plans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialMedia",
                table: "SocialMedia");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SocialMedia");

            migrationBuilder.RenameColumn(
                name: "AhtleteId",
                table: "Plans",
                newName: "AthleteId");

            migrationBuilder.RenameIndex(
                name: "IX_Plans_AhtleteId",
                table: "Plans",
                newName: "IX_Plans_AthleteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialMedia",
                table: "SocialMedia",
                columns: new[] { "Name", "TeamId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Person_AthleteId",
                table: "Plans",
                column: "AthleteId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Person_AthleteId",
                table: "Plans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialMedia",
                table: "SocialMedia");

            migrationBuilder.RenameColumn(
                name: "AthleteId",
                table: "Plans",
                newName: "AhtleteId");

            migrationBuilder.RenameIndex(
                name: "IX_Plans_AthleteId",
                table: "Plans",
                newName: "IX_Plans_AhtleteId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SocialMedia",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialMedia",
                table: "SocialMedia",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Person_AhtleteId",
                table: "Plans",
                column: "AhtleteId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
