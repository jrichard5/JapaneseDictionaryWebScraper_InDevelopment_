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
        public async static Task AddWordsToDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var kanjiRepo = serviceProvider.GetRequiredService<IChapterNoteCardRepo>();
                var jwncRepo = serviceProvider.GetRequiredService<IJapaneseWordNoteCardRepo>();

                var notecardList = await GetJapaneseWordNoteCardFromFile(kanjiRepo);
                await jwncRepo.AddAsync(notecardList);
            }
        }

        public async static Task<List<JapaneseWordNoteCard>> GetJapaneseWordNoteCardFromFile(IChapterNoteCardRepo chapterRepository)
        {
            List<JapaneseWordNoteCard> notecards = new List<JapaneseWordNoteCard>();

            string pathToTestFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\zzNihongoDb\一 - Jisho.org.htm";

            var doc = new HtmlDocument();
            doc.Load(pathToTestFile);

            var mainResults = doc.GetElementbyId("main_results");
            var kanjiDiv = doc.GetElementbyId("secondary");

            var kanjiFromPage = GetKanjiFromPage(kanjiDiv);
            var kanjiNoteCard = await chapterRepository.GetChapterNoteCardByTopicName(kanjiFromPage);

            //AddRange will change the list that called it
            notecards.AddRange(GetAllInfo(mainResults, kanjiNoteCard));

            var moreWordsNode = mainResults.SelectNodes(".//a").First(node => node.GetClasses().Contains("more"));
            var moreWordsLink = "";
            if (moreWordsNode != null)
            {
                moreWordsLink = moreWordsNode.GetAttributeValue("href", "");
            }
            
            //Only call 5 pages max
            const int LINK_LIMIT = 5;
            int currentLinkCount = 1;
            while (moreWordsLink != "" && currentLinkCount < LINK_LIMIT)
            {
                Console.WriteLine(moreWordsLink);
                //moreWordsLink = "";



                //Wait time between each call
                await Task.Delay(6000);
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
                Console.WriteLine(theKanji + "...though I expect it to be ?....because console doesn't have font");
                return theKanji;
            }
            else
            {
                Console.WriteLine("Debug.WriteLine(\"this page doesn't have only one kanji\")");
                throw new Exception();
            }
        }

        private static List<JapaneseWordNoteCard> GetAllInfo(HtmlNode startDiv, ChapterNoteCard kanji)
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
                Console.WriteLine(word);
                japanNoteCard.SentenceNoteCard.ItemQuestion = word;


                var hintSpan = wordDiv.SelectNodes(".//span").First(node => node.GetClasses().Contains("furigana"));
                if (hintSpan != null)
                {
                    var hint = hintSpan.InnerText.Trim();
                    Console.WriteLine(hint);
                    japanNoteCard.SentenceNoteCard.Hint = hint;
                }






                var isCommon = wordDiv.SelectNodes(".//span").Any(node =>
                node.GetClasses().Contains("concept_light-common")
                && node.GetClasses().Contains("success"));
                Console.WriteLine(isCommon);
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
                    Console.WriteLine(jlptLevel);
                    Int32.TryParse(jlptLevel.Replace("JLPT N", ""), out result);
                    if (result != -1)
                    {
                        Console.WriteLine($"jltp - {result}");
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
                    Console.WriteLine(allDefineInOneString);
                    japanNoteCard.SentenceNoteCard.ItemAnswer = allDefineInOneString;
                }

                japaneseWordNoteCards.Add(japanNoteCard);
            }
            return japaneseWordNoteCards;
        }
    }
}
