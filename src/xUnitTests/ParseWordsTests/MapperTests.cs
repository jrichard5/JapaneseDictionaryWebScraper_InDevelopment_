using DataLayer.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraper.ParseHTML.ParsingWords;

namespace xUnitTests.ParseWordsTests
{
    public class MapperTests
    {
        [Fact]
        public async Task InfoMapper_HintNull_Success()
        {

            var cncKanji = new ChapterNoteCard
            {
                TopicName = "百"
            };
            var testDiv = @"<div class=""concept_light clearfix"">
                          <div class=""concept_light-wrapper  columns zero-padding"">
                        <div class=""concept_light-readings japanese japanese_gothic"" lang=""ja"">
                        <div class=""concept_light-representation"">      <span>
                        <span class=""kanji-3-up kanji"">ひゃく</span>
                        </span>
                        <span class=""text"">
                        百
                        </span>
                        </div>
                        </div>
                        <div class=""concept_light-status"">
                        <span class=""concept_light-tag concept_light-common success label"">Common word</span> <span class=""concept_light-tag label"">JLPT N5</span> <span class=""concept_light-tag label""><a href='http://wanikani.com/'>Wanikani level 4</a></span>  <a class=""concept_light-status_link"" data-dropdown=""links_drop_51859c43d5dda72954016d82"" data-options=""is_hover:true; hover_timeout:300"" href=""#"">Links</a><ul class=""f-dropdown"" id=""links_drop_51859c43d5dda72954016d82"" data-dropdown-content=""data-dropdown-content""><li><a href=""/search/%E7%99%BE%20%23sentences"">Sentence search for 百</a></li><li><a href=""/search/%E3%81%B2%E3%82%83%E3%81%8F%20%23sentences"">Sentence search for ひゃく</a></li><li><a href=""/search/%EF%BC%91%EF%BC%90%EF%BC%90%20%23sentences"">Sentence search for １００</a></li><li><a href=""/search/%E9%99%8C%20%23sentences"">Sentence search for 陌</a></li><li><a href=""/search/%E4%BD%B0%20%23sentences"">Sentence search for 佰</a></li><li><a href=""/search/%E4%B8%80%E3%80%87%E3%80%87%20%23sentences"">Sentence search for 一〇〇</a></li><li><a href=""/search/%E3%81%AF%E3%81%8F%20%23sentences"">Sentence search for はく</a></li><li><a href=""//jisho.org/search/%E7%99%BE%20%23kanji"">Kanji details for 百</a></li><li><a href=""http://www.edrdg.org/jmdictdb/cgi-bin/edform.py?svc=jmdict&amp;sid=&amp;q=1488000&amp;a=2"">Edit in JMdict</a></li></ul>
                        </div>
                        </div>";

            var realHtmlDoc = new HtmlDocument();
            realHtmlDoc.LoadHtml(testDiv);
            HtmlNode wordDiv = realHtmlDoc.DocumentNode;

            var info = new JapanWordInfoFromDiv(wordDiv);

            var mapper = new InfoMapper();
            var notecard = mapper.ToJapanNoteCard(info, cncKanji);

            Assert.Equal("百", notecard.SentenceNoteCard.ItemQuestion);
            //Assert.Equal("百", notecard.ItemQuestion);
            Assert.Null(notecard.SentenceNoteCard.Hint);
            Assert.True(notecard.IsCommonWord);
            Assert.Equal(5, notecard.JLPTLevel);
            Assert.True(notecard.SentenceNoteCard.Chapters.Any());
        }

