using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class switchfromintIDtoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                schema: "ath",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "ath",
                table: "Users",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ReceiverId",
                schema: "com",
                table: "RequestedFriendships",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "SenderId",
                schema: "com",
                table: "RequestedFriendships",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "SenderId",
                schema: "com",
                table: "Messages",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiverId",
                schema: "com",
                table: "Messages",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "FriendId",
                schema: "com",
                table: "Friends",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                schema: "com",
                table: "Friends",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                schema: "ath",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "ath",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ReceiverId",
                schema: "com",
                table: "RequestedFriendships",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "SenderId",
                schema: "com",
                table: "RequestedFriendships",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "SenderId",
                schema: "com",
                table: "Messages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiverId",
                schema: "com",
                table: "Messages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "FriendId",
                schema: "com",
                table: "Friends",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                schema: "com",
                table: "Friends",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "uuid");
        }
    }
}
