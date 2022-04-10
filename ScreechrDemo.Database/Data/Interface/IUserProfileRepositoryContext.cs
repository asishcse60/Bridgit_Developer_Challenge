using ScreechrDemo.Contracts.Model;
using ScreechrDemo.Databases.Dtos.User;
using ScreechrDemo.Databases.Pagination;
using ScreechrDemo.Databases.Utils;

namespace ScreechrDemo.Databases.Data.Interface
{
    public interface IUserProfileRepositoryContext : IDisposable
    {
        Task AddUserProfileAsync(UserProfile user);
        Task <UserProfile> GetUserProfileByKeyAsync(ulong key);
        Task <Result>UpdateProfilePictureAsync(ulong key, string uri);
        Task <Result>UpdateProfileAsync(UpdateProfileModel updatedUser, ulong key);

        Task<PagedResult<UserProfileViewModel>> GetProfilesAsync(int pageNo, int pageSize);

        Task<UserProfile> GetUserByNameAsync(string userName);
    }
}
