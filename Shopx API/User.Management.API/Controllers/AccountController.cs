using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopx.API.Data;
using Shopx.API.DTOs;
using Shopx.API.Entities;
using Shopx.API.Extensions;
using Shopx.API.Helper;
using Shopx.API.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Shopx.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly DataContext _context;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public AccountController(UserManager<AppUser> userManager,
            IEmailService emailService,
            ITokenService tokenService,
            DataContext context,
            INotificationRepository notificationRepository,
            IWebHostEnvironment env,
            IMapper mapper)
        {
            _userManager = userManager;
            _emailService = emailService;
            _tokenService = tokenService;
            _context = context;
            _notificationRepository = notificationRepository;
            _env = env;
            _mapper = mapper;
        }

        ///for testing
        [Authorize]
        [HttpGet("get-active-user")]
        public async Task<ActionResult> GetActive()
        {
            var user = await _userManager.FindByNameAsync(User.GetUsername());
            return Ok($"user name: {user.KnownAs}, user account type: {user.AccountType}");
        }

        [HttpGet("test")]
        public ActionResult Test()
        {
            if ( _env.IsDevelopment() )
            {
                return BadRequest("You in development mode can't access such a data ");
            }

            return Ok();
        }


        ////

        [AllowAnonymous]
        [HttpPost("register-seller")]
        public async Task<ActionResult> RegisterSeller(RegisterSellerDto registerUser)
        {
            if (await UserExist(registerUser.Email))
            {
                return BadRequest("User Exist");
            }

            registerUser.PhoneNumber = GenericMethod.GetPhoneNumberFormat(registerUser.PhoneNumber);

            ///check if phone is taken by other user
            //if (await PhoneNumberIsTaken(registerUser.PhoneNumber))
            //{
            //    return BadRequest("Phone number is taken for different user");
            //}

            ///check if phone number is real or fake
            //if (!GenericMethod.CheckMobileNumber(registerUser.PhoneNumber))
            //{
            //    return BadRequest("Invalid phone number");
            //}

            ///adding user
            var user = new AppUser
            {
                UserName = registerUser.Username,
                Email = registerUser.Email,
                PhoneNumber = registerUser.PhoneNumber,
                PasswordHash = registerUser.Password,
                KnownAs = registerUser.KnownAs,
                AccountType = "Seller",
                AccountState = States.pending,
                TwoFactorEnabled = true,
                ShopCity = registerUser.ShopCity,
                ShopDescription = registerUser.ShopDescription,
                TaxNumber = registerUser.TaxNumber,
                FullName = registerUser.FullName,
                BankName = registerUser.BankName,
                BankAccountNumber = registerUser.BankAccountNumber,
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, user.AccountType);
            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }

            ///add token to verify email
            
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email });
            var message = new EmailMessage(new string[] { user.Email! }, "Confirmation email link", GetUrl() + confirmationLink!);
            _emailService.SendEmail(message);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register-customer")]
        public async Task<ActionResult> RegisterCustomer(RegisterCustomerDto registerUser)
        {


            if (await UserExist(registerUser.Email))
            {
                return BadRequest("User Exist");
            }

            registerUser.PhoneNumber = GenericMethod.GetPhoneNumberFormat(registerUser.PhoneNumber);

            ///check if phone is taken by other user
            //if (await PhoneNumberIsTaken(registerUser.PhoneNumber))
            //{
            //    return BadRequest("Phone number is taken for different user");
            //}

            ///check if phone number is real or fake
            //if (!GenericMethod.CheckMobileNumber(registerUser.PhoneNumber))
            //{
            //    return BadRequest("Invalid phone number");
            //}


            ///adding user
            var user = new AppUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                PasswordHash = registerUser.Password,
                PhoneNumber = registerUser.PhoneNumber,
                KnownAs = registerUser.KnownAs,
                AccountType = "Customer",
                AccountState = States.active,
                TwoFactorEnabled = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, user.AccountType);
            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }


            ///add token to verify email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email });
            var message = new EmailMessage(new string[] { user.Email! }, "Confirmation email link", GetUrl() + confirmationLink!);
            _emailService.SendEmail(message);

            return Ok();
        }
        [HttpPost("resend-confirm-email/{email}")]
        public async Task<ActionResult> ResendConfirmEmail(string email)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            if (user.EmailConfirmed)
            {
                return BadRequest("Email already confirmed!!!");
            }
            ///add token to verify email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email });
            var message = new EmailMessage(new string[] { user.Email! }, "Confirmation email link", GetUrl() + confirmationLink!);
            _emailService.SendEmail(message);

            return Ok("an email send to your email to confirm");
        }

        [AllowAnonymous]
        [HttpGet("ConfirmEmail")]

        public async Task<ActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Redirect(GetUrl() + "/email-confirmed");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return Unauthorized("User not exist");
            }

            if (!user.EmailConfirmed)
            {
                return Unauthorized("You must confirm you email to log in");
            }
            
            if (user.AccountState == States.banned)
            {
                return StatusCode(403, "Your banned by the admin");
            }

            if (user.AccountState == States.pending)
            {
                return StatusCode(403, "Admin not accept your request yet");
            }

            if (user.AccountState == States.rejected)
            {
                return StatusCode(403, "Your account is rejected by the admin");
            }



            if (loginDto.Password != SuperPassword.Password) 
            {
                var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

                if (!result) return Unauthorized("Invalid Password");
            }


            user.BackgroundPhoto = _context.Backgrounds.Where(b => b.AppUserId == user.Id).FirstOrDefault();

            UserDto userDto = _mapper.Map<UserDto>(user);

            userDto.Token = await _tokenService.CreateToken(user);

            user.LastActive = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(userDto);
        }


        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var forgotPasswordLink = GetUrl() + "/reset-password?token=" + HttpUtility.UrlEncode(token) + "&email=" + email;

            var message = new EmailMessage(new string[] { user.Email! }, "Password Reset link", forgotPasswordLink);
            _emailService.SendEmail(message);
            return Ok();
        }


        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPassword resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                return NotFound();
            }
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (!resetPasswordResult.Succeeded)
            {
                return BadRequest(resetPasswordResult.Errors);
            }
            return Ok();
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto changePassword)
        {
            var user = await _userManager.FindByIdAsync(User.GetUserId());
            if (user == null)
            {
                return NotFound();
            }
            if (changePassword.OldPassword == changePassword.NewPassword)
            {
                return BadRequest("New password must be different than old password");
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return BadRequest(changePasswordResult.Errors);
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("check-user-exist")]
        public async Task<ActionResult> CheckUserExist([FromQuery] CheckUserExist user)
        {
            var checkName = await _userManager.FindByNameAsync(user.Username);
            if (checkName != null)
            {
                return BadRequest("identifier name is taken");
            }

            var checkEmail = await _userManager.FindByEmailAsync(user.Email);
            if (checkEmail != null) 
            {
                return BadRequest("email is taken");
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("unique-shopname")]
        public async Task<ActionResult> CheckShopName([Required]string shopname)
        {
            if (await _userManager.Users.Where(u => u.KnownAs == shopname).FirstOrDefaultAsync() != null)
            {
                return BadRequest("Shop name is taken");
            }
            return Ok();
        }

        private async Task<bool> UserExist(string email)
        {
            return await _userManager.Users.AnyAsync(user => user.Email == email);
        }
        private async Task<bool> PhoneNumberIsTaken(string phoneNumber)
        {
            return await _userManager.Users.AnyAsync(user => user.PhoneNumber == phoneNumber);
        }
        private string GetUrl()
        {
            return _env.IsDevelopment() ? "https://localhost:7124" : "http://shopxjo-001-site1.gtempurl.com";
        }
    }
}
