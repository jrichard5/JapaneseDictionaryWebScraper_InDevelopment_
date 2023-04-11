// See https://aka.ms/new-console-template for more information


using DataLayer;
using DataLayer.CSV;
using DataLayer.Entities;
using DataLayer.IRepos;
using DataLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebScraper.CsvUI;



IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDiForDbContext();
        
    }).Build();

//Dependency Injection
//https://stackoverflow.com/questions/55983541/understanding-net-core-dependency-injection-in-a-console-app
using (var scope = host.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var repo = serviceProvider.GetRequiredService<IGenericRepo<SentenceNoteCard>>();
    var svcCSV = new ExportSentencesCSV(repo);
    await svcCSV.CreateCSV();
}


Console.WriteLine("The Program has ended....beep boop bop");
