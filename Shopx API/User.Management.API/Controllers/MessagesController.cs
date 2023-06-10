using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Extensions;
using Shopx.API.Helper;
using Shopx.API.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Shopx.API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IMessagesRepository _messagesRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public MessagesController(IMessagesRepository messagesRepository,
            UserManager<AppUser> userManager, IMapper mapper, IProductRepository productRepository)
        {
            _messagesRepository = messagesRepository;
            _userManager = userManager;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        [HttpPost("send-message")]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var sender = await _userManager.FindByNameAsync(User.GetUsername());
            var recipenet = await _userManager.FindByNameAsync(createMessageDto.RecipenetUsername);

            var product = await _productRepository.GetProductByIdAsync(createMessageDto.ProductId);

            if (product == null)
            {
                return NotFound("Product Not Found");
            }
            if (recipenet == null)
            {
                return NotFound("Recipenet not exist");
            }

            /*
             (Validation)
               1 - only seller account and customer account could chat 
               2 - the seller should have the product that they chat about
             */
            if (sender.AccountType == "Seller")
            {
                if (recipenet.AccountType != "Customer")
                    return BadRequest("Chat between seller and customer only.");


                ///could happen if this seller try to send message for customer within a product 
                ///that he does not have
                if (product.SellerId != sender.Id)
                    return BadRequest("You don't have such a product");
            }
            else if (recipenet.AccountType == "Seller")
            {
                if (sender.AccountType != "Customer")
                    return BadRequest("Chat between seller and customer only.");

                ///if a customer send to wrong shop 
                ///since this should does not have the required product
                if (product.SellerId != recipenet.Id)
                    return BadRequest("this seller does not have the required product");
            }
            else
            {
                return BadRequest("Chat between seller and customer only.");
            }



            ///last chat between seller and customer about this product

            var message = new Message
            {
                Sender = sender,
                Recipenet = recipenet,
                SenderUsername = sender.UserName,
                RecipenetUsername = recipenet.UserName,
                Content = createMessageDto.Content,
                ProductId = createMessageDto.ProductId,
            };

            ///since you send message so you should be reading all text from the user you texting
            

            await _messagesRepository.AddMessage(message);

            return Ok();
        }

        [HttpGet("get-messages")]
        public async Task<ActionResult> GetMessagesView()
        {
            return Ok(await _messagesRepository.GetMessageView(User.GetUsername()));
        }

        [Authorize(Roles = "Seller")]
        [HttpGet("get-product-messages/{productId}")]
        public async Task<ActionResult> GetProductMessages(int productId)
        {
            ///check if seller has this product
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product.SellerId != User.GetUserId())
            {
                return BadRequest("You don't own this product");
            }
            return Ok(await _messagesRepository.GetProductMessages(productId, User.GetUsername()));
        }
        [HttpGet("thread/{username}/{id}")]
        public async Task<ActionResult> GetMessageThread(string username, int id)
        {
            string curentName = User.GetUsername();


            if (await _userManager.FindByNameAsync(username) != null)
            {
                return Ok(await _messagesRepository.GetMessageThread(curentName, username, id));
            }
            return NotFound();
        }

        [HttpDelete("delete-message")]
        public async Task<ActionResult> DeleteMessage([Required]string recipenetName,[Required] int productId)
        {
            var currentUsername = User.GetUsername();
            var message = await _messagesRepository.GetLastMessage(currentUsername, recipenetName, productId);

            if (message == null)
                return NotFound();


            if (message.SenderUsername == currentUsername)
            {
                if (message.RecipenetUsername != recipenetName)
                    return NotFound();

                message.SenderDeleted = true;
            }
            else if (message.RecipenetUsername == currentUsername)
            {
                if (message.SenderUsername.ToLower() != recipenetName.ToLower())
                    return NotFound();

                message.RecipenetDeleted = true;
            }
            else
                return NotFound("there is no chat to delete");

            return Ok();
        }
    }
}
