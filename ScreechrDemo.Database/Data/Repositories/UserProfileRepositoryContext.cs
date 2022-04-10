using Microsoft.EntityFrameworkCore;
using ScreechrDemo.Contracts.Model;
using ScreechrDemo.Databases.Data.Interface;
using ScreechrDemo.Databases.Dtos.User;
using ScreechrDemo.Databases.Pagination;
using ScreechrDemo.Databases.Utils;

namespace ScreechrDemo.Databases.Data.Repositories
{

    public class UserProfileRepositoryContext : DbContext, IUserProfileRepositoryContext
    {

        public DbSet<UserProfile>UserProfile { get; set; }
        public UserProfileRepositoryContext()
        {

        }
        public UserProfileRepositoryContext(DbContextOptions<UserProfileRepositoryContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserProfile>().HasIndex(p => new { p.UserName }).IsUnique(true);
            modelBuilder.Entity<UserProfile>().Property(m => m.ProfileImageUri).IsRequired(false);

        }


        public async Task AddUserProfileAsync(UserProfile user)
        {
            await UserProfile.AddAsync(user);
            await SaveChangesAsync();
        }

        public async Task<UserProfile> GetUserProfileByKeyAsync(ulong key)
        {
            return await UserProfile.FirstOrDefaultAsync(k => k.Id == key);
        }

        public async Task<Result> UpdateProfilePictureAsync(ulong key, string uri)
        {
            var userProfile = await GetUserProfileByKeyAsync(key);
            if (userProfile == null)
            {
                return Result.PROFILE_NOT_FOUND;
            }
            userProfile.ProfileImageUri = uri;
            
            UserProfile.Update(userProfile);
            await SaveChangesAsync();

            return Result.SUCCESS;
        }

        public async Task<Result> UpdateProfileAsync(UpdateProfileModel updatedUser, ulong key)
        {
            var existProfile = await GetUserProfileByKeyAsync(key);
            if (existProfile == null)
            {
                return Result.PROFILE_NOT_FOUND;
            }

            existProfile.ProfileImageUri = updatedUser.ProfileImageUri;
            existProfile.ModifiedDate = DateTime.UtcNow;
            existProfile.FirstName = updatedUser.FirstName;
            existProfile.LastName = updatedUser.LastName;

            UserProfile.Update(existProfile);
            await SaveChangesAsync();

            return Result.SUCCESS;
        }
       

        public Task<PagedResult<UserProfileViewModel>> GetProfilesAsync(int pageNo, int pageSize)
        {
            var pageResults = PageResultExtension.GetPaged(UserProfile.OrderBy(u => u.FirstName), pageNo, pageSize, TransformResult);
            return Task.FromResult(pageResults);
        }

        public Task<UserProfile> GetUserByNameAsync(string userName)
        {
            return Task.FromResult(UserProfile.FirstOrDefault(user => user.UserName.ToLower().Equals(userName.ToLower())));
        }

        #region private method for pagination

        private UserProfileViewModel TransformResult(UserProfile user)
        {
            var vModel = new UserProfileViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImageUri = user.ProfileImageUri,
                CreatedAt = user.CreatedAt,
                ModifiedDate = user.ModifiedDate
            };
            return vModel;
        }

        #endregion
    }

    
}
