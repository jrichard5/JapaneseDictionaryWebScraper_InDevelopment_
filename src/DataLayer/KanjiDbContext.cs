using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DataLayer
{
    public class KanjiDbContext : DbContext
    {

        //private string DbPath { get; }
        public KanjiDbContext()
        {
            /*
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var newAppDataFolder = Path.Join(path, @"zzNihongoDb\");
            Directory.CreateDirectory(newAppDataFolder);
            DbPath = System.IO.Path.Combine(newAppDataFolder, "kdb.db");
           // 
            */
        }

        public KanjiDbContext(DbContextOptions<KanjiDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*   Dont need anymore since I am using DI
            Console.WriteLine("Using databasefile " +  DbPath);
            optionsBuilder.UseSqlite($"Data Source={DbPath}");

            */
            //TODO: do I need this??
            //base.OnConfiguring(optionsBuilder);
        }

        //https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli --uses =>
        //=> optionsBuilder.UseSqlite("Data Source=KanjiDatabasePrototype");

        public override void Dispose()
        {
            Console.WriteLine("context is being disposed of.....in 3 .... 2 .... 1.............");
            Debug.WriteLine("The Context has been disposed of.....I think....");
            base.Dispose();
            Debug.WriteLine("The Context has been disposed of.....I think....");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChapterNoteCard>().HasKey(c => c.TopicName);
            modelBuilder.Entity<SentenceNoteCard>().HasKey(s => s.ItemQuestion);
            modelBuilder.Entity<KanjiReading>().HasKey(kr => new { kr.ChapterNoteCardTopicName, kr.TypeOfReading, kr.Reading });
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

            modelBuilder.Entity<ChapterNoteCard>().HasData(
                    new ChapterNoteCard { TopicName = "日", TopicDefinition = "day, sun, Japan", GradeLevel = 1 },
                    new ChapterNoteCard { TopicName = "毎", TopicDefinition = "every", GradeLevel = 2  }
                );
            modelBuilder.Entity<KanjiNoteCard>().HasData(
                new KanjiNoteCard { TopicName = "日", JLPTLevel = 5, NewspaperRank = 1 }
                );
            modelBuilder.Entity<KanjiReading>().HasData(
                new KanjiReading { ChapterNoteCardTopicName = "日", TypeOfReading = "kun", Reading = "ひ" },　　
                new KanjiReading { ChapterNoteCardTopicName = "日", TypeOfReading = "kun", Reading = "び" },
                new KanjiReading { ChapterNoteCardTopicName = "日", TypeOfReading = "kun", Reading = "か" }
                );

            modelBuilder.Entity<ChapterNoteCardSentenceNoteCard>().HasData(
                new ChapterNoteCardSentenceNoteCard { ChapterNoteCardTopicName = "毎", SentenceNoteCardItemQuestion = "毎日" },
                new ChapterNoteCardSentenceNoteCard { ChapterNoteCardTopicName = "日", SentenceNoteCardItemQuestion = "毎日" }
                );

            modelBuilder.Entity<SentenceNoteCard>().HasData(
                new SentenceNoteCard
                {
                    ItemQuestion = "毎日",
                    ItemAnswer = "every day​",
                    Hint = "まい·にち",
                    IsUserWantsToFocusOn = false,
                    MemorizationLevel = 0,
                    LastTimeAccess = DateTime.Now
                }
                ) ;

            //base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<ChapterNoteCard> Chapters { get; set; }
        public DbSet<SentenceNoteCard> Sentences { get; set; }
        public virtual DbSet<KanjiNoteCard> ExtraKanjiInfos { get; set; }
        public virtual DbSet<KanjiReading> KanjiReadings { get; set; }
        public virtual DbSet<ChapterNoteCardSentenceNoteCard> ChapterSentences { get; set; }    
    }
}
