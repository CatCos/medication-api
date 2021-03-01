namespace MedicationApi.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using MedicationApi.Application.Handlers.Queries;
    using MedicationApi.Data.Models.Entities;
    using MedicationApi.Data.Models.Queries;
    using MedicationApi.Repositories.Read;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class GetMedicationsQueryHandlerTests
    {
        private readonly Fixture fixture;

        private readonly Mock<IMedicationReadRepository> medicationReadRepositoryMock;

        private readonly Mock<ILogger<GetMedicationsQueryHandler>> logMock;

        private readonly GetMedicationsQueryHandler getMedicationsQueryHandler;

        public GetMedicationsQueryHandlerTests()
        {
            this.fixture = new Fixture();

            this.medicationReadRepositoryMock = new Mock<IMedicationReadRepository>();
            this.logMock = new Mock<ILogger<GetMedicationsQueryHandler>>();

            this.getMedicationsQueryHandler = new GetMedicationsQueryHandler(
                this.medicationReadRepositoryMock.Object,
                this.logMock.Object);
        }

        [TestMethod]
        public async Task GetMedicationsQueryHandler_HandleAsync_ReturnsListOfMedications()
        {
            // Arrange
            var medications = this.fixture.CreateMany<Medication>();
            var query = this.fixture.Create<GetMedicationsQuery>();

            this.medicationReadRepositoryMock.Setup(
                x => x.GetAllMedicationsAsync(
                    It.Is<int>(x => x == query.Offset),
                    It.Is<int>(x => x == query.Limit)))
                .ReturnsAsync(medications);

            // Act
            var result = await this.getMedicationsQueryHandler.HandleAsync(query);

            // Assert
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.IsTrue(result.Any(), "Should not return an empty list.");

            this.medicationReadRepositoryMock.Verify(
                x => x.GetAllMedicationsAsync(
                    It.Is<int>(x => x == query.Offset),
                    It.Is<int>(x => x == query.Limit)),
                Times.Once);
        }

        [TestMethod]
        public async Task GetMedicationsQueryHandler_HandleAsync_ThrowsException()
        {
            // Arrange
            var exception = this.fixture.Create<Exception>();

            var query = this.fixture.Create<GetMedicationsQuery>();

            this.medicationReadRepositoryMock.Setup(
                x => x.GetAllMedicationsAsync(
                    It.Is<int>(x => x == query.Offset),
                    It.Is<int>(x => x == query.Limit)))
                .ThrowsAsync(exception);

            // Act
            var ex = await Assert.ThrowsExceptionAsync<Exception>(
                () => this.getMedicationsQueryHandler.HandleAsync(query));

            // Assert
            Assert.IsTrue(ex.Message.Contains(exception.Message, StringComparison.InvariantCultureIgnoreCase));

            this.medicationReadRepositoryMock.Verify(
                x => x.GetAllMedicationsAsync(
                    It.Is<int>(x => x == query.Offset),
                    It.Is<int>(x => x == query.Limit)),
                Times.Once);
        }
    }
}
