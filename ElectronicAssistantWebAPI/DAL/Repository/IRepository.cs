namespace ElectronicAssistantWebAPI.DAL.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Get();
        Task<T> GetByIdAsync(string id);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(string id);
    }
}
