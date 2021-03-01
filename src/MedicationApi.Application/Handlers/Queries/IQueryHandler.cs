namespace MedicationApi.Application.Handlers.Queries
{
    using System.Threading.Tasks;

    public interface IQueryHandler<THandle, TReturn>
    {
        Task<TReturn> HandleAsync(THandle toHandle);
    }
}
