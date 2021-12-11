using BeetrootCqrs.BLL.Messages.Commands.SaveMessage;
using BeetrootCqrs.Test.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Xunit;

namespace BeetrootCqrs.Test
{
    public class SaveMessageCommandHandlerTest : BaseCommand
    {
        public SaveMessageCommandHandlerTest()
        {
        }

        [Fact]
        public async Task SaveMessageCommandHandler_Succsess()
        {
            //Arrange
            var handler = new SaveMessageCommandHandler(_dbContext);
            string ipAddress = "2.0.0.2";
            string textMessage = "test message 1";

            //Act
            var messageId = await handler.Handle(
                new SaveMessageCommand()
                {
                    IpAddress = ipAddress,
                    Text = textMessage
                },
                CancellationToken.None);

            //Assert
            Assert.NotNull(_dbContext.Messages.FirstOrDefault(m => (m.Id == messageId) && (m.Text == textMessage)));
        }
    }
}
