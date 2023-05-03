using DataLayer.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebScraper.ParseHTML
{
    public class JapanWordInfoFromDiv
    {
        public string? Word { get; private set; }
        public string? Hint { get; private set; }
        public bool IsCommon { get; private set; }
        public int? JlptLevel { get; private set; }
        public string? Defination { get; private set; }

        public JapanWordInfoFromDiv(HtmlNode wordDiv)
        {
            TrySetTopicFromDiv(wordDiv);
            SetHintFromDiv(wordDiv);
            SetCommonFromDiv(wordDiv);
            SetJlptLevelFromDiv(wordDiv);
            SetDefinationsFromDiv(wordDiv);
        }

        /// <summary>
        /// Tries to get word from the word Div, if it fails, returns false.
        /// </summary>
        /// <param name="wordDiv"></param>
        /// <param name="japanNoteCard"></param>
        /// <returns>bool</returns>
        private void TrySetTopicFromDiv(HtmlNode wordDiv)
        {
            var wordFromDiv = wordDiv.SelectNodes(".//span").First(node => node.GetClasses().Contains("text")).InnerText.Trim();
            Word = wordFromDiv;
            //The website I parse had words that didn't contain this kanji >.>
            //if (word.Contains(japanNoteCard.SentenceNoteCard.Chapters.First().TopicName))
            //{
            //    japanNoteCard.SentenceNoteCard.ItemQuestion = word;
            //}
            
        }
        /*Problem :  <span class="kanji-2-up kanji">おな</span><span></span><span class="kanji-2-up kanji">どし</span> and 同<span>い</span>年
 *  Cannot associate span with kanji because sometimes unqiue readings:
 *  <span class="kanji-4-up kanji">おととし</span>  and <span class="text">一昨年</span>
 * 
 */
        private void SetHintFromDiv(HtmlNode wordDiv)
        {
            var hintSpan = wordDiv.SelectNodes(".//span").First(node => node.GetClasses().Contains("furigana"));
            var wordNode = wordDiv.SelectNodes(".//span").First(node => node.GetClasses().Contains("text"));
            var hiraganaList = new Queue<string>();
            if (wordNode != null)
            {
                var wordChildren = wordNode.SelectNodes(".//span");
                if (wordChildren != null)
                {
                    foreach (var wordChild in wordChildren)
                    {
                        hiraganaList.Enqueue(wordChild.InnerText);
                    }
                }
            }

            if (hintSpan != null)
            {
                var hintChildrenSpans = hintSpan.SelectNodes(".//span");
                var listOfStrings = new List<string>();
                foreach (var hintChild in hintChildrenSpans)
                {
                    if (hintChild.InnerText == "")
                    {
                        if (hiraganaList.Any())
                        {
                            listOfStrings.Add(hiraganaList.Dequeue());
                        }
                        else
                        {
                            listOfStrings.Add("__");
                        }

                    }
                    else
                    {
                        listOfStrings.Add($"[{hintChild.InnerText}]".Trim());
                    }
                }
                //var hint = hintSpan.InnerText.Trim();
                Hint = String.Join(" ", listOfStrings);
                //japanNoteCard.SentenceNoteCard.Hint = String.Join(" ", listOfStrings);
            }
        }
        private void SetCommonFromDiv(HtmlNode wordDiv)
        {
            var isCommonFromNode = wordDiv.SelectNodes(".//span").Any(node =>
                node.GetClasses().Contains("concept_light-common")
                && node.GetClasses().Contains("success"));
            //japanNoteCard.IsCommonWord = isCommon;
            IsCommon = isCommonFromNode;
        }

        private void SetJlptLevelFromDiv(HtmlNode wordDiv)
        {
            var regExpression = new Regex(@"JLPT");
            var jlptLevelNode = wordDiv.SelectNodes(".//span").FirstOrDefault(node => node.GetClasses().Contains("concept_light-tag")
            && node.GetClasses().Contains("label")
            && regExpression.IsMatch(node.InnerText));

            if (jlptLevelNode != null)
            {
                var jlptLevelFromDiv = jlptLevelNode.InnerText;
                int result = -1;
                Int32.TryParse(jlptLevelFromDiv.Replace("JLPT N", ""), out result);
                if (result != -1)
                {
                    //japanNoteCard.JLPTLevel = result;
                    JlptLevel = result;
                }
            }
        }
        private void SetDefinationsFromDiv(HtmlNode wordDiv)
        {
            var definationDivs = wordDiv.SelectNodes(".//span").Where(spans => spans.GetClasses().Contains("meaning-meaning"));
            if (definationDivs.Any())
            {
                var strings = new List<string>();
                foreach (var define in definationDivs)
                {
                    strings.Add(define.InnerText);
                }

                var allDefineInOneString = String.Join(" -||- ", strings);
                //japanNoteCard.SentenceNoteCard.ItemAnswer = allDefineInOneString;
                Defination = allDefineInOneString;
            }
        }
    }
}
