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
            switch(result)
            {
                case 4:
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Phone is exist" });
                case 5:
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Email and phone are exist" });
                case 6:
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Email is exist" });
                case 7:
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data has been added" });
                case 8:
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Data cannot added" });
                default:
                    return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Internal server error" });
            }
            
        }


        [Route("{registeredData}")]
        //[HttpGet]
        public ActionResult GetRegisteredData()
        {
            var result = employeeRepository.GetRegisteredData();
            if (result != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data has been added" });
            }
            else
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Record is empty" });
            }
        }

    }
}




       