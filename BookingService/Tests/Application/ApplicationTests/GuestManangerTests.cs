using Application;
using Application.Guest.DTO;
using Application.Guest.Ports;
using Application.Guest.Requests;
using Application.Guest.Responses;
using Domain.Guest.Entities;
using Domain.Guest.Enums;
using Domain.Guest.Ports;
using Moq;

namespace ApplicationTests
{

    public class Tests
    {
        GuestManager guestMananger;

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public async Task HappyPath()
        {
            var guestDto = new GuestDTO
            {
                Name = "Test",
                Surname = "Test",
                Email = "Test@email.com",
                IdNumber = "idNumberTest",
                IdTypeCode = 1
            };

            int expectedId = 222;

            var request = new CreateGuestRequest
            {
                Data = guestDto,
            };

            var fakeRepo = new Mock<IGuestRepository>();

            fakeRepo.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(222));

            guestMananger = new GuestManager(fakeRepo.Object);


            var res = await guestMananger.CreateGuest(request);
            Assert.IsNotNull(res);
            Assert.True(res.Success);
            Assert.That(expectedId.Equals(res.Data.Id));
        }

        [TestCase(null)]
        [TestCase("a")]
        [TestCase("ab")]
        [TestCase("")]
        public async Task ShouldInvalidPersonalDocumentIdExceptionWhenDocsAreInvalid(string docNumber)
        {
            var guestDto = new GuestDTO
            {
                Name = "Test",
                Surname = "Test",
                Email = "Test@email.com",
                IdNumber = docNumber,
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest
            {
                Data = guestDto,
            };

            var fakeRepo = new Mock<IGuestRepository>();

            fakeRepo.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(222));

            guestMananger = new GuestManager(fakeRepo.Object);


            var res = await guestMananger.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.That(ErrorCodes.INVALID_PERSON_ID.Equals(res.ErrorCode));
            Assert.That("The ID passed is not valid.".Equals(res.Message));
        }

        [TestCase("", "surname test", "email@email.com")]
        [TestCase("name test", "", "email@email.com")]
        [TestCase("name", "surname", "")]
        public async Task ShouldMissingRequiredInformationExceptionWhenInfoAreInvalid(string name, string surname, string email)
        {
            var guestDto = new GuestDTO
            {
                Name = name,
                Surname = surname,
                Email = email,
                IdNumber = "abc213",
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest
            {
                Data = guestDto,
            };

            var fakeRepo = new Mock<IGuestRepository>();

            fakeRepo.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(222));

            guestMananger = new GuestManager(fakeRepo.Object);


            var res = await guestMananger.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.That(ErrorCodes.MISSING_REQUIRED_INFORMATION.Equals(res.ErrorCode));
            Assert.That("Missing required information.".Equals(res.Message));
        }

        [Test]
        public async Task Should_Return_GuestNotFound_When_GuestDoestnExists()
        {
            var fakeRepo = new Mock<IGuestRepository>();

            var fakeGuest = new Guest
            {
                Id = 333,
                Name = "test",
            };

            fakeRepo.Setup(x => x.Get(333)).Returns(Task.FromResult<Guest?>(null));

            guestMananger = new GuestManager(fakeRepo.Object);

            var res = await guestMananger.GetGuest(333);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.That(ErrorCodes.GUEST_NOT_FOUND.Equals(res.ErrorCode));
            Assert.That("No guest record was found with the given id".Equals(res.Message));
        }

        [Test]
        public async Task Should_Return_Guest_Success()
        {
            var fakeRepo = new Mock<IGuestRepository>();

            var fakeGuest = new Guest
            {
                Id = 333,
                Name = "test",
                DocumentId = new Domain.Guest.ValueObjects.PersonId
                {
                    DocumentType = DocumentType.DriveLicence,
                    IdNumber = "123",
                }
            };

            fakeRepo.Setup(x => x.Get(333)).Returns(Task.FromResult((Guest?)fakeGuest));

            guestMananger = new GuestManager(fakeRepo.Object);

            var res = await guestMananger.GetGuest(333);

            Assert.IsNotNull(res);
            Assert.True(res.Success);
            Assert.That(fakeGuest.Id.Equals(res.Data.Id));
        }
        // IMPLEMENTAR TESTCASE DO EMAIL DEPOIS
    }
}