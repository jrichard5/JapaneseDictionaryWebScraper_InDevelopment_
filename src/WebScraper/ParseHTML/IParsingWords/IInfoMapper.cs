using DataLayer.Entities;
using WebScraper.ParseHTML.ParsingWords;

namespace WebScraper.ParseHTML.IParsingWords
{
    public interface IInfoMapper
    {
        public JapaneseWordNoteCard ToJapanNoteCard(JapanWordInfoFromDiv info, ChapterNoteCard kanji);
    }
}
