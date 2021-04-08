namespace Domain.Queries
{
    public interface IQueryHandler<T, R> where T : IQuery
    {
        R Handle(T query);
    }
}