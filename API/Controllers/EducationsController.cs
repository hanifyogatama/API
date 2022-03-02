using API.Base;
using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationsController : BaseController<Education, EducationRepository, int>
    {
        private readonly EducationRepository _educationRepository;
        public EducationsController(EducationRepository repository) : base(repository)
        {
            this._educationRepository = repository; 
        }

        [HttpGet("degree")]
        public ActionResult Gender()
        {
            var result = _educationRepository.ShowDegree();
            return Ok(result);
        }
    }
}
