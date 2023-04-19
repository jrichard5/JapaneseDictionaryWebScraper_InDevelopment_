using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class ExtraJishoInfoOnBridge
    {
        public int Id { get; set; }
        public string ChapterNoteCardTopicName { get; set; }
        public string SentenceNoteCardItemQuestion { get; set; }
        public ChapterNoteCardSentenceNoteCard ChapterNoteCardSentenceNoteCard { get; set; }
        public int PageNumber { get; set; }
        public int Order { get; set; }
    }
}
