using Microsoft.EntityFrameworkCore;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Helper;
using Shopx.API.Interfaces;

namespace Shopx.API.Data.Repository
{
    public class FollowRepository : IFollowRepository
    {
        private readonly DataContext _context;
        public FollowRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Follows> GetUserFollow(string sourceId, string targetUserId)
        {
            return await _context.Follows.FindAsync(sourceId, targetUserId);
        }

        public async Task<PagedList<FollowDto>> GetUserFollows(FollowParams followParams)
        {
            var users = _context.Users.AsQueryable();
            var follows = _context.Follows.AsQueryable();

            if (followParams.Predicate == "following")
            {
                follows = follows.Where(l => l.SourceUserId == followParams.UserId);
                users = follows.Select(x => x.TargetUser);
            }
            if (followParams.Predicate == "followers")
            {
                follows = follows.Where(l => l.TargetUserId == followParams.UserId);
                users = follows.Select(x => x.SourceUser);
            }

            var source = users.Select(user => new FollowDto
            {
                Id = user.Id,
                Email = user.Email,
                KnownAs = user.KnownAs,
                PhotoUrl = user.BackgroundPhoto.Url
            });

            var result = await PagedList<FollowDto>.CreateAsync(source, followParams.PageNumber, followParams.PageSize);


            return result;
        }

        public async Task<AppUser> GetUserWithFollows(string userId)
        {
            return await _context.Users.Include(u => u.Following).FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
