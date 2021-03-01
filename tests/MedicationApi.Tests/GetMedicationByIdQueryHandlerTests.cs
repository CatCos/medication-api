namespace MedicationApi.Tests
{
    using System;
    using System.Threading.Tasks;
    using AutoFixture;
    using MedicationApi.Application.Exceptions;
    using MedicationApi.Application.Handlers.Queries;
    using MedicationApi.Data.Models.Entities;
    using MedicationApi.Data.Models.Queries;
    using MedicationApi.Repositories.Read;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class GetMedicationByIdQueryHandlerTests
    {
        private readonly Fixture fixture;

        private readonly Mock<IMedicationReadRepository> medicationReadRepositoryMock;

        private readonly Mock<ILogger<GetMedicationByIdQueryHandler>> logMock;

        private readonly GetMedicationByIdQueryHandler getMedicationByIdQueryHandler;

        public GetMedicationByIdQueryHandlerTests()
        {
            this.fixture = new Fixture();

            this.medicationReadRepositoryMock = new Mock<IMedicationReadRepository>();
            this.logMock = new Mock<ILogger<GetMedicationByIdQueryHandler>>();

            this.getMedicationByIdQueryHandler = new GetMedicationByIdQueryHandler(
                this.medicationReadRepositoryMock.Object,
                this.logMock.Object);
        }

        [TestMethod]
        public async Task GetMedicationByIdQueryHandler_HandleAsync_ReturnsMedication()
        {
            // Arrange
            var medicationId = this.fixture.Create<Guid>();
            var medication = this.fixture.Build<Medication>()
                .With(x => x.Id, medicationId)
                .Create();

            var query = this.fixture.Build<GetMedicationByIdQuery>()
                .With(x => x.Id, medicationId)
                .Create();

            this.medicationReadRepositoryMock.Setup(
                x => x.GetMedicationByIdAsync(
                    It.Is<Guid>(y => y == medicationId)))
                .ReturnsAsync(medication);

            // Act
            var result = await this.getMedicationByIdQueryHandler.HandleAsync(query);

            // Assert

            Assert.IsNotNull(result, "Result should not be null.");

            Assert.AreEqual(medication.Id, result.Id, "Invalid Id value returned.");
            Assert.AreEqual(medication.Name, result.Name, "Invalid Name value returned.");
            Assert.AreEqual(medication.Quantity, result.Quantity, "Invalid Quantity value returned.");
            Assert.AreEqual(medication.CreatedAt, result.CreatedAt, "Invalid CreatedAt value returned.");

            this.medicationReadRepositoryMock.Verify(
                 x => x.GetMedicationByIdAsync(
                    It.Is<Guid>(y => y == medicationId)),
                Times.Once);
        }

        [TestMethod]
        public async Task GetMedicationByIdQueryHandler_HandleAsync_ThrowsNotFound()
        {
            // Arrange
            var medicationId = this.fixture.Create<Guid>();
            var medication = this.fixture.Build<Medication>()
                .With(x => x.Id, medicationId)
                .Create();

            var query = this.fixture.Build<GetMedicationByIdQuery>()
                .With(x => x.Id, medicationId)
                .Create();

            var exception = this.fixture.Create<NotFoundException>();

            this.medicationReadRepositoryMock.Setup(
                x => x.GetMedicationByIdAsync(
                    It.Is<Guid>(y => y == medicationId)))
                .ReturnsAsync((Medication)null);

            // Act
            var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(
                () => this.getMedicationByIdQueryHandler.HandleAsync(query));

            // Assert
            Assert.IsTrue(ex.Message.Contains("Medication not found", StringComparison.InvariantCultureIgnoreCase));

            this.medicationReadRepositoryMock.Verify(
                x => x.GetMedicationByIdAsync(
                    It.Is<Guid>(y => y == medicationId)),
                Times.Once);
        }

        [TestMethod]
        public async Task GetMedicationByIdQueryHandler_HandleAsync_Throws()
        {
            // Arrange
            var medicationId = this.fixture.Create<Guid>();
            var medication = this.fixture.Build<Medication>()
                .With(x => x.Id, medicationId)
                .Create();

            var query = this.fixture.Build<GetMedicationByIdQuery>()
                .With(x => x.Id, medicationId)
                .Create();

            var exception = this.fixture.Create<Exception>();

            this.medicationReadRepositoryMock.Setup(
                x => x.GetMedicationByIdAsync(
                    It.Is<Guid>(y => y == medicationId)))
                .ThrowsAsync(exception);

            // Act
            var ex = await Assert.ThrowsExceptionAsync<Exception>(
                () => this.getMedicationByIdQueryHandler.HandleAsync(query));

            // Assert
            Assert.IsTrue(ex.Message.Contains(exception.Message, StringComparison.InvariantCultureIgnoreCase));

            this.medicationReadRepositoryMock.Verify(
                 x => x.GetMedicationByIdAsync(
                    It.Is<Guid>(y => y == medicationId)),
                Times.Once);
        }

    }
}
