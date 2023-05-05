using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraper.ParseHTML.IParsingWords;

namespace WebScraper.ParseHTML.ParsingWords
{
    public class InfoMapper : IInfoMapper
    {
        public JapaneseWordNoteCard ToJapanNoteCard(JapanWordInfoFromDiv info, ChapterNoteCard kanji)
        {
            if(info.Word == null)
            {
                throw new ArgumentException("info needs word");
            }
            var japanNoteCard = new JapaneseWordNoteCard(kanji)
            {
                IsCommonWord = info.IsCommon,
                JLPTLevel = info.JlptLevel
            };
            japanNoteCard.SentenceNoteCard.ItemQuestion = info.Word;
            japanNoteCard.SentenceNoteCard.ItemAnswer = info.Defination;
            japanNoteCard.SentenceNoteCard.Hint = info.Hint;

            return japanNoteCard;
        }
    }
}
