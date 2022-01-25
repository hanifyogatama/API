using API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;

namespace API.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repository;

        public BaseController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult<Entity> Get()
        {
            var result = repository.Get();
            if (result.Count() > 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "showing record" });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, result, message = "record is empty" });
            }
        }

        [HttpPost]
        public ActionResult<Entity> Post(Entity entity)
        {
            var result = repository.Insert(entity);
            if(result != 0)
            {
                return Ok(new { status = HttpStatusCode.OK, message = "data has been added" });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "data cannot added" });
            }
        }

        [HttpGet("{key}")]
        public ActionResult<Entity> Get(Key key)
        {
             var result = repository.Get(key);

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
        public ActionResult Update(Entity entity)
        {
            var result = repository.Update(entity);
            switch (result)
            {
                case 0:
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "record cannot updated" });
                case 1:
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "record has been updated" });
                default:
                    return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "internal server error" });
            }
        }


        [HttpDelete("{Key}")]
        public ActionResult<Entity> Delete(Key key)
        {
            var result = repository.Delete(key);

            switch (result)
            {
                case 1:
                    return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "id not found" });
                case 2:
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "record has been deleted" });
                case 3:
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "failed to delete record" });
                default:
                    return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "internal server error" });

            }
        }
    }
}
