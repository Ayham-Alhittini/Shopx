using Shopx.API.Entities;

namespace Shopx.API.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
