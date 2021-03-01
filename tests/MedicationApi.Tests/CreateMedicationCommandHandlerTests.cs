namespace MedicationApi.Tests
{
    using System;
    using System.Threading.Tasks;
    using AutoFixture;
    using MedicationApi.Application.Handlers.Commands;
    using MedicationApi.Data.Models.Commands;
    using MedicationApi.Data.Models.Entities;
    using MedicationApi.Repositories.Write;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class CreateMedicationCommandHandlerTests
    {
        private readonly Fixture fixture;

        private readonly Mock<IMedicationWriteRepository> medicationWriteRepositoryMock;

        private readonly Mock<ILogger<CreateMedicationCommandHandler>> logMock;

        private readonly CreateMedicationCommandHandler createMedicationCommandHandler;

        public CreateMedicationCommandHandlerTests()
        {
            this.fixture = new Fixture();

            this.medicationWriteRepositoryMock = new Mock<IMedicationWriteRepository>();
            this.logMock = new Mock<ILogger<CreateMedicationCommandHandler>>();

            this.createMedicationCommandHandler = new CreateMedicationCommandHandler(
                this.medicationWriteRepositoryMock.Object,
                this.logMock.Object);
        }

        [TestMethod]
        public async Task CreateMedicationCommandHandler_HandleAsync_ReturnsMedication()
        {
            // Arrange
            var medicationId = this.fixture.Create<Guid>();
            var medication = this.fixture.Build<Medication>()
                .With(x => x.Id, medicationId)
                .Create();

            var command = this.fixture.Build<CreateMedicationCommand>()
                .With(x => x.Medication, medication)
                .Create();

            this.medicationWriteRepositoryMock.Setup(
                x => x.InsertAsync(It.Is<Medication>(y => y.Id == medicationId)))
                .Returns(Task.CompletedTask);

            // Act
            await this.createMedicationCommandHandler.HandleAsync(command);

            // Assert
            this.medicationWriteRepositoryMock.Verify(
                 x => x.InsertAsync(It.Is<Medication>(y => y.Id == medicationId)),
                 Times.Once);
        }

        [TestMethod]
        public async Task CreateMedicationCommandHandler_HandleAsync_Throws()
        {
            // Arrange
            var medicationId = this.fixture.Create<Guid>();
            var exception = this.fixture.Create<Exception>();

            var medication = this.fixture.Build<Medication>()
                .With(x => x.Id, medicationId)
                .Create();

            var command = this.fixture.Build<CreateMedicationCommand>()
                .With(x => x.Medication, medication)
                .Create();

            this.medicationWriteRepositoryMock.Setup(
                x => x.InsertAsync(It.Is<Medication>(y => y.Id == medicationId)))
                .ThrowsAsync(exception);

            // Act
            var ex = await Assert.ThrowsExceptionAsync<Exception>(
                () => this.createMedicationCommandHandler.HandleAsync(command));

            // Assert
            Assert.IsTrue(ex.Message.Contains(exception.Message, StringComparison.InvariantCultureIgnoreCase));

            this.medicationWriteRepositoryMock.Verify(
                x => x.InsertAsync(It.Is<Medication>(y => y.Id == medicationId)),
                Times.Once);
        }
    }
}
