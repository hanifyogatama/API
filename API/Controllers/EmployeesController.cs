using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController <Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository employeeRepository; 
        public EmployeesController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
           this.employeeRepository = employeeRepository;    
        }

        [HttpPost("Register")]
        public ActionResult<RegisterVM> Post(RegisterVM registerVM)
        {
            var result = employeeRepository.Register(registerVM);

            if (result != 0)
            {
                if (result == 4)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "phone is exist" });
                }
                else if (result == 5)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "email and phone are exist" });
                }
                else if (result == 6)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "email is exist" });
                }
                else
                {
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "data has been added" });
                }
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "data failed to added" });
            }
        }
    }
}




       