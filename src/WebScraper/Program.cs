// See https://aka.ms/new-console-template for more information
using DataLayer;
using Microsoft.Extensions.Hosting;
using WebScraper.ParseHTML;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDiForDbContext();
        
    }).Build();

//Ctrl+K and Ctrl+F to autoformat a section
//Ctrl+K and Ctrl+D to autoformat document

//ParseKanjiHtmlFromFile.AddTestFileToDatabase(host);
//ParseWordsFromFile.GetJapaneseWordNoteCardFromFile();

await ParseWordsFromFile.AddWordsToDatabase(host);


Console.WriteLine("The Program has ended....beep boop bop");
