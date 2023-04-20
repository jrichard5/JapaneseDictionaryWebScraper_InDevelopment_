using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class minorChangesToKanjiReadings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChapterNoteCardTopicName",
                table: "KanjiReadings",
                newName: "KanjiNoteCardTopicName");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastTimeAccess",
                table: "Chapters",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

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
                name: "FK_KanjiReadings_ExtraKanjiInfos_KanjiNoteCardTopicName",
                table: "KanjiReadings",
                column: "KanjiNoteCardTopicName",
                principalTable: "ExtraKanjiInfos",
                principalColumn: "TopicName",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanjiReadings_ExtraKanjiInfos_KanjiNoteCardTopicName",
                table: "KanjiReadings");

            migrationBuilder.RenameColumn(
                name: "KanjiNoteCardTopicName",
                table: "KanjiReadings",
                newName: "ChapterNoteCardTopicName");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastTimeAccess",
                table: "Chapters",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "TopicName",
                keyValue: "日",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 18, 19, 30, 58, 135, DateTimeKind.Local).AddTicks(6621));

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "TopicName",
                keyValue: "毎",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 18, 19, 30, 58, 135, DateTimeKind.Local).AddTicks(6648));

            migrationBuilder.UpdateData(
                table: "Sentences",
                keyColumn: "ItemQuestion",
                keyValue: "毎日",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 18, 19, 30, 58, 135, DateTimeKind.Local).AddTicks(6693));
        }
    }
}
