using DataLayer.Entities;
using DataLayer.IRepos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class ChapterNoteCardRepo : GenericRepo<ChapterNoteCard>, IChapterNoteCardRepo
    {
        //Is the repository pattern suppose to be DbSets >.> monkaS i forgot
        //private readonly DbSet<ChapterNoteCard> _chapters;

        public ChapterNoteCardRepo(KanjiDbContext context) : base(context)
        {
            //I think this is what I would do if using dbset....
            //_chapters = context.Chapters;
        }

        public async Task<ChapterNoteCard> GetChapterNoteCardByTopicName(string topicName)
        {
            return await _dbContext.Chapters.FirstAsync(chapter => chapter.TopicName == topicName);
        }

        public async Task<int> GetLastItemByTopicName(string topicName)
        {
            var lastItem = await _dbContext.Chapters.Where(c => c.TopicName == topicName)
                .SelectMany(c => c.ChapterSentences)
                .MaxAsync(cs => (int?)cs.ExtraJishoInfo.Order);

            if (lastItem == null) {
                return 0;
            }
            Console.WriteLine();
            return (int)lastItem;
        }
    }
}
