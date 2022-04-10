using ScreechrDemo.Business.Core.Utils;
using ScreechrDemo.Contracts.Model;
using ScreechrDemo.Databases.Dtos.User;
using ScreechrDemo.Databases.Pagination;

namespace ScreechrDemo.Business.Core.Interface
{
    public interface IUserProfileService
    {
        Task<UserResultWrapper>AddUserAsync(UserModel user);
        Task<UserResultWrapper> GetUserProfileByKeyAsync(ulong key);
        Task<UserResultWrapper> UpdateProfilePictureAsync(ulong key, UpdateProfileImageModel model);
        Task<UserResultWrapper> UpdateProfileAsync(UpdateProfileModel updatedUser, ulong key);

        Task<PagedResult<UserProfileViewModel>> GetProfilesAsync(int pageNo, int pageSize);
        Task <UserResultWrapper>GetUserByUserNameAsync(string userName);
    }
}
