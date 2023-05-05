using DataLayer.CSV;
using DataLayer.Entities;
using DataLayer.IRepos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebScraper.CsvUI
{
    public static class GetAnExportCSV
    {
        public async static void ExportCSV(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var repo = serviceProvider.GetRequiredService<IGenericRepo<SentenceNoteCard>>();
                var svcCSV = new ExportSentencesCSV(repo);
                await svcCSV.CreateCSV();
            }
        }
    }
}

//Old Comments
//Dependency Injection
//https://stackoverflow.com/questions/55983541/understanding-net-core-dependency-injection-in-a-console-app