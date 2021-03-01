namespace MedicationApi.Application.Handlers.Commands
{
    using System.Threading.Tasks;

    public interface ICommandHandler<T>
    {
        Task HandleAsync(T toHandle);
    }
}
