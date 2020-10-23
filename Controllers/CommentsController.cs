﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pempo_backend.Model;

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

        // POST: api/Comments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("Comment/Create")]
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

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Comments>> DeleteComments(int id)
        {
            var comments = await _context.tblComments.FindAsync(id);
            if (comments == null)
            {
                return NotFound();
            }

            _context.tblComments.Remove(comments);
            await _context.SaveChangesAsync();

            return comments;
        }        
    }
}