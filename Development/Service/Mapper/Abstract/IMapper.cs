namespace Service.Mapper
{
    public interface IMapper
    {
        public R Map<T, R>(T source);

        public static IMapper CreateMapper(AbstractMappingConfiguration config) => new Mapper(config);
    }
}
