using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class KanjiDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=KanjiDatabasePrototype.db");
            Console.WriteLine("hi");
            //TODO: do I need this??
            //base.OnConfiguring(optionsBuilder);
        }

        //https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli --uses =>
        //=> optionsBuilder.UseSqlite("Data Source=KanjiDatabasePrototype");

        public override void Dispose()
        {
            Debug.WriteLine("The Context has been disposed of.....I think....");
            base.Dispose();
            Console.WriteLine("Why not double check that it gets disposed with console?");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChapterNoteCard>().HasKey(c => c.TopicName);
            modelBuilder.Entity<SentenceNoteCard>().HasKey(s => s.ItemQuestion);
            modelBuilder.Entity<KanjiReadings>().HasKey(kr => new { kr.TopicName, kr.TypeOfReading, kr.Reading });
            modelBuilder.Entity<KanjiNoteCard>().HasKey(knc => knc.TopicName);

            modelBuilder.Entity<ChapterNoteCard>()
                .HasMany(k => k.Sentences)
                .WithMany(s => s.Chapters)
                .UsingEntity<ChapterNoteCardSentenceNoteCard>();

            modelBuilder.Entity<KanjiNoteCard>()
                .HasOne(knc => knc.ChapterNoteCard)
                .WithOne()
                .HasForeignKey<KanjiNoteCard>(knc => knc.TopicName)
                .IsRequired();
            /*
            modelBuilder.Entity<KanjiNoteCard>().HasData(
                new KanjiNoteCard {  chapterNoteCard = { TopicName = "日", TopicDefinition = "day, sun, Japan", GradeLevel = 1, IsUserWantsToFocusOn = false, MemorizationLevel = 0, LastTimeAccess = DateTime.Now } }
                );*/

            //base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<ChapterNoteCard> Chapters { get; set; }
        public virtual DbSet<SentenceNoteCard> Sentences { get; set; }
        public virtual DbSet<KanjiNoteCard> ExtraKanjiInfos { get; set; }
        public virtual DbSet<KanjiReadings> KanjiReadings { get; set; }
        public virtual DbSet<ChapterNoteCardSentenceNoteCard> ChapterSentences { get; set; }    
    }
}
