namespace DataLayer.Entities
{
    public class ChapterNoteCard
    {
        public string TopicName { get; set; }
        public string TopicDefinition { get; set; }
        public int GradeLevel { get; set; }
        public DateTime LastTimeAccess { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<SentenceNoteCard>? Sentences { get; set; }
        public List<ChapterNoteCardSentenceNoteCard>? ChapterSentences { get; set; }
    }
}
