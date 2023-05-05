namespace DataLayer.Entities
{
    public class ChapterNoteCardSentenceNoteCard
    {
        public string ChapterNoteCardTopicName { get; set; }
        public string SentenceNoteCardItemQuestion { get; set; }

        public ExtraJishoInfoOnBridge? ExtraJishoInfo { get; set; }
    }
}
