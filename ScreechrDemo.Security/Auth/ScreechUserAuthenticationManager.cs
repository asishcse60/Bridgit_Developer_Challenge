using System.Text;
using ScreechrDemo.Business.Core.Interface;
using ScreechrDemo.Databases.Utils;
using ScreechrDemo.Security.@interface;

namespace ScreechrDemo.Security.Auth
{
    public class ScreechUserAuthenticationManager : IScreechUserAuthenticationManager
    {
        private readonly IUserProfileService _userProfileService;

        public ScreechUserAuthenticationManager(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }
        public async Task<AccessResult> ValidateToken(string userToken)
        {
            if (string.IsNullOrEmpty(userToken))
                return new AccessResult { IsAuthenticated = false };

            var secret = DecodeToken(userToken);
            var tokenParts = secret.Split(':');
            if (tokenParts?.Length == 2)
            {
                return await GetAuthenticateUser(userName: tokenParts[0], secret: tokenParts[1]);
            }

            return new AccessResult { IsAuthenticated = false };
        }

        #region Private method
        private static string DecodeToken(string tokenValue)
        {
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(tokenValue);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch { }
            return "";
        }
        private async Task<AccessResult> GetAuthenticateUser(string userName, string secret)
        {
            var result = await _userProfileService.GetUserByUserNameAsync(userName);
            if (result.ResultStatus != Result.SUCCESS)
                return new AccessResult { IsAuthenticated = false };

            if (result.UserProfile.SecretToken.ToLower().Equals(secret))
            {
                return new AccessResult { IsAuthenticated = true, Id = result.UserProfile.Id, UserName = string.Join(result.UserProfile.FirstName, "  ", result.UserProfile.LastName) };
            }

            return new AccessResult { IsAuthenticated = false };
        }

        #endregion

    }
}
