using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraper.ParseHTML.ParsingWords;

namespace WebScraper.ParseHTML.IParsingWords
{
    public interface IInfoMapper
    {
        public JapaneseWordNoteCard ToJapanNoteCard(JapanWordInfoFromDiv info, ChapterNoteCard kanji);
    }
}
