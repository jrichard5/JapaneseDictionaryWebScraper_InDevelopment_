using DataLayer.Entities;

namespace DataLayer.IRepos
{
    //I feel like I could've just used IGenericRepo<Category> instead of making another, but decided to make this interfaces for entities incase they need a specific function/method in the future
    public interface ICategoryRepo : IGenericRepo<Category>
    {
        Task<Category> GetFirstCategoryByName(string name);
    }
}
