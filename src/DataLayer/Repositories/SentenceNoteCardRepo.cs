using DataLayer.Entities;
using DataLayer.IRepos;

namespace DataLayer.Repositories
{
    public class SentenceNoteCardRepo : GenericRepo<SentenceNoteCard>, ISentenceNoteCardRepo
    {

        //Is the repository pattern suppose to be DbSets >.> monkaS i forgot
        public SentenceNoteCardRepo(KanjiDbContext context) : base(context) { }
    }
}
