using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class ChapterNoteCard
    {
        public string TopicName { get; set; }
        public string TopicDefinition { get; set; }
        public int GradeLevel { get; set; }
        public List<SentenceNoteCard>? Sentences { get; set; }
        public List<ChapterNoteCardSentenceNoteCard>? ChapterSentences { get; set; }
    }
}
