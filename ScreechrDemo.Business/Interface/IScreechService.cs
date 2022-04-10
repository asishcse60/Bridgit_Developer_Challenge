using ScreechrDemo.Business.Core.Utils;
using ScreechrDemo.Contracts.Model;
using ScreechrDemo.Databases.Pagination;

namespace ScreechrDemo.Business.Core.Interface
{
    public interface IScreechService
    {
        Task <ScreechWrapperResult>AddScreechAsync(ScreechModel model);
        Task <ScreechWrapperResult> UpdateScreechrContentByIdAsync(ulong id, UpdateScreechModel model);
        Task<ScreechWrapperResult> GetScreechrByIdAsync(ulong id);
        PagedResult<ScreechViewModel> GetAllScreechAsync(PageModel filter);
        PagedResult<ScreechViewModel> GetScreechesByUserIdAsync(ulong userId, PageModel filter);
    }
}
