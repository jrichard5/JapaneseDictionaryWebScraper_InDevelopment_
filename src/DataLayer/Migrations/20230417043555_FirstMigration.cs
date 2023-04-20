using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KanjiReadings",
                columns: table => new
                {
                    ChapterNoteCardTopicName = table.Column<string>(type: "TEXT", nullable: false),
                    TypeOfReading = table.Column<string>(type: "TEXT", nullable: false),
                    Reading = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanjiReadings", x => new { x.ChapterNoteCardTopicName, x.TypeOfReading, x.Reading });
                });

            migrationBuilder.CreateTable(
                name: "Sentences",
                columns: table => new
                {
                    ItemQuestion = table.Column<string>(type: "TEXT", nullable: false),
                    ItemAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    Hint = table.Column<string>(type: "TEXT", nullable: false),
                    MemorizationLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    IsUserWantsToFocusOn = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastTimeAccess = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sentences", x => x.ItemQuestion);
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    TopicName = table.Column<string>(type: "TEXT", nullable: false),
                    TopicDefinition = table.Column<string>(type: "TEXT", nullable: false),
                    GradeLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    LastTimeAccess = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.TopicName);
                    table.ForeignKey(
                        name: "FK_Chapters_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChapterSentences",
                columns: table => new
                {
                    ChapterNoteCardTopicName = table.Column<string>(type: "TEXT", nullable: false),
                    SentenceNoteCardItemQuestion = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterSentences", x => new { x.ChapterNoteCardTopicName, x.SentenceNoteCardItemQuestion });
                    table.ForeignKey(
                        name: "FK_ChapterSentences_Chapters_ChapterNoteCardTopicName",
                        column: x => x.ChapterNoteCardTopicName,
                        principalTable: "Chapters",
                        principalColumn: "TopicName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChapterSentences_Sentences_SentenceNoteCardItemQuestion",
                        column: x => x.SentenceNoteCardItemQuestion,
                        principalTable: "Sentences",
                        principalColumn: "ItemQuestion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExtraKanjiInfos",
                columns: table => new
                {
                    TopicName = table.Column<string>(type: "TEXT", nullable: false),
                    NewspaperRank = table.Column<int>(type: "INTEGER", nullable: false),
                    JLPTLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraKanjiInfos", x => x.TopicName);
                    table.ForeignKey(
                        name: "FK_ExtraKanjiInfos_Chapters_TopicName",
                        column: x => x.TopicName,
                        principalTable: "Chapters",
                        principalColumn: "TopicName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "CategoryName" },
                values: new object[] { 1, "Japanese Vocab" });

            migrationBuilder.InsertData(
                table: "KanjiReadings",
                columns: new[] { "ChapterNoteCardTopicName", "Reading", "TypeOfReading" },
                values: new object[,]
                {
                    { "日", "か", "kun" },
                    { "日", "ひ", "kun" },
                    { "日", "び", "kun" }
                });

            migrationBuilder.InsertData(
                table: "Sentences",
                columns: new[] { "ItemQuestion", "Hint", "IsUserWantsToFocusOn", "ItemAnswer", "LastTimeAccess", "MemorizationLevel" },
                values: new object[] { "毎日", "まい·にち", false, "every day​", new DateTime(2023, 4, 16, 23, 35, 55, 375, DateTimeKind.Local).AddTicks(3248), 0 });

            migrationBuilder.InsertData(
                table: "Chapters",
                columns: new[] { "TopicName", "CategoryId", "GradeLevel", "LastTimeAccess", "TopicDefinition" },
                values: new object[,]
                {
                    { "日", 1, 1, new DateTime(2023, 4, 16, 23, 35, 55, 375, DateTimeKind.Local).AddTicks(3188), "day, sun, Japan" },
                    { "毎", 1, 2, new DateTime(2023, 4, 16, 23, 35, 55, 375, DateTimeKind.Local).AddTicks(3217), "every" }
                });

            migrationBuilder.InsertData(
                table: "ChapterSentences",
                columns: new[] { "ChapterNoteCardTopicName", "SentenceNoteCardItemQuestion" },
                values: new object[,]
                {
                    { "日", "毎日" },
                    { "毎", "毎日" }
                });

            migrationBuilder.InsertData(
                table: "ExtraKanjiInfos",
                columns: new[] { "TopicName", "JLPTLevel", "NewspaperRank" },
                values: new object[] { "日", 5, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_CategoryId",
                table: "Chapters",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterSentences_SentenceNoteCardItemQuestion",
                table: "ChapterSentences",
                column: "SentenceNoteCardItemQuestion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChapterSentences");

            migrationBuilder.DropTable(
                name: "ExtraKanjiInfos");

            migrationBuilder.DropTable(
                name: "KanjiReadings");

            migrationBuilder.DropTable(
                name: "Sentences");

            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
