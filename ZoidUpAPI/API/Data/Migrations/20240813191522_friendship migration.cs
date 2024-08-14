using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class friendshipmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friends",
                schema: "com",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    FriendID = table.Column<int>(type: "integer", nullable: false),
                    Since = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => new { x.UserID, x.FriendID });
                    table.ForeignKey(
                        name: "FK_Friends_Users_FriendID",
                        column: x => x.FriendID,
                        principalSchema: "ath",
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friends_Users_UserID",
                        column: x => x.UserID,
                        principalSchema: "ath",
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FriendID",
                schema: "com",
                table: "Friends",
                column: "FriendID");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_UserID_FriendID",
                schema: "com",
                table: "Friends",
                columns: new[] { "UserID", "FriendID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friends",
                schema: "com");
        }
    }
}
