using Moq;
using Service.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Tests.Service
{
    public class UowTestBase
    {
        protected Mock<UnitOfWork> uow = new Mock<UnitOfWork>();

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

        protected Mock<IExpressionRepository> expRepo
        {
            set
            {
                uow.Setup(_ => _.Expressions).Returns(value.Object);
            }
        }
    }
}
