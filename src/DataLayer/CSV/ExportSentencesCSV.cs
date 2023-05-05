using CsvHelper;
using DataLayer.Entities;
using DataLayer.IRepos;
using System.Globalization;

namespace DataLayer.CSV
{
    public class ExportSentencesCSV
    {
        private readonly IGenericRepo<SentenceNoteCard> _repo;
        public ExportSentencesCSV(IGenericRepo<SentenceNoteCard> repo) {
            _repo = repo;
        }

        public async Task CreateCSV()
        {
            var sentences = await _repo.GetAll();

            string fileName = "anywhereYouWantToSaveIt.csv";
            string path = Path.Combine(Environment.CurrentDirectory, fileName);
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(sentences);
                Console.WriteLine($"wrote CSV file to {path}");
            }
        }
    }
}
