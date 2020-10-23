using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pempo_backend.Model;
using static Pempo_backend.PempoEnums.PempoEnums;

namespace Pempo_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly PempoContext _context;

        public LikesController(PempoContext context)
        {
            _context = context;
        }              
       
        [HttpPost("PostLike")]
        public async Task<ActionResult<Likes>> PostLikes(Likes like)
        {
            try
            {
                Likes newLike = new Likes
                {
                    Count = 1,
                    PostId = like.PostId
                };

                _context.tblLikes.Add(newLike);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Like added!!",
                    responseCode = ePempoStatus.success
                });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    Message = "Something went Wrong",
                    responseCode = ePempoStatus.failure
                });
            }            
        }        
    }
}
