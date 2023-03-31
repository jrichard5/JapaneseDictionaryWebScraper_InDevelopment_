using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class KanjiNoteCard
    {
        public string TopicName { get; set; }
        public ChapterNoteCard ChapterNoteCard { get; set; }
        public int NewspaperRank { get; set; }
        public int JLPTLevel { get; set; }
    }

    
}
