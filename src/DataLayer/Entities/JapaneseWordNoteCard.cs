using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public  class JapaneseWordNoteCard
    {
        public string ItemQuestion { get; set; }
        public SentenceNoteCard SentenceNoteCard { get; set; }
        public bool IsCommonWord { get; set; }
        public int? JLPTLevel { get; set; }
    }
}
