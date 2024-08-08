using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZoidUpAPI.Migrations
{
    /// <inheritdoc />
    public partial class Removeduselessentitiesandfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connections",
                schema: "ath");

            migrationBuilder.RenameColumn(
                name: "ChatID",
                schema: "ath",
                table: "Users",
                newName: "Token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                schema: "ath",
                table: "Users",
                newName: "ChatID");

            migrationBuilder.CreateTable(
                name: "Connections",
                schema: "ath",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Connected = table.Column<bool>(type: "boolean", nullable: false),
                    UserAgent = table.Column<string>(type: "text", nullable: false),
                    UserID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Connections_Users_UserID",
                        column: x => x.UserID,
                        principalSchema: "ath",
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connections_UserID",
                schema: "ath",
                table: "Connections",
                column: "UserID");
        }
    }
}
