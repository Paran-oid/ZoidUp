using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class requestedfriendshipsandschemasupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "com");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Messages",
                newSchema: "com");

            migrationBuilder.CreateTable(
                name: "RequestedFriendships",
                schema: "com",
                columns: table => new
                {
                    SenderID = table.Column<int>(type: "integer", nullable: false),
                    ReceiverID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedFriendships", x => new { x.SenderID, x.ReceiverID });
                    table.ForeignKey(
                        name: "FK_RequestedFriendships_Users_ReceiverID",
                        column: x => x.ReceiverID,
                        principalSchema: "ath",
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestedFriendships_Users_SenderID",
                        column: x => x.SenderID,
                        principalSchema: "ath",
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestedFriendships_ReceiverID",
                schema: "com",
                table: "RequestedFriendships",
                column: "ReceiverID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestedFriendships",
                schema: "com");

            migrationBuilder.RenameTable(
                name: "Messages",
                schema: "com",
                newName: "Messages");
        }
    }
}
