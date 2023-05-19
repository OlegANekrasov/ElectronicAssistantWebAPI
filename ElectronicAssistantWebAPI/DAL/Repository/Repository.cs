using ElectronicAssistantWebAPI.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace ElectronicAssistantWebAPI.DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext _db;

        public DbSet<T> Set { get; private set; }

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            var set = _db.Set<T>();
            set.Load();

            Set = set;
        }

        public IEnumerable<T> Get()
        {
            return Set;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await Set.FindAsync(id);
        }

        public async Task CreateAsync(T item)
        {
            Set.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(T item)
        {
            Set.Update(item);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var item = await Set.FindAsync(id);
            if (item != null)
            {
                Set.Remove(item);
                await _db.SaveChangesAsync();
            }
        }
    }
}
