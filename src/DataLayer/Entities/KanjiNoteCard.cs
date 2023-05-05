namespace DataLayer.Entities
{
    public class KanjiNoteCard
    {
        public string TopicName { get; set; }
        public ChapterNoteCard ChapterNoteCard { get; set; }
        public int NewspaperRank { get; set; }
        public int JLPTLevel { get; set; }
        public List<KanjiReading> KanjiReadings { get; set; }
    }
}
