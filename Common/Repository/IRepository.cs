namespace Common.Repository;

public interface IRepository : IReadonlyRepository
{
    void Create<TEntity>(TEntity entity) where TEntity : class;
    void Update<TEntity>(TEntity entity) where TEntity : class;
    void Delete<TEntity>(int id) where TEntity : class;
    void Delete<TEntity>(TEntity entity) where TEntity : class;

    void Save();
    Task SaveAsync();
}
