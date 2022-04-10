using ScreechrDemo.Security.Auth;

namespace ScreechrDemo.Security.@interface;

public interface IScreechUserAuthenticationManager
{
    Task<AccessResult> ValidateToken(string userToken);
}