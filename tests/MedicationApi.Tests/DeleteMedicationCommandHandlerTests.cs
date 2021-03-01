namespace MedicationApi.Tests
{
    using System;
    using System.Threading.Tasks;
    using AutoFixture;
    using MedicationApi.Application.Exceptions;
    using MedicationApi.Application.Handlers.Commands;
    using MedicationApi.Data.Models.Commands;
    using MedicationApi.Data.Models.Entities;
    using MedicationApi.Repositories.Read;
    using MedicationApi.Repositories.Write;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DeleteMedicationCommandHandlerTests
    {
        private readonly Fixture fixture;

        private readonly Mock<IMedicationWriteRepository> medicationWriteRepositoryMock;

        private readonly Mock<IMedicationReadRepository> medicationReadRepositoryMock;

        private readonly Mock<ILogger<DeleteMedicationCommandHandler>> logMock;

        private readonly DeleteMedicationCommandHandler deleteMedicationCommandHandler;

        public DeleteMedicationCommandHandlerTests()
        {
            this.fixture = new Fixture();

            this.medicationWriteRepositoryMock = new Mock<IMedicationWriteRepository>();
            this.medicationReadRepositoryMock = new Mock<IMedicationReadRepository>();
            this.logMock = new Mock<ILogger<DeleteMedicationCommandHandler>>();

            this.deleteMedicationCommandHandler = new DeleteMedicationCommandHandler(
                this.medicationWriteRepositoryMock.Object,
                this.medicationReadRepositoryMock.Object,
                this.logMock.Object);
        }

        [TestMethod]
        public async Task DeleteMedicationCommandHandler_HandleAsync_ReturnsMedication()
        {
            // Arrange
            var medicationId = this.fixture.Create<Guid>();
            var medication = this.fixture.Build<Medication>()
                .With(x => x.Id, medicationId)
                .Create();

            var command = this.fixture.Build<DeleteMedicationCommand>()
                .With(x => x.Id, medicationId)
                .Create();

            this.medicationReadRepositoryMock.Setup(
                x => x.GetMedicationByIdAsync(It.Is<Guid>(y => y == medicationId)))
                .ReturnsAsync(medication);

            this.medicationWriteRepositoryMock.Setup(
                x => x.DeleteAsync(It.Is<Guid>(y => y == medicationId)))
                .Returns(Task.CompletedTask);

            // Act
            await this.deleteMedicationCommandHandler.HandleAsync(command);

            // Assert
            this.medicationReadRepositoryMock.Verify(
                x => x.GetMedicationByIdAsync(It.Is<Guid>(y => y == medicationId)),
                Times.Once);

            this.medicationWriteRepositoryMock.Verify(
                 x => x.DeleteAsync(It.Is<Guid>(y => y == medicationId)),
                 Times.Once);
        }

        [TestMethod]
        public async Task DeleteMedicationCommandHandler_HandleAsync_ThrowsNotFound()
        {
            // Arrange
            var medicationId = this.fixture.Create<Guid>();
            var exception = this.fixture.Create<NotFoundException>();

            var command = this.fixture.Build<DeleteMedicationCommand>()
                .With(x => x.Id, medicationId)
                .Create();

            this.medicationReadRepositoryMock.Setup(
                x => x.GetMedicationByIdAsync(It.Is<Guid>(y => y == medicationId)))
                .ThrowsAsync(exception);

            this.medicationWriteRepositoryMock.Setup(
                x => x.DeleteAsync(It.Is<Guid>(y => y == medicationId)))
                .Returns(Task.CompletedTask);

            // Act
            var ex = await Assert.ThrowsExceptionAsync<NotFoundException>(
                () => this.deleteMedicationCommandHandler.HandleAsync(command));

            // Assert
            Assert.IsTrue(ex.Message.Contains("Medication not found", StringComparison.InvariantCultureIgnoreCase));

            this.medicationReadRepositoryMock.Verify(
                 x => x.GetMedicationByIdAsync(
                    It.Is<Guid>(y => y == medicationId)),
                Times.Once);

            this.medicationWriteRepositoryMock.Verify(
                 x => x.DeleteAsync(It.Is<Guid>(y => y == medicationId)),
                 Times.Never);
        }

        [TestMethod]
        public async Task DeleteMedicationCommandHandler_HandleAsync_Throws()
        {
            // Arrange
            var medicationId = this.fixture.Create<Guid>();
            var exception = this.fixture.Create<Exception>();
            var medication = this.fixture.Build<Medication>()
                .With(x => x.Id, medicationId)
                .Create();

            var command = this.fixture.Build<DeleteMedicationCommand>()
                .With(x => x.Id, medicationId)
                .Create();

            this.medicationReadRepositoryMock.Setup(
                x => x.GetMedicationByIdAsync(It.Is<Guid>(y => y == medicationId)))
                .ReturnsAsync(medication);

            this.medicationWriteRepositoryMock.Setup(
                x => x.DeleteAsync(It.Is<Guid>(y => y == medicationId)))
                .ThrowsAsync(exception);

            // Act
            var ex = await Assert.ThrowsExceptionAsync<Exception>(
                () => this.deleteMedicationCommandHandler.HandleAsync(command));

            // Assert
            Assert.IsTrue(ex.Message.Contains(exception.Message, StringComparison.InvariantCultureIgnoreCase));

            this.medicationReadRepositoryMock.Verify(
                x => x.GetMedicationByIdAsync(It.Is<Guid>(y => y == medicationId)),
                Times.Once);

            this.medicationWriteRepositoryMock.Verify(
                x => x.DeleteAsync(It.Is<Guid>(y => y == medicationId)),
                Times.Once);
        }
    }
}
