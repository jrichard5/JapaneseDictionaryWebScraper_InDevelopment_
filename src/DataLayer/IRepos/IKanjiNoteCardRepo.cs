using DataLayer.Entities;

namespace DataLayer.IRepos
{
    //I feel like I could've just used IGenericRepo<Category> instead of making another, but decided to make this interfaces for entities incase they need a specific function/method in the future
    public interface IKanjiNoteCardRepo : IGenericRepo<KanjiNoteCard>
    {
        //Since only adding one kanji, going to make it so if it fails the unique constraint DbUpdateException, it will just skip it;
        public Task AddButSkipUniqueException(KanjiNoteCard item);
    }
}
