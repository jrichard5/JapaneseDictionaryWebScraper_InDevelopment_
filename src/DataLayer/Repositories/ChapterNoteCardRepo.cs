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
        public ChapterNoteCardRepo(KanjiDbContext context) : base(context) { }

        public async Task<ChapterNoteCard> GetChapterNoteCardByTopicName(string topicName)
        {
            return await _dbContext.Chapters.FirstAsync(chapter => chapter.TopicName == topicName);
        }
    }
}
