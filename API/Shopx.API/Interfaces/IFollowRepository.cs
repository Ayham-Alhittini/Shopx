using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Helper;

namespace Shopx.API.Interfaces
{
    public interface IFollowRepository
    {
        Task<Follows> GetUserFollow(string sourceId, string targetUserId);
        Task<AppUser> GetUserWithFollows(string userId);
        Task<PagedList<FollowDto>> GetUserFollows(FollowParams followParams);
    }
}
