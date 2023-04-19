using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class implosion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExtraJishoInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChapterNoteCardTopicName = table.Column<string>(type: "TEXT", nullable: false),
                    SentenceNoteCardItemQuestion = table.Column<string>(type: "TEXT", nullable: false),
                    PageNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraJishoInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtraJishoInfos_ChapterSentences_ChapterNoteCardTopicName_SentenceNoteCardItemQuestion",
                        columns: x => new { x.ChapterNoteCardTopicName, x.SentenceNoteCardItemQuestion },
                        principalTable: "ChapterSentences",
                        principalColumns: new[] { "ChapterNoteCardTopicName", "SentenceNoteCardItemQuestion" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JapaneseWordNoteCards",
                columns: table => new
                {
                    ItemQuestion = table.Column<string>(type: "TEXT", nullable: false),
                    IsCommonWord = table.Column<bool>(type: "INTEGER", nullable: false),
                    JLPTLevel = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JapaneseWordNoteCards", x => x.ItemQuestion);
                    table.ForeignKey(
                        name: "FK_JapaneseWordNoteCards_Sentences_ItemQuestion",
                        column: x => x.ItemQuestion,
                        principalTable: "Sentences",
                        principalColumn: "ItemQuestion",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "ExtraJishoInfos",
                columns: new[] { "Id", "ChapterNoteCardTopicName", "Order", "PageNumber", "SentenceNoteCardItemQuestion" },
                values: new object[,]
                {
                    { 1, "毎", 1, 1, "毎日" },
                    { 2, "日", 1, 1, "毎日" }
                });

            migrationBuilder.InsertData(
                table: "JapaneseWordNoteCards",
                columns: new[] { "ItemQuestion", "IsCommonWord", "JLPTLevel" },
                values: new object[] { "毎日", true, 5 });

            migrationBuilder.UpdateData(
                table: "Sentences",
                keyColumn: "ItemQuestion",
                keyValue: "毎日",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 18, 19, 30, 58, 135, DateTimeKind.Local).AddTicks(6693));

            migrationBuilder.CreateIndex(
                name: "IX_ExtraJishoInfos_ChapterNoteCardTopicName_SentenceNoteCardItemQuestion",
                table: "ExtraJishoInfos",
                columns: new[] { "ChapterNoteCardTopicName", "SentenceNoteCardItemQuestion" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtraJishoInfos");

            migrationBuilder.DropTable(
                name: "JapaneseWordNoteCards");

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "TopicName",
                keyValue: "日",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 16, 23, 35, 55, 375, DateTimeKind.Local).AddTicks(3188));

            migrationBuilder.UpdateData(
                table: "Chapters",
                keyColumn: "TopicName",
                keyValue: "毎",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 16, 23, 35, 55, 375, DateTimeKind.Local).AddTicks(3217));

            migrationBuilder.UpdateData(
                table: "Sentences",
                keyColumn: "ItemQuestion",
                keyValue: "毎日",
                column: "LastTimeAccess",
                value: new DateTime(2023, 4, 16, 23, 35, 55, 375, DateTimeKind.Local).AddTicks(3248));
        }
    }
}
