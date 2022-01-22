using System;
using System.Linq;
using System.Net;
using API.Context;
using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private EmployeeRepository employeeRepository;

        public EmployeesController(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpPost]
        public IActionResult Post(Employee employee)
        {

            var result = employeeRepository.Insert(employee);
            switch (result)
            {
                case 1:
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "email already exists" });
                case 2:
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "number phone already exists" });
                case 3:
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "invalid e-mail address format" });
                case 4:
                    return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "record has been added" });
                case 5:
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "failed to add data" });
                default:
                    return StatusCode(500, new { status = HttpStatusCode.InternalServerError, result, message = "internal server error" });
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = employeeRepository.Get();

            if (result.Count() > 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "showing record" });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, result, message = "record is empty" });
            }
        }

        [HttpGet("{nik}")]
        public IActionResult GetByNIK(string nik)
        {
            var result = employeeRepository.Get(nik);

            if (result != null)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "showing record" });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, result, message = "record not found" });
            }
        }

        [HttpPut]
        public IActionResult Put(Employee employee)
        {
            var result = employeeRepository.Update(employee);
            switch (result)
            {
                case 1:
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "email cannot be empty" });
                case 2:
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "number phone cannot be empty" });
                case 3:
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "invalid e-mail address format" });
                case 4:
                    return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "record has been updated" });
                case 5:
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "failed to update record" });
                default:
                    return StatusCode(500, new { status = HttpStatusCode.InternalServerError, result, message = "internal server error" });
            }
        }

        [HttpDelete]
        public IActionResult Delete(string nik)
        {
            var result = employeeRepository.Delete(nik);

            switch(result)
            {
                case 1:
                    return StatusCode(404, new { status = HttpStatusCode.NotFound, result, message = "nik not found" });
                case 2:
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "record has been deleted" });
                case 3:
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, result, message = "failed to delete record" });
                default:
                    return StatusCode(500, new { status = HttpStatusCode.InternalServerError, result, message = "internal server error" });

            }
        }
    }

    // /cars?color=blue&type=sedan&doors=4
}


