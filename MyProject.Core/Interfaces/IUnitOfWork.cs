namespace MyProject.Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
        IRepository<T> GetRepository<T>() where T : class;
        //Task ExecuteInTransactionAsync(Func<Task> action);
    }
}