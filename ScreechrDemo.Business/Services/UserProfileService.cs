using ScreechrDemo.Business.Core.Interface;
using ScreechrDemo.Business.Core.Utils;
using ScreechrDemo.Contracts.Model;
using ScreechrDemo.Databases.Data.Interface;
using ScreechrDemo.Databases.Dtos.User;
using ScreechrDemo.Databases.Pagination;
using ScreechrDemo.Databases.Utils;

namespace ScreechrDemo.Business.Core.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepositoryContext _userProfileRepository;
        public UserProfileService(IUserProfileRepositoryContext userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }
        public async Task<UserResultWrapper> AddUserAsync(UserModel user)
        {
            var userProfile = new UserProfile()
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                SecretToken = user.SecretToken,
                ProfileImageUri = user.ProfileImageUrl
            };
            await _userProfileRepository.AddUserProfileAsync(userProfile);
            return new UserResultWrapper()
            {
                ResultStatus = Result.SUCCESS,
                UserProfile = userProfile
            };
        }

        public async Task<UserResultWrapper> GetUserProfileByKeyAsync(ulong key)
        {
            var uresultWrapper = new UserResultWrapper();
            var res = await _userProfileRepository.GetUserProfileByKeyAsync(key);
            if (res == null)
            {
                uresultWrapper.ResultStatus = Result.PROFILE_NOT_FOUND;
            }
            else
            {
                uresultWrapper.ResultStatus = Result.SUCCESS;
                uresultWrapper.UserProfile = res;
            }

            return uresultWrapper;
        }

        public async Task<UserResultWrapper> UpdateProfilePictureAsync(ulong key, UpdateProfileImageModel model)
        {
            var res = await _userProfileRepository.UpdateProfilePictureAsync(key, model.ProfileImageUri);
            var uresultWrapper = new UserResultWrapper();
            if (res == Result.PROFILE_NOT_FOUND)
            {
                uresultWrapper.ResultStatus = Result.PROFILE_NOT_FOUND;
            }
            else
            {
                uresultWrapper.ResultStatus = Result.SUCCESS;
            }

            return uresultWrapper;
        }

        public async Task<UserResultWrapper> UpdateProfileAsync(UpdateProfileModel updatedUser, ulong key)
        {
            var res = await _userProfileRepository.UpdateProfileAsync(updatedUser, key);
            var uresultWrapper = new UserResultWrapper();
            if (res == Result.PROFILE_NOT_FOUND)
            {
                uresultWrapper.ResultStatus = Result.PROFILE_NOT_FOUND;
            }
            else
            {
                uresultWrapper.ResultStatus = Result.SUCCESS;
            }

            return uresultWrapper;
        }

        public async Task<PagedResult<UserProfileViewModel>> GetProfilesAsync(int pageNo, int pageSize)
        {
            return await _userProfileRepository.GetProfilesAsync(pageNo, pageSize);
        }

        public async Task<UserResultWrapper> GetUserByUserNameAsync(string userName)
        {
            var user = await _userProfileRepository.GetUserByNameAsync(userName);
            if (user == null)
            {
                return new UserResultWrapper() { ResultStatus = Result.PROFILE_NOT_FOUND };
            }

            return new UserResultWrapper
            {
                ResultStatus = Result.SUCCESS,
                UserProfile = user
            };
        }
    }

}
