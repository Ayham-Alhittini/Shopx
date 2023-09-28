using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Extensions;
using Shopx.API.Interfaces;

namespace Shopx.API.Controllers
{
    [Authorize]
    public class NotificationController: BaseApiController
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        public NotificationController(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        [HttpGet("get-notifications")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotification()
        {
            var notification = await _notificationRepository.GetAllNotifications(User.GetUserId());


            return Ok(_mapper.Map<IEnumerable<NotificationDto>>(notification));
        }

        [HttpPost("read-notification/{notificationId}")]
        public async Task<ActionResult> ShowNotification(int notificationId)
        {
            var notification = await _notificationRepository.GetNotification(notificationId);

            if (notification == null)
                return NotFound("Notification not exist");

            if (notification.UserId != User.GetUserId())
                return Unauthorized("This notification not belong to you");

            notification.Read = true;

            await _notificationRepository.SaveChanges();

            return Ok();
        }
    }
}
