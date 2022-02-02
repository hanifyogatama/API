using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController <Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository employeeRepository;
        private readonly IConfiguration iConfiguration;
        public EmployeesController(EmployeeRepository employeeRepository, IConfiguration iConfiguration) : base(employeeRepository)
        {
           this.employeeRepository = employeeRepository;    
           this.iConfiguration = iConfiguration;    
        }

        [HttpPost("Register")]
        public ActionResult<RegisterVM> Post(RegisterVM registerVM)
        {
            var result = employeeRepository.Register(registerVM);
            if (result != 0)
            {
                if (result == 5)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "email is exist" });
                }
                else if (result == 6)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "phone is exist" });
                }
                else if (result == 7)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "email and phone are exist" });
                }
                else
                {
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "account registration successful" });
                }
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "failed" });
            }
        }

        [Authorize(Roles = "Director, Manager")]
        [HttpGet("registeredData")]
        //[HttpGet]
        public ActionResult GetRegisteredData()
        {
            var result = employeeRepository.GetRegisteredData();
            if (result != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result , message = "showing data" });
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest,result, message = "Record is empty" });
            }
        }


        [HttpGet("TestCORS")]
        public ActionResult TestCORS()
        {
            return Ok("test cors berhasil");
        }


    }
}




       