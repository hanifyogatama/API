using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository accountRepository;

        public AccountsController(AccountRepository accountRepository) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [HttpPost("Login")]
        public ActionResult<LoginVM> Post(LoginVM loginVM)
        {
            var login = accountRepository.Login(loginVM.Email, loginVM.Password);
            if(login != 0)
            {
                if(login == 1)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "account not registered" });
                }
                else if(login == 2)
                {
                    var getProfile = accountRepository.GetProfile(loginVM.Email);
                    return StatusCode(200, new { status = HttpStatusCode.OK, getProfile, message = "login succesfully" });
                }
                else if (login == 3)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "wrong password" });
                }
                else if (login == 4)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "wrong email" });
                }
                else
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "login is failed" });
                }
            }
            else
            {
                return StatusCode(505, new { status = HttpStatusCode.NotFound, message = "not found" });
            }
        }

        [HttpPost("ForgetPassword")]
        public ActionResult<ForgetPasswordVM> Post(ForgetPasswordVM forgetPasswordMV)
        {
            var forgotPassword = accountRepository.ForgotPassword(forgetPasswordMV.Email);
            if (forgotPassword != 0)
            {
                if (forgotPassword == 2)
                {
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "email sent successfully" });
                }
                else if (forgotPassword == 3)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "email cannot send" });
                }
                else if (forgotPassword == 4)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "email not registered" });
                }
                else
                {
                    return StatusCode(505, new { status = HttpStatusCode.NotFound, message = "not found" });
                }
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "internal server error" });
            }
        }

        [HttpPost("ChangePassword")]
        public ActionResult<ChangePasswordVM> Post(ChangePasswordVM changePasswordMV)
        {
            var changePassword = accountRepository.ChangePassword(changePasswordMV.Email, 
                changePasswordMV.OTP, changePasswordMV.NewPassword, changePasswordMV.ConfirmPassword);
            if (changePassword != 0)
            {
                if (changePassword == 5)
                {
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "password has been changed" });
                }
                else if (changePassword == 6)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "otp is used" });
                }
                else if (changePassword == 7)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "wrong confirm password" });
                }
                else if (changePassword == 8)
                {
                    return StatusCode(400, new { status = HttpStatusCode.NotFound, message = "otp is expired" });
                }
                else if (changePassword == 9)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "otp is wrong" });
                }
                else
                {
                    return StatusCode(505, new { status = HttpStatusCode.NotFound, message = "change password is failed" });
                }
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "internal server error"});
            }
        }
    }
}
