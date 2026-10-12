using ServicesSwF.Helper.Application.DTO;

namespace ServicesSwF.Infrastructure.Data.Repositories
{
    public interface IRepository
    {
        Task<ResponseGenericDTO> InsertAsync(IList<RequestGenericDTO> request, string storedProcedure);
        Task<ResponseGenericDTO> UpdateAsync(IList<RequestGenericDTO> request, string storedProcedure);
        Task<ResponseGenericDTO> ReadAsync<T>(IList<RequestGenericDTO> request, string storedProcedure);
        Task<ResponseGenericDTO> ReadAsyncByQuery<T>(IList<RequestGenericDTO> request, string query);
    }
}
