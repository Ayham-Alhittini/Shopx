using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shopx.API.Data.Repository;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Extensions;
using Shopx.API.Helper;
using Shopx.API.Interfaces;

namespace Shopx.API.Controllers
{
    public class FollowController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IFollowRepository _followRepository;
        public FollowController(IUserRepository userRepository, IFollowRepository followRepository) 
        {
            _userRepository = userRepository;
            _followRepository = followRepository;
        }


        [Authorize(Roles = "Customer")]
        [HttpPost("{username}")]
        public async Task<ActionResult<string>> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var sourceUser = await _followRepository.GetUserWithFollows(sourceUserId);


            var targetUser = await _userRepository.GetUserByNameAsync(username);

            if (targetUser == null)
            {
                return NotFound();
            }

            if (targetUser.AccountType != "Seller")
            {
                return BadRequest("You can only follow shops");
            }

            ///check if it's already follow that shop

            var userFollow = await _followRepository.GetUserFollow(sourceUserId, targetUser.Id);

            if (userFollow != null)
            {
                ///remove like
                sourceUser.Following.Remove(userFollow);
                if (await _userRepository.SaveAllAsync())
                {
                    return Ok(JsonConvert.SerializeObject("removed"));
                }
                return BadRequest("Failde to remove follow for this shop !!!");
            }

            /// add user process

            userFollow = new Follows
            {
                SourceUserId = sourceUserId,
                TargetUserId = targetUser.Id
            };

            sourceUser.Following.Add(userFollow);

            if (await _userRepository.SaveAllAsync())
            {
                return Ok(JsonConvert.SerializeObject("added"));
            }


            return BadRequest("Failed to follow user!!");
        }



        [HttpGet]
        public async Task<ActionResult<PagedList<FollowDto>>> GetLikesUser([FromQuery] FollowParams followParams)
        {
            followParams.UserId = User.GetUserId();

            var users = await _followRepository.GetUserFollows(followParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

            return Ok(users);
        }
    }
}
