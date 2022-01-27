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
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "login succesfully" });
                }
                else if(login == 2)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "wrong password" });
                }
                else if (login == 3)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "wrong email" });
                }
                else
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Login is failed" });
                }
            }
            else
            {
                return StatusCode(505, new { status = HttpStatusCode.NotFound, message = "not found" });
            }
        }
    }
}
