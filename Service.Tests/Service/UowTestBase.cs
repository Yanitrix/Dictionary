using Moq;
using Service.Repository;

namespace Service.Tests.Service
{
    public class UowTestBase
    {
        protected Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

        protected Mock<ILanguageRepository> langRepo
        {
            set
            {
                uow.Setup(_ => _.Languages).Returns(value.Object);
            }
        }

        protected Mock<IDictionaryRepository> dictRepo
        {
            set
            {
                uow.Setup(_ => _.Dictionaries).Returns(value.Object);
            }
        }

        protected Mock<IWordRepository> wordRepo
        {
            set
            {
                uow.Setup(_ => _.Words).Returns(value.Object);
            }
        }

        protected Mock<IEntryRepository> entryRepo
        {
            set
            {
                uow.Setup(_ => _.Entries).Returns(value.Object);
            }
        }

        protected Mock<IMeaningRepository> meaningRepo
        {
            set
            {
                uow.Setup(_ => _.Meanings).Returns(value.Object);
            }
        }

        protected Mock<IFreeExpressionRepository> freeExpressionRepo
        {
            set
            {
                uow.Setup(_ => _.FreeExpressions).Returns(value.Object);
            }
        }

        protected Mock<IExampleRepository> exampleRepo
        {
            set
            {
                uow.Setup(_ => _.Examples).Returns(value.Object);
            }
        }
    }
}
