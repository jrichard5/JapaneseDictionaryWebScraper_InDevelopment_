using DataLayer.Entities;
using DataLayer.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
    {
        //Is the repository pattern suppose to be DbSets >.> monkaS i forgot
        public CategoryRepo(KanjiDbContext context) : base(context)
        {
        }

        public async Task<Category> GetFirstCategoryByName(string name)
        {
            var result =  await this._dbContext.Categories.FirstAsync(cate => cate.CategoryName == name);
            return result;
        }
    }
}
