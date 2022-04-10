using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using ScreechrDemo.Contracts.Constants;
using ScreechrDemo.Contracts.Model;
using ScreechrDemo.Databases.Data.Interface;
using ScreechrDemo.Databases.Dtos.Screechr;
using ScreechrDemo.Databases.Pagination;
using ScreechrDemo.Databases.Utils;
using System.Configuration;

namespace ScreechrDemo.Databases.Data.Repositories
{

    public class ScreechRepositoryContext : DbContext, IScreechRepositoryContext
    {
        public DbSet<Screech> Screech { get; set; }

        public ScreechRepositoryContext()
        {

        }
        public ScreechRepositoryContext(DbContextOptions<ScreechRepositoryContext> options) :base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var adminPanelConnectionString = "Data Source=LION\\TEST;Initial Catalog=OrbitaxAdminPanelCore;user id=sa;password=OrbiDB20@13;Integrated Security=True;";
            //var conn = ConfigurationManager.ConnectionStrings["ScreechDB"].ConnectionString;
           
           // optionsBuilder.UseSqlServer(conn);
           // optionsBuilder.EnableDetailedErrors();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public async Task<Screech> GetScreechByKeyAsync(ulong key)
        {
            return await Screech.FirstOrDefaultAsync(k => k.Id == key);
        }

        public async Task PostScreech(Screech screech)
        {
            await Screech.AddAsync(screech);
            await SaveChangesAsync();
        }

        public async Task<Result>UpdateScreechAsync(ulong screechId, string content)
        {
            var existScreech = await GetScreechByKeyAsync(screechId);
            if (existScreech == null)
            {
                return Result.Screech_NOT_FOUND;
            }

            existScreech.Content = content;


            Screech.Update(existScreech);
            await SaveChangesAsync();

            return Result.SUCCESS;
        }

        public Task<PagedResult<ScreechViewModel>> GetScreechesByUserId(ulong userId, PageModel pageModel)
        {
            PagedResult<ScreechViewModel> pageResult = new PagedResult<ScreechViewModel>();

            var pageResults = PageResultExtension.GetPaged(Screech.Where(s => s.UserProfileId == userId).OrderByDescending(o=>o.CreatedAt)
             , pageModel.PageNo.Value, pageModel.PageSize.Value, TransformResult);
            if (pageModel.SortDir == FieldLimit.SortAsc)
            {
                var res = pageResults.Results.OrderBy(o => o.CreatedAt).ToList();
                pageResult.Results = res;
                return Task.FromResult(pageResult);
            }
            return Task.FromResult(pageResults);

        }

       
        public Task<PagedResult<ScreechViewModel>> GetScreeches(PageModel pageModel)
        {
            PagedResult<ScreechViewModel> pageResult = new PagedResult<ScreechViewModel>();
            var pageResults = PageResultExtension.GetPaged(Screech.OrderByDescending(o => o.CreatedAt)
                , pageModel.PageNo.Value, pageModel.PageSize.Value, TransformResult);
           
            if (pageModel.SortDir == FieldLimit.SortAsc)
            {
                var res = pageResults.Results.OrderBy(o => o.CreatedAt).ToList();
                pageResult.Results = res;
                return Task.FromResult(pageResult);
            }

            return Task.FromResult(pageResults);
        }

        #region Private Method

        private ScreechViewModel TransformResult(Screech screech)
        {
            var sViewModel = new ScreechViewModel
            {
                Id = screech.Id,
                content = screech.Content,
                CreatedAt = screech.CreatedAt
            };
            return sViewModel;
        }


        #endregion
    }
}
