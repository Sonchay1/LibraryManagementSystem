using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberPhotoBytes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoData",
                table: "Members");

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoBytes",
                table: "Members",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoBytes",
                table: "Members");

            migrationBuilder.AddColumn<string>(
                name: "PhotoData",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
