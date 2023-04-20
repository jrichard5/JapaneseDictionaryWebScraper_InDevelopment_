using DataLayer.Entities;
using DataLayer.IRepos;
using DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace DataLayer
{
    public static class DbContextServiceOptions
    {
        public static void AddDiForDbContext(this IServiceCollection services)
        {
            
            string DbPath;
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var newAppDataFolder = Path.Join(path, @"zzNihongoDb\");
            Directory.CreateDirectory(newAppDataFolder);
            DbPath = System.IO.Path.Combine(newAppDataFolder, "kdb.db");

            services.AddDbContext<KanjiDbContext>();
            //    options =>
            //{
            //    options.UseSqlite($"Data Source={DbPath}");
            //});
            services.AddScoped<ISentenceNoteCardRepo, SentenceNoteCardRepo>();
            services.AddScoped<IKanjiNoteCardRepo, KanjiNoteCardRepo>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IChapterNoteCardRepo, ChapterNoteCardRepo>();
            services.AddScoped<IJapaneseWordNoteCardRepo, JapaneseWordNoteCardRepo>();
        }
    }
}
