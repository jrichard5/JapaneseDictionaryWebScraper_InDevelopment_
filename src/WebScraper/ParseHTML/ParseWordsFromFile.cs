using DataLayer.Entities;
using DataLayer.IRepos;
using DataLayer.Repositories;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebScraper.ParseHTML
{
    public static class ParseWordsFromFile
    {
        public async static Task AddWordsToDatabase(IHost host, string url)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var kanjiRepo = serviceProvider.GetRequiredService<IChapterNoteCardRepo>();
                var jwncRepo = serviceProvider.GetRequiredService<IJapaneseWordNoteCardRepo>();

                var notecardList = await GetJapaneseWordNoteCardFromFile(kanjiRepo, url);
                await jwncRepo.AddAsync(notecardList);
            }
        }


        private static int count = 0;
        public async static Task<List<JapaneseWordNoteCard>> GetJapaneseWordNoteCardFromFile(IChapterNoteCardRepo chapterRepository, string url)
        {
            var pageNumber = GetPageNumberFromURL(url);
            List<JapaneseWordNoteCard> notecards = new List<JapaneseWordNoteCard>();

            string pathToTestFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\zzNihongoDb\人 - Jisho.org.htm";
            string pathToTestFile2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\zzNihongoDb\人 #words - Jisho.org.htm";
            var doc = new HtmlDocument();
            doc.Load(pathToTestFile);

            var mainResults = doc.GetElementbyId("main_results");
            var kanjiDiv = doc.GetElementbyId("secondary");

            var kanjiFromPage = GetKanjiFromPage(kanjiDiv);
            var kanjiNoteCard = await chapterRepository.GetChapterNoteCardByTopicName(kanjiFromPage);

            count = await chapterRepository.GetLastItemByTopicName(kanjiFromPage);

            //AddRange will change the list that called it
            notecards.AddRange(GetAllInfo(mainResults, kanjiNoteCard, pageNumber));

            var moreWordsNode = mainResults.SelectNodes(".//a").First(node => node.GetClasses().Contains("more"));
            var moreWordsLink = "";
            if (moreWordsNode != null)
            {
                moreWordsLink = moreWordsNode.GetAttributeValue("href", "");
            }
            
            //Only call 5 pages max
            const int LINK_LIMIT = 2;
            int currentLinkCount = 1;
            while (moreWordsLink != "" && currentLinkCount < LINK_LIMIT)
            {
                pageNumber++;

                //Calling the webpage (not yet)
                Console.WriteLine("waiting 5 secs before calling" + moreWordsLink);
                await Task.Delay(2000);
                var moreDoc = new HtmlDocument();
                moreDoc.Load(pathToTestFile2);
                var moreMainResults = moreDoc.GetElementbyId("main_results");
                notecards.AddRange(GetAllInfo(moreMainResults, kanjiNoteCard, pageNumber));

                Console.WriteLine("waiting 2 secs after calling" + moreWordsLink);
                await Task.Delay(1000);
                moreWordsNode = mainResults.SelectNodes(".//a").First(node => node.GetClasses().Contains("more"));
                moreWordsLink = "";
                if (moreWordsNode != null)
                {
                    moreWordsLink = moreWordsNode.GetAttributeValue("href", "");
                }
                Console.WriteLine("Will now call" + moreWordsLink);
                moreWordsLink = "";
                //Wait time between each call
                await Task.Delay(1000);
                //ADD CURRENT COUNT TO BREAK IT OUT OF LIMIT
                currentLinkCount++;
            }

            //if it has a link, need to do all this again on same page

            return notecards;

        }

        private static string GetKanjiFromPage(HtmlNode startDiv)
        {
            const int NUMBER_OF_KANJI_LOOK_FOR = 1;

            string resultCount = startDiv.SelectSingleNode(".//h4/span").InnerText;

            var theNumberInArray = resultCount.Where(c => Char.IsDigit(c)).ToArray();
            var numberString = new String(theNumberInArray);
            int result = -1;
            Int32.TryParse(numberString, out result);

            if (result == NUMBER_OF_KANJI_LOOK_FOR)
            {
                var kanjiSpan = startDiv.Descendants().First(desc => desc.GetClasses().Contains("character"));
                var theKanji = kanjiSpan.SelectSingleNode(".//a").InnerText.Trim();
                return theKanji;
            }
            else
            {
                Console.WriteLine("Debug.WriteLine(\"this page doesn't have only one kanji\")");
                throw new Exception();
            }
        }

        private static List<JapaneseWordNoteCard> GetAllInfo(HtmlNode startDiv, ChapterNoteCard kanji, int pageNumber)
        {
            List<JapaneseWordNoteCard> japaneseWordNoteCards = new List<JapaneseWordNoteCard>();

            var primaryDiv = startDiv.Descendants().First(desc => desc.Id == "primary");

            var wordDivs = primaryDiv.Descendants().Where(desc =>
            {
                var classes = desc.GetClasses();
                return classes.Contains("concept_light") && classes.Contains("clearfix");
            }
            );
            foreach (var wordDiv in wordDivs)
            {
                var japanNoteCard = new JapaneseWordNoteCard();
                japanNoteCard.SentenceNoteCard = new SentenceNoteCard();
                japanNoteCard.SentenceNoteCard.ChapterSentences = new List<ChapterNoteCardSentenceNoteCard>();
                japanNoteCard.SentenceNoteCard.Chapters = new List<ChapterNoteCard>
                {
                    kanji
                };

                var word = wordDiv.SelectNodes(".//span").First(node => node.GetClasses().Contains("text")).InnerText.Trim();
                //The website I parse had words that didn't contain this kanji >.>
                if (!word.Contains(kanji.TopicName))
                {
                    continue;
                }
                japanNoteCard.SentenceNoteCard.ItemQuestion = word;

                japanNoteCard.SentenceNoteCard.ChapterSentences.Add(PageAndOrderNumber(pageNumber, kanji.TopicName, word));


                var hintSpan = wordDiv.SelectNodes(".//span").First(node => node.GetClasses().Contains("furigana"));
                if (hintSpan != null)
                {
                    var hint = hintSpan.InnerText.Trim();
                    japanNoteCard.SentenceNoteCard.Hint = hint;
                }


                



                var isCommon = wordDiv.SelectNodes(".//span").Any(node =>
                node.GetClasses().Contains("concept_light-common")
                && node.GetClasses().Contains("success"));
                japanNoteCard.IsCommonWord = isCommon;


                var regExpression = new Regex(@"JLPT");
                var jlptLevel = wordDiv.SelectNodes(".//span").First(node => node.GetClasses().Contains("concept_light-tag")
                && node.GetClasses().Contains("label")
                && regExpression.IsMatch(node.InnerText)
                ).InnerText;
                if (jlptLevel != null)
                {
                    int result = -1;
                    //jlptLevel = jlptLevel.Trim();
                    Int32.TryParse(jlptLevel.Replace("JLPT N", ""), out result);
                    if (result != -1)
                    {
                        japanNoteCard.JLPTLevel = result;
                    }
                }

                var definationDivs = wordDiv.SelectNodes(".//span").Where(spans => spans.GetClasses().Contains("meaning-meaning"));
                if (definationDivs.Any())
                {
                    var strings = new List<string>();
                    foreach (var define in definationDivs)
                    {
                        strings.Add(define.InnerText);
                    }

                    var allDefineInOneString = String.Join(" -||- ", strings);
                    japanNoteCard.SentenceNoteCard.ItemAnswer = allDefineInOneString;
                }

                japaneseWordNoteCards.Add(japanNoteCard);
            }
            return japaneseWordNoteCards;
        }

        private static ChapterNoteCardSentenceNoteCard PageAndOrderNumber(int pageNumber, string kanji, string word)
        {
            count++;
            return new ChapterNoteCardSentenceNoteCard { ChapterNoteCardTopicName = kanji, SentenceNoteCardItemQuestion = word, ExtraJishoInfo = new ExtraJishoInfoOnBridge { PageNumber = pageNumber, Order = count } };
            
        }
        private static int GetPageNumberFromURL(string url)
        {
            int pageNumber;
            Regex pageRegex = new Regex(@"page=[\d]+");
            var what = pageRegex.Match(url).Value.Replace("page=", "").Trim();
            if (!(Int32.TryParse(what, out pageNumber)))
            {
                pageNumber = 1;
            }
            return pageNumber;
        }
    }
}
