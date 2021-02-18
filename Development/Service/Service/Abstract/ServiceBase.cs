using Domain.Repository;
using Service.Mapper;

namespace Service.Service.Abstract
{
    public abstract class ServiceBase
    {
        protected readonly IMapper mapper;
        protected readonly IUnitOfWork uow;

        protected ServiceBase(IUnitOfWork uow, IMapper mapper)
        {
            this.mapper = mapper;
            this.uow = uow;
        }
    }
}
