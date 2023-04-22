using DataLayer.Entities;
using DataLayer.IRepos;
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
        public static void AddWordsToDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var kanjiRepo = serviceProvider.GetRequiredService<IKanjiNoteCardRepo>();
                var categoryRepo = serviceProvider.GetRequiredService<ICategoryRepo>();
            }
        }

        public static void GetJapaneseWordNoteCardFromFile()
        {
            JapaneseWordNoteCard japaneseWord = new JapaneseWordNoteCard();
            japaneseWord.SentenceNoteCard = new SentenceNoteCard();
            japaneseWord.SentenceNoteCard.Chapters = new List<ChapterNoteCard>();

            string pathToTestFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\zzNihongoDb\一 - Jisho.org.htm";

            var doc = new HtmlDocument();
            doc.Load(pathToTestFile);

            var mainResults = doc.GetElementbyId("main_results");
            var kanjiDiv = doc.GetElementbyId("secondary");

            var kanjiFromPage = GetKanjiFromPage(kanjiDiv);

            GetAllInfo(mainResults);
            
        }

        private static string GetKanjiFromPage(HtmlNode startDiv)
        {
            const int NUMBER_OF_KANJI_LOOK_FOR = 1;

            string resultCount = startDiv.SelectSingleNode(".//h4/span").InnerText;
            
            var theNumberInArray = resultCount.Where(c => Char.IsDigit(c)).ToArray();
            var numberString = new String(theNumberInArray);
            int result = -1;
            Int32.TryParse(numberString, out result);

            if (result == 1)
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

        private static void GetAllInfo(HtmlNode startDiv)
        {
            List<JapaneseWordNoteCard> japaneseWordNoteCards = new List<JapaneseWordNoteCard>();

            var primaryDiv = startDiv.Descendants().First(desc => desc.Id == "primary");

            var wordDivs = primaryDiv.Descendants().Where(desc => 
            {
                var classes = desc.GetClasses();
                return classes.Contains("concept_light") && classes.Contains("clearfix");
            }
            );
            foreach ( var wordDiv in wordDivs)
            {
                var japanNoteCard = new JapaneseWordNoteCard();
                japanNoteCard.SentenceNoteCard = new SentenceNoteCard();
                japanNoteCard.SentenceNoteCard.Chapters = new List<ChapterNoteCard>();


                var hintSpan = wordDiv.SelectNodes(".//span").First(node => node.GetClasses().Contains("furigana"));
                if (hintSpan != null)
                {
                    var hint = hintSpan.InnerText.Trim();
                    Console.WriteLine(hint);
                    japanNoteCard.SentenceNoteCard.Hint = hint;
                }
                

                var word = wordDiv.SelectNodes(".//span").First(node => node.GetClasses().Contains("text")).InnerText.Trim();
                Console.WriteLine(word);
                japanNoteCard.SentenceNoteCard.ItemQuestion = word;



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
                    foreach(var define in definationDivs)
                    {
                        strings.Add(define.InnerText);
                    }

                    var allDefineInOneString = String.Join(" -||- ", strings);
                    Console.WriteLine(allDefineInOneString);
                    japanNoteCard.SentenceNoteCard.ItemAnswer = allDefineInOneString;
                }

                japaneseWordNoteCards.Add(japanNoteCard);
            }
        }
    }
}
