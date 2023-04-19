// See https://aka.ms/new-console-template for more information
using DataLayer;
using Microsoft.Extensions.Hosting;
using WebScraper.ParseHTML;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDiForDbContext();
        
    }).Build();


ParseKanjiHtmlFromFile.testFilePath();

Console.WriteLine("The Program has ended....beep boop bop");
