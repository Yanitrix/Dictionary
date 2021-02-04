namespace Service
{
    public interface IService<T>
    {
        public IValidationDictionary Add(T entity);

        public IValidationDictionary Update(T entity);
    }
}
