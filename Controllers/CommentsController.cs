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
    public class CommentsController : ControllerBase
    {
        private readonly PempoContext _context;

        public CommentsController(PempoContext context)
        {
            _context = context;
        }                      
        
        [HttpPost("PostComments")]
        public async Task<ActionResult<Comments>> PostComments(Comments comments)
        {
            try
            {                
                var comment = new Comments
                {
                    PostId = comments.PostId,
                    Comment = comments.Comment
                };

                _context.tblComments.Add(comment);
                var saved = await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Comment created successfully!!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    ex.Message
                });               
            }          
        }                              
    }
}
