using Application;
using Application.Room.Queries;
using Application.Room.Querys;
using Domain.Room.Entities;
using Domain.Room.Ports;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests
{
    public class GetRoomQueryHandlerTests
    {
        [Test]
        public async Task Should_Return_Room()
        {
            var query = new GetRoomQuery { Id = 1 };

            var repoMock = new Mock<IRoomRepository>();
            var fakeRoom = new Room() { Id = 1 };
            repoMock.Setup(x => x.Get(query.Id)).Returns(Task.FromResult(fakeRoom));

            var handler = new GetRoomQueryHandler(repoMock.Object);
            var res = await handler.Handle(query, CancellationToken.None);

            Assert.True(res.Success);
            Assert.NotNull(res.Data);
        }

        [Test]
        public async Task Should_Return_ProperError_Message_WhenRoom_NotFound()
        {
            var query = new GetRoomQuery { Id = 1 };

            var repoMock = new Mock<IRoomRepository>();

            var handler = new GetRoomQueryHandler(repoMock.Object);
            var res = await handler.Handle(query, CancellationToken.None);

            Assert.False(res.Success);
            Assert.That(res.ErrorCode, Is.EqualTo(ErrorCodes.ROOM_NOT_FOUND));
            Assert.That(res.Message, Is.EqualTo("Could not find a Room with the given id"));
        }
    }
}
