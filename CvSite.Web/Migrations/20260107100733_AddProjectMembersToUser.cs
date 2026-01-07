using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CvSite.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectMembersToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ProjectMembers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ApplicationUserId",
                table: "ProjectMembers",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_ApplicationUserId",
                table: "ProjectMembers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_ApplicationUserId",
                table: "ProjectMembers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectMembers_ApplicationUserId",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ProjectMembers");
        }
    }
}
