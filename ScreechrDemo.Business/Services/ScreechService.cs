using ScreechrDemo.Business.Core.Interface;
using ScreechrDemo.Business.Core.Utils;
using ScreechrDemo.Contracts.Model;
using ScreechrDemo.Databases.Data.Interface;
using ScreechrDemo.Databases.Dtos.Screechr;
using ScreechrDemo.Databases.Pagination;
using ScreechrDemo.Databases.Utils;

namespace ScreechrDemo.Business.Core.Services
{
    public class ScreechService : IScreechService
    {
        private readonly IScreechRepositoryContext _screechRepository;

        public ScreechService(IScreechRepositoryContext screechRepository)
        {
            _screechRepository = screechRepository;
        }
        public async Task<ScreechWrapperResult> AddScreechAsync(ScreechModel model)
        {
            var screech = new Screech()
            {
                UserProfileId = model.CreatorId,
                Content = model.Content
            };
            await _screechRepository.PostScreech(screech);
            return new ScreechWrapperResult()
            {
                Result = Result.SUCCESS,
                Screech = screech
            };
        }

        public async Task<ScreechWrapperResult> UpdateScreechrContentByIdAsync(ulong id, UpdateScreechModel model)
        {
            var res = await _screechRepository.UpdateScreechAsync(id, model.Text);
            var sResult = new ScreechWrapperResult
            {
                Result = res
            };
            return sResult;
        }

        public async Task<ScreechWrapperResult> GetScreechrByIdAsync(ulong id)
        {
            var screech = await _screechRepository.GetScreechByKeyAsync(id);
            var sResult = new ScreechWrapperResult();
           
            if (screech == null)
            {
                sResult.Result = Result.Screech_NOT_FOUND;
            }
            else
            {
                sResult.Result = Result.SUCCESS;
                sResult.Screech = screech;
            }

            return sResult;
        }

        public PagedResult<ScreechViewModel> GetAllScreechAsync(PageModel filter)
        {
            PagedResult<ScreechViewModel> viewModels;
            if (filter.UserId != null)
            {
                viewModels = GetScreechesByUserIdAsync(filter.UserId.Value, filter);
            }
            else
            {
                viewModels = _screechRepository.GetScreeches(filter).Result;

            }
            return viewModels;
        }

        public PagedResult<ScreechViewModel> GetScreechesByUserIdAsync(ulong userId, PageModel filter)
        {
            var result = _screechRepository.GetScreechesByUserId(userId, filter).Result;
            return result;
        }
    }
}
