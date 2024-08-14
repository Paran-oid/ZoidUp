using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class removeddeletebehaivoroftyperestrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_FriendID",
                schema: "com",
                table: "Friends");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                schema: "ath",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_FriendID",
                schema: "com",
                table: "Friends",
                column: "FriendID",
                principalSchema: "ath",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_FriendID",
                schema: "com",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                schema: "ath",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_FriendID",
                schema: "com",
                table: "Friends",
                column: "FriendID",
                principalSchema: "ath",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
