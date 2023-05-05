using DataLayer.Entities;
using HtmlAgilityPack;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraper.ParseHTML;
using WebScraper.ParseHTML.ParsingWords;

namespace xUnitTests.ParseWordsTests
{
    public class HintTests
    {

        [Fact]
        public async Task HintTest_HasBlankHintSpan_Success()
        {
            //I think these should use Mocks, but the function does use the node....

            var testDiv = "<div class=\"concept_light clearfix\">  <div class=\"concept_light-wrapper  columns zero-padding\">    <div class=\"concept_light-readings japanese japanese_gothic\" lang=\"ja\"><div class=\"concept_light-representation\">      <span class=\"furigana\"><span class=\"kanji-2-up kanji\">おな</span><span></span><span class=\"kanji-2-up kanji\">どし</span></span><span class=\"text\">同<span>い</span>年</span></div>    </div>      <div class=\"concept_light-status\"><span class=\"concept_light-tag label\">JLPT N1</span>  <a class=\"concept_light-status_link\" data-dropdown=\"links_drop_51859c03d5dda729540151f0\" data-options=\"is_hover:true; hover_timeout:300\" href=\"#\">Links</a><ul class=\"f-dropdown\" id=\"links_drop_51859c03d5dda729540151f0\" data-dropdown-content=\"data-dropdown-content\"><li><a href=\"/search/%E5%90%8C%E3%81%84%E5%B9%B4%20%23sentences\">Sentence search for 同い年</a></li><li><a href=\"/search/%E3%81%8A%E3%81%AA%E3%81%84%E3%81%A9%E3%81%97%20%23sentences\">Sentence search for おないどし</a></li><li><a href=\"//jisho.org/search/%E5%90%8C%E5%B9%B4%20%23kanji\">Kanji details for 同 and 年</a></li><li><a href=\"http://www.edrdg.org/jmdictdb/cgi-bin/edform.py?svc=jmdict&amp;sid=&amp;q=1451740&amp;a=2\">Edit in JMdict</a></li></ul>      </div>  </div>  <div class=\"concept_light-meanings medium-9 columns\">    <div class='meanings-wrapper'><div class=\"meaning-tags\">Noun, Noun which may take the genitive case particle &#39;no&#39;</div><div class=\"meaning-wrapper\"><div class=\"meaning-definition zero-padding\"><span class=\"meaning-definition-section_divider\">1. </span><span class=\"meaning-meaning\">the same age</span><span>&#8203;</span></div></div></div>  </div>    <a class=\"light-details_link\" href=\"//jisho.org/word/%E5%90%8C%E3%81%84%E5%B9%B4\">Details ▸</a></div>";

            var realHtmlDoc = new HtmlDocument();
            realHtmlDoc.LoadHtml(testDiv);

            HtmlNode wordDiv = realHtmlDoc.DocumentNode;
            Console.WriteLine();

            var parseWords = new JapanWordInfoFromDiv(wordDiv);


            Assert.Equal(@"[おな] い [どし]", parseWords.Hint);
            Assert.Equal("同い年", parseWords.Word);
            Assert.False(parseWords.IsCommon);
            Assert.Equal(1, parseWords.JlptLevel);
            Assert.Equal("the same age", parseWords.Defination);
        }
        



    }
}
