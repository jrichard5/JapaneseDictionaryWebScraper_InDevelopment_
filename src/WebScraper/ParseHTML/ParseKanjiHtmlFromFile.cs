using DataLayer.CSV;
using DataLayer.Entities;
using DataLayer.IRepos;
using DataLayer.Repositories;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//NOTES:  //Need the '.' for the xpath to search the "context node"


namespace WebScraper.ParseHTML
{
    public static class ParseKanjiHtmlFromFile
    {
        public async static Task AddTestFileToDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var kanjiRepo = serviceProvider.GetRequiredService<IKanjiNoteCardRepo>();
                var categoryRepo = serviceProvider.GetRequiredService<ICategoryRepo>();
                var testFilePathTuple = await testFilePath(categoryRepo);
                var kanjinotecard = testFilePathTuple.Item1;
                var url = testFilePathTuple.Item2;


                await Task.Delay(1000);
                await kanjiRepo.AddButSkipUniqueException(kanjinotecard);
                Console.WriteLine("calling url but should get changed " + url);
                await Task.Delay(1000);
                ParseWordsFromFile parser = new ParseWordsFromFile();
                await parser.AddWordsToDatabase(host, url);
            }
        }

        public async static Task<(KanjiNoteCard, string)> testFilePath(ICategoryRepo cateRepo)
        {
            KanjiNoteCard testKanjiNoteCard = new KanjiNoteCard();
            testKanjiNoteCard.ChapterNoteCard = new ChapterNoteCard();
            testKanjiNoteCard.ChapterNoteCard.Category = await cateRepo.GetFirstCategoryByName("Japanese Vocab");
            Console.WriteLine(testKanjiNoteCard.ChapterNoteCard.Category.Id + " category number --------");

            List<KanjiReading> kanjiReadings = new List<KanjiReading>();

            string pathToTestFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\zzNihongoDb\一 #kanji - Jisho.org.htm";

            var doc = new HtmlDocument();
            doc.Load(pathToTestFile);

            ////How to print out entire HTML
            //var node = doc.Text;
            //Console.WriteLine(node);

            var resultsDiv = doc.GetElementbyId("result_area");
            if (resultsDiv == null)
            {
                throw new Exception("No div with result area");
            }

            var kanji = GetKanji(resultsDiv);
            testKanjiNoteCard.ChapterNoteCard.TopicName = kanji;


            //TODO: determine if I should just make all these throw exceptions, and use one try/catch block
            //If one fails, I don't really want to continue
            try
            {
                var meaning = GetMeaning(resultsDiv);
                testKanjiNoteCard.ChapterNoteCard.TopicDefinition = meaning;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Programmer miss #1");
            }

            var url = GetNextUrl(resultsDiv);

            GetGrade(resultsDiv, testKanjiNoteCard);
            GetJLPTLevel(resultsDiv, testKanjiNoteCard);
            GetNewspaperRank(resultsDiv, testKanjiNoteCard);
            GetReadings(resultsDiv, kanjiReadings, kanji);
            Console.WriteLine($"this should have something {kanjiReadings[0]}");
            testKanjiNoteCard.KanjiReadings = kanjiReadings;
            return (testKanjiNoteCard, url);
        }



        private static string GetNextUrl(HtmlNode startDiv)
        {
            var urlLi = startDiv.SelectNodes(".//a").First(node => node.InnerText.StartsWith("Words containing")).GetAttributeValue("href", "");
            Console.WriteLine(urlLi);
            return urlLi;
        }

        private static void GetReadings(HtmlNode startDiv, List<KanjiReading> readings, string kanji)
        {
            var readingsDiv = startDiv.Descendants().First(desc => desc.GetClasses().Contains("kanji-details__main-readings"));
            var kunDl = readingsDiv.Descendants().First(desc => desc.GetClasses().Contains("kun_yomi"));
            var onDl = readingsDiv.Descendants().First(desc => desc.GetClasses().Contains("on_yomi"));

            var kunTags = kunDl.SelectNodes(".//dd/a");
            var onTags = onDl.SelectNodes(".//dd/a");

            foreach (var kunTag in kunTags)
            {
                Console.WriteLine(kunTag.InnerText);
                readings.Add(new KanjiReading { KanjiNoteCardTopicName = kanji, TypeOfReading = "kun", Reading = kunTag.InnerText });
            }
            foreach (var onTag in onTags)
            {
                Console.WriteLine(onTag.InnerText);
                readings.Add(new KanjiReading { KanjiNoteCardTopicName = kanji, TypeOfReading = "on", Reading = onTag.InnerText });
            }
            //TODO:  Remove this, as this was test to make sure right words
            //string localdirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\zzNihongoDb";
            //using StreamWriter outputFile = new StreamWriter(Path.Combine(localdirectory, "hi.txt"));
            //{
            //    foreach (KanjiReading kanjiReading in readings)
            //    {
            //        outputFile.WriteLine(kanjiReading.TypeOfReading + "  " + kanjiReading.Reading);
            //        Console.WriteLine("writing line to file");
            //    }
            //}
        }
        private static void GetNewspaperRank(HtmlNode startDiv, KanjiNoteCard notecard)
        {
            var frequencyDiv = startDiv.Descendants().First(desc => desc.GetClasses().Contains("frequency"));
            var newspaperRank = frequencyDiv.SelectSingleNode(".//strong").InnerText.Trim();
            int result = -1;
            if (Int32.TryParse(newspaperRank, out result))
            {
                Console.WriteLine($"newspaper rank is {result}");
                notecard.NewspaperRank = result;
            }
            else
            {
                Console.WriteLine("This really should be debugbut oh well");
            }
            
        }

        //COPYRIGHT question jlpt????
        private static void GetJLPTLevel(HtmlNode startDiv, KanjiNoteCard notecard)
        {
            var jlptDiv = startDiv.Descendants().First(desc => desc.GetClasses().Contains("jlpt"));
            var jlptNumber = jlptDiv.SelectSingleNode(".//strong").InnerText.Replace("N", "").Trim();
            var result = -1;
            if (Int32.TryParse(jlptNumber, out result))
            {
                Console.WriteLine($"jlpt --- {result}");
                notecard.JLPTLevel = result;
            }
            else
            {
                Console.WriteLine("Debug.WriteLine(jlpterror)");
            }

        }
        private static string GetKanji(HtmlNode startDiv)
        {
            //Need the '.' for the xpath to search the "context node"
            string kanji = startDiv.SelectSingleNode(".//h1").InnerHtml;
            Console.WriteLine(kanji);
            return kanji;
        }

        private static string GetMeaning(HtmlNode startDiv)
        {
            var kanjiMeaningNode = startDiv.Descendants()
                .FirstOrDefault(descendant => descendant.GetClasses().Contains("kanji-details__main-meanings"));

            if (kanjiMeaningNode != null)
            {
                var kanjiMeaning = kanjiMeaningNode.InnerHtml.Trim();
                Console.WriteLine(kanjiMeaning);
                return kanjiMeaning;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        private static void GetGrade(HtmlNode startDiv, KanjiNoteCard notecard)
        {
            var gradeDiv = startDiv.Descendants().First(desc => desc.GetClasses().Contains("grade"));
            //Will return grade 1
            var grade = gradeDiv.SelectSingleNode(".//strong").InnerText.Replace("grade", "").Trim();

            int result = -1;
            if (Int32.TryParse(grade, out result))
            {
                Console.WriteLine($"Debug.WriteLine(grade is {grade})");
                notecard.ChapterNoteCard.GradeLevel = result;
            }
            else
            {
                Console.WriteLine("Could not get a number for the grade column");
            }
        }
    }
}
