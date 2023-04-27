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

await ParseKanjiHtmlFromFile.AddTestFileToDatabase(host);
//ParseWordsFromFile.GetJapaneseWordNoteCardFromFile();


//TODO:  when it does #word in the search bar after clicking the link, it removes the "Kanji - 1 Found" div, don't know if I should cahnge the way it get it
//Kanji would be the same for very page, so as long as I get from 1st page, should be fine
//TODO: put code inside for loop.

//await ParseWordsFromFile.AddWordsToDatabase(host);


Console.WriteLine("The Program has ended....beep boop bop");