        [Fact]
        public async Task InfoMapper_AllValid_Success()
        {
            var cncKanji = new ChapterNoteCard
            {
                TopicName = "百"
            };
            var testDiv = @"<div class=""concept_light clearfix"">
                              <div class=""concept_light-wrapper  columns zero-padding"">
                                <div class=""concept_light-readings japanese japanese_gothic"" lang=""ja"">
                                  <div class=""concept_light-representation"">      <span class=""furigana"">
                                    <span class=""kanji-3-up kanji"">ひゃく</span>
                                  </span>
                                  <span class=""text"">
                                    百
                                  </span>
                            </div>
                                </div>

                                  <div class=""concept_light-status"">
                                    <span class=""concept_light-tag concept_light-common success label"">Common word</span> <span class=""concept_light-tag label"">JLPT N5</span> <span class=""concept_light-tag label""><a href='http://wanikani.com/'>Wanikani level 4</a></span>  <a class=""concept_light-status_link"" data-dropdown=""links_drop_51859c43d5dda72954016d82"" data-options=""is_hover:true; hover_timeout:300"" href=""#"">Links</a><ul class=""f-dropdown"" id=""links_drop_51859c43d5dda72954016d82"" data-dropdown-content=""data-dropdown-content""><li><a href=""/search/%E7%99%BE%20%23sentences"">Sentence search for 百</a></li><li><a href=""/search/%E3%81%B2%E3%82%83%E3%81%8F%20%23sentences"">Sentence search for ひゃく</a></li><li><a href=""/search/%EF%BC%91%EF%BC%90%EF%BC%90%20%23sentences"">Sentence search for １００</a></li><li><a href=""/search/%E9%99%8C%20%23sentences"">Sentence search for 陌</a></li><li><a href=""/search/%E4%BD%B0%20%23sentences"">Sentence search for 佰</a></li><li><a href=""/search/%E4%B8%80%E3%80%87%E3%80%87%20%23sentences"">Sentence search for 一〇〇</a></li><li><a href=""/search/%E3%81%AF%E3%81%8F%20%23sentences"">Sentence search for はく</a></li><li><a href=""//jisho.org/search/%E7%99%BE%20%23kanji"">Kanji details for 百</a></li><li><a href=""http://www.edrdg.org/jmdictdb/cgi-bin/edform.py?svc=jmdict&amp;sid=&amp;q=1488000&amp;a=2"">Edit in JMdict</a></li></ul>
                                  </div>
                              </div>


                              <div class=""concept_light-meanings medium-9 columns"">
                                <div class='meanings-wrapper'><div class=""meaning-tags"">Numeric</div><div class=""meaning-wrapper""><div class=""meaning-definition zero-padding""><span class=""meaning-definition-section_divider"">1. </span><span class=""meaning-meaning"">hundred; 100</span><span>&#8203;</span><span class=""supplemental_info""><span class=""sense-tag tag-info"">陌 and 佰 are used in legal documents</span></span></div></div><div class=""meaning-tags"">Place</div><div class=""meaning-wrapper""><div class=""meaning-definition zero-padding""><span class=""meaning-definition-section_divider"">2. </span><span class=""meaning-meaning"">Hyaku</span><span>&#8203;</span></div></div><div class=""meaning-tags"">Other forms</div><div class=""meaning-wrapper""><div class=""meaning-definition zero-padding""><span class=""meaning-meaning""><span class=""break-unit"">１００ 【ひゃく】</span>、<span class=""break-unit"">陌 【ひゃく】</span>、<span class=""break-unit"">佰 【ひゃく】</span>、<span class=""break-unit"">一〇〇 【ひゃく】</span>、<span class=""break-unit"">陌 【はく】</span>、<span class=""break-unit"">佰 【はく】</span></span></div></div></div>
                              </div>


                                <a class=""light-details_link"" href=""//jisho.org/word/%E7%99%BE"">Details ▸</a>
                            </div>";

            var realHtmlDoc = new HtmlDocument();
            realHtmlDoc.LoadHtml(testDiv);
            HtmlNode wordDiv = realHtmlDoc.DocumentNode;

            var info = new JapanWordInfoFromDiv(wordDiv);

            var mapper = new InfoMapper();
            var notecard = mapper.ToJapanNoteCard(info, cncKanji);


            
            Assert.Equal("百", notecard.SentenceNoteCard.ItemQuestion);
            Assert.NotNull(notecard.SentenceNoteCard.Hint);
            Assert.Equal("[ひゃく]", notecard.SentenceNoteCard.Hint);
            Assert.True(notecard.IsCommonWord);
            Assert.Equal(5, notecard.JLPTLevel);
            Assert.Equal("hundred; 100 -||- Hyaku -||- １００ 【ひゃく】、陌 【ひゃく】、佰 【ひゃく】、一〇〇 【ひゃく】、陌 【はく】、佰 【はく】", notecard.SentenceNoteCard.ItemAnswer);
        }
    }
}
