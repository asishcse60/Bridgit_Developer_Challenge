using ScreechrDemo.Contracts.Model;
using ScreechrDemo.Databases.Dtos.Screechr;
using ScreechrDemo.Databases.Pagination;
using ScreechrDemo.Databases.Utils;

namespace ScreechrDemo.Databases.Data.Interface
{
    public interface IScreechRepositoryContext
    {
        Task<Screech> GetScreechByKeyAsync(ulong key);
        Task PostScreech(Screech screech);
        Task <Result>UpdateScreechAsync(ulong screechId, string content);
        Task<PagedResult<ScreechViewModel>> GetScreechesByUserId(ulong userId, PageModel filter);
        Task<PagedResult<ScreechViewModel>> GetScreeches(PageModel filter);

    }
}
