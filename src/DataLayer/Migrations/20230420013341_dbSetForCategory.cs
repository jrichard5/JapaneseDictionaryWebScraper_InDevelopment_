using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class dbSetForCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Category_CategoryId",
                table: "Chapters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "TopicName",
                keyValue: "日",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 19, 20, 33, 41, 99, DateTimeKind.Local).AddTicks(8586));

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "TopicName",
                keyValue: "毎",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 19, 20, 33, 41, 99, DateTimeKind.Local).AddTicks(8613));

            migrationBuilder.UpdateData(
                table: "Sentences",
                keyColumn: "ItemQuestion",
                keyValue: "毎日",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 19, 20, 33, 41, 99, DateTimeKind.Local).AddTicks(8678));

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Categories_CategoryId",
                table: "Chapters",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Categories_CategoryId",
                table: "Chapters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "TopicName",
                keyValue: "日",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 19, 19, 32, 39, 463, DateTimeKind.Local).AddTicks(8643));

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "TopicName",
                keyValue: "毎",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 19, 19, 32, 39, 463, DateTimeKind.Local).AddTicks(8669));

            migrationBuilder.UpdateData(
                table: "Sentences",
                keyColumn: "ItemQuestion",
                keyValue: "毎日",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 19, 19, 32, 39, 463, DateTimeKind.Local).AddTicks(8711));

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Category_CategoryId",
                table: "Chapters",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
