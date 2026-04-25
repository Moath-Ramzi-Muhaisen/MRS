using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public IQueryable<T> GetAll();
        public void Delete(T input);
        public void Update(T input);
        public T GetById(Guid id);
        public Task<T> GetByIdAsync(Guid id);
        public void Insert(T input);
        public Task InsertAsync(T input);
        public void InsertRange(List<T> input);
        public Task InsertRangeAsync(List<T> input);
        public void SaveChanges();
        public Task SaveChangesAsync();
    }
}
