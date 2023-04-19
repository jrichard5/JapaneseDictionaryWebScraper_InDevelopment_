using HtmlAgilityPack;

namespace WebScraper.ParseHTML
{
    public static class ParseKanjiHtmlFromFile
    {
        public static void testFilePath()
        {
            string pathToTestFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\zzNihongoDb\人 #kanji - Jisho.org.htm";

            var doc = new HtmlDocument();
            doc.Load(pathToTestFile);

            ////How to print out entire HTML
            //var node = doc.Text;
            //Console.WriteLine(node);

            var resultsDiv = doc.GetElementbyId("result_area");

            //Need the '.' for the xpath to search the "context node"
            string kanji = resultsDiv.SelectSingleNode(".//h1").InnerHtml;
            Console.WriteLine(kanji);

            var kanjiMeaning = resultsDiv.Descendants()
                .FirstOrDefault(descendant => descendant.GetClasses().Contains("kanji-details__main-meanings")).InnerHtml.Trim();

            if (kanjiMeaning != null)
            {
                Console.WriteLine(kanjiMeaning);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        
        
    }
}
