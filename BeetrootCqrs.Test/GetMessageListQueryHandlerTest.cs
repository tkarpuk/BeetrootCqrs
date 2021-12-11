using BeetrootCqrs.BLL.Messages.Queries.GetMessageList;
using BeetrootCqrs.Test.Common;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BeetrootCqrs.Test
{
    public class GetMessageListQueryHandlerTest : BaseCommand
    {
        [Fact]
        public async Task GetMessageListQueryHandler_OneIP()
        {
            //Arrange
            var handler = new GetMessageListQueryHandler(_dbContext);

            //Act
            var lst = await handler.Handle(
                new GetMessageListQuery() 
                {
                    IpAddress = "127.0.0.2"
                }, CancellationToken.None);

            //Assert
            Assert.True(lst.ToList().Count == 1);
        }

        [Fact]
        public async Task GetMessageListQueryHandler_Date_Start_End()
        {
            //Arrange
            var handler = new GetMessageListQueryHandler(_dbContext);

            //Act
            var lst = await handler.Handle(
                new GetMessageListQuery()
                {
                    DateStart = new DateTime(2021, 12, 01),
                    DateEnd = new DateTime(2021, 12, 03)
                }, CancellationToken.None);

            //Assert
            Assert.True(lst.ToList().Count == 2);
        }

        [Fact]
        public async Task GetMessageListQueryHandler_EmptyResult_IP_plus_Start_End()
        {
            //Arrange
            var handler = new GetMessageListQueryHandler(_dbContext);

            //Act
            var lst = await handler.Handle(
                new GetMessageListQuery()
                {
                    IpAddress = "1.1.1.1",
                    DateStart = new DateTime(2021, 12, 01),
                    DateEnd = new DateTime(2021, 12, 03)
                }, CancellationToken.None);

            //Assert
            Assert.Empty(lst);
        }

        [Fact]
        public async Task GetMessageListQueryHandler_IP_plus_Start()
        {
            //Arrange
            var handler = new GetMessageListQueryHandler(_dbContext);

            //Act
            var lst = await handler.Handle(
                new GetMessageListQuery()
                {
                    IpAddress = "127.0.0.3",
                    DateStart = new DateTime(2021, 12, 04)
                }, CancellationToken.None);

            //Assert
            Assert.Single(lst);
        }

        [Fact]
        public async Task GetMessageListQueryHandler_IP_plus_End()
        {
            //Arrange
            var handler = new GetMessageListQueryHandler(_dbContext);

            //Act
            var lst = await handler.Handle(
                new GetMessageListQuery()
                {
                    IpAddress = "127.0.0.3",
                    DateEnd = new DateTime(2021, 12, 04)
                }, CancellationToken.None);

            //Assert
            Assert.Empty(lst);
        }
    }
}
