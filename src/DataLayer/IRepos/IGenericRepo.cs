namespace DataLayer.IRepos
{
    public interface IGenericRepo<T> where T : class
    {
        public Task<T> AddAsync(T entity);
        public Task DeleteAsync(T entity);
        public Task<T> FindByIdAsync(int id);
        public Task<List<T>> GetAll();
    }
}
