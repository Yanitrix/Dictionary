namespace Service
{
    public interface IService<T>
    {
        public ValidationResult Add(T entity);

        public ValidationResult Update(T entity);
    }
}
