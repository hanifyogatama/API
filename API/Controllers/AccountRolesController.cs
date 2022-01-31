using API.Base;
using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountRolesController : BaseController<RoleAccount, AccountRoleRepository, string>
    {
        private readonly AccountRoleRepository accountRoleRepository;

        public AccountRolesController(AccountRoleRepository repository, IConfiguration configuration ) : base(repository)
        {
            this.accountRoleRepository = repository;  
        }

        [Authorize(Roles = "Director")]
        [HttpPost("SignManager/{Key}")]
        
        public ActionResult SignManager(string key)
        {
            var result = accountRoleRepository.SignManager(key);
            if(result != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "role has been changed to manager" });
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "failed to change role" });
            }
        }
    }
}
