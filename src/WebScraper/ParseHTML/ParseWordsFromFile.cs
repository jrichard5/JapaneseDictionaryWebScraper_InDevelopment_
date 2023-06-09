﻿using DataLayer.Entities;
using DataLayer.IRepos;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;
using WebScraper.ParseHTML.ParsingWords;

namespace WebScraper.ParseHTML
{
    public class ParseWordsFromFile
    {
        public async Task AddWordsToDatabase(IHost host, string url)
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


        private int count = 0;

        public async Task<List<JapaneseWordNoteCard>> GetJapaneseWordNoteCardFromFile(IChapterNoteCardRepo chapterRepository, string url)
        {
            List<JapaneseWordNoteCard> notecards = new List<JapaneseWordNoteCard>();

            string pathToTestFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\zzNihongoDb\一 - Jisho.org.htm";
            string pathToTestFile2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\zzNihongoDb\一 - Jisho.org.htm";

            string currentUrl = pathToTestFile;
            var pageNumber = GetPageNumberFromURL(currentUrl);

            Console.WriteLine("waiting 5 secs before calling " + currentUrl);
            await Task.Delay(5000);
            var doc = new HtmlDocument();
            doc.Load(currentUrl);

            var kanjiDiv = doc.GetElementbyId("secondary");
            if (kanjiDiv == null)
            {
                throw new ArgumentException("this div needs to have a secondarydiv");
            }
            var kanjiFromPage = GetKanjiFromPage(kanjiDiv);

            //I wanted to call the first one in the while loop, but the kanji part means I made two calls to the first page.
            //TODO: Maybe I can do
            //load, get kanji.  while loop
            //get words, get next url, load html at the end.
            var kanjiNoteCard = await chapterRepository.GetChapterNoteCardByTopicName(kanjiFromPage);
            count = await chapterRepository.GetLastItemByTopicName(kanjiFromPage);

            var nextUrl = AddWordUsingDoc(doc, notecards, kanjiNoteCard, pageNumber);

            //var mainResults = doc.GetElementbyId("main_results");
            //var moreWordsNode = mainResults.SelectNodes(".//a").First(node => node.GetClasses().Contains("more"));
            if (nextUrl != "")
            {
                //currentUrl = moreWordsNode.GetAttributeValue("href", "");
                //TODO: This is here for test purposes (not making calls to jisho until tested);
                nextUrl = pathToTestFile2;
                if (currentUrl != nextUrl)
                {
                    currentUrl = nextUrl;
                }
                else
                {
                    currentUrl = "";
                }
            }

            //Only call 5 pages max
            const int LINK_LIMIT = 5;
            int currentLinkCount = 1;
            while (currentUrl != "" && currentLinkCount < LINK_LIMIT)
            {
                pageNumber++;
                Console.WriteLine("waiting 10 secs before calling" + currentUrl);
                await Task.Delay(10000);

                doc = new HtmlDocument();
                doc.Load(currentUrl);

                nextUrl = AddWordUsingDoc(doc, notecards, kanjiNoteCard, pageNumber);

                //Checks if there is an anchor tag to the next page.
                if (nextUrl != "")
                {
                    //currentUrl = moreWordsNode.GetAttributeValue("href", "");
                    //TODO: This is here for test purposes (not making calls to jisho until tested);
                    nextUrl = pathToTestFile2;
                    if (currentUrl != nextUrl)
                    {
                        currentUrl = nextUrl;
                        pageNumber++;
                    }
                    else {
                        Console.WriteLine("Same page twice, stopping loop");
                        break;
                    }
                }
                //ADD CURRENT COUNT TO BREAK IT OUT OF LIMIT
                currentLinkCount++;
            }

            return notecards;
        }

        /// <summary>
        /// Takes in an HTML Doc.  adds the words to the List.  
        /// </summary>
        /// <returns>Returns the url if there is a link to other page or "" if there is not another page</returns>
        private string AddWordUsingDoc(HtmlDocument doc, List<JapaneseWordNoteCard> noteCards, ChapterNoteCard kanji, int pageNumber)
        {
            var mainResults = doc.GetElementbyId("main_results");
            noteCards.AddRange(GetAllInfo(mainResults, kanji, pageNumber));
            var moreWordsNode = mainResults.SelectNodes(".//a").First(node => node.GetClasses().Contains("more"));
            if (moreWordsNode != null)
            {
                var nextUrl = moreWordsNode.GetAttributeValue("href", "");
                return nextUrl;
            }
            else
            {
                return "";
            }
        }

        private string GetKanjiFromPage(HtmlNode startDiv)
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

        private List<JapaneseWordNoteCard> GetAllInfo(HtmlNode startDiv, ChapterNoteCard kanji, int pageNumber)
        {
            List<JapaneseWordNoteCard> japaneseWordNoteCards = new List<JapaneseWordNoteCard>();

            var primaryDiv = startDiv.Descendants().First(desc => desc.Id == "primary");
            var wordDivs = primaryDiv.Descendants().Where(desc =>
            {
                var classes = desc.GetClasses();
                return classes.Contains("concept_light") && classes.Contains("clearfix");
            });

            if (!wordDivs.Any())
            {
                throw new ArgumentException("No words, probably a bad file");
            }

            foreach (var wordDiv in wordDivs)
            {
                var wordInfo = new JapanWordInfoFromDiv(wordDiv);

                if (wordInfo != null && wordInfo.Word.Contains(kanji.TopicName))
                {
                    var mapper = new InfoMapper();
                    var japanNoteCard = mapper.ToJapanNoteCard(wordInfo, kanji);
                    japaneseWordNoteCards.Add(japanNoteCard);
                    japanNoteCard.SentenceNoteCard.ChapterSentences.Add(PageAndOrderNumber(pageNumber, kanji.TopicName, japanNoteCard.SentenceNoteCard.ItemQuestion));
                }      
            }
            return japaneseWordNoteCards;
        }

        private ChapterNoteCardSentenceNoteCard PageAndOrderNumber(int pageNumber, string kanji, string word)
        {
            count++;
            return new ChapterNoteCardSentenceNoteCard { ChapterNoteCardTopicName = kanji, SentenceNoteCardItemQuestion = word, ExtraJishoInfo = new ExtraJishoInfoOnBridge { PageNumber = pageNumber, Order = count } };
        }
        private int GetPageNumberFromURL(string url)
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


//Old Comments
//AddRange will change the list that called it