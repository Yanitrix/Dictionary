namespace Service.Mapper
{
    public interface IMapper
    {
        public R Map<T, R>(T source);

        public R Map<T, R>(T source, R destination);
    }
}
