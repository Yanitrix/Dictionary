
namespace Service
{
    public interface IService<T>
    {
        public IValidationDictionary TryAdd(T entity);
        
        public IValidationDictionary TryUpdate(T entity);

        public IValidationDictionary IsValid(T entity);
    }
}
