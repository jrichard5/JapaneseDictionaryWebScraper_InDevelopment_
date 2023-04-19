namespace DataLayer.Entities
{
    public class SentenceNoteCard
    {
        public string ItemQuestion { get; set; }
        public string ItemAnswer { get; set; }
        public string Hint { get; set; }
        public List<ChapterNoteCard> Chapters { get; set; }
        public List<ChapterNoteCardSentenceNoteCard> ChapterSentences { get; set; }

        public int MemorizationLevel { get; set; }
        public bool IsUserWantsToFocusOn { get; set; }
        public DateTime LastTimeAccess { get; set; }

        //Ok there is like 1300 words that contain the kanji 日

    }
}
