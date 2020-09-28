using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class PostController : ControllerBase
    {
        private readonly PempoContext _context;

        public PostController(PempoContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GettblPost()
        {
            return await _context.tblPost.ToListAsync();
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.tblPost.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Posts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("Post")]
        public async Task<ActionResult> CreatePost(Post post)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var id = int.Parse(User.Claims.First(u => u.Type == ClaimTypes.Name).Value);

                    var postExists = _context.tblPost.Where(m => m.AdminId == id && m.Title == post.Title).FirstOrDefault();

                    if (postExists != null)
                    {
                        return BadRequest(new
                        {
                            Message = "Post already exists",
                            ResponseCode = ePempoStatus.failure
                        });
                    }
                    else
                    {
                        _context.tblPost.Add(post);
                        var saved = await _context.SaveChangesAsync();

                        return CreatedAtAction("GetPost", new { id = post.Id }, saved);
                    }                   
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Wrong/Invalid token"
                    });
                }                                                                                       
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }            
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(int id)
        {
            var post = await _context.tblPost.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.tblPost.Remove(post);
            await _context.SaveChangesAsync();

            return post;
        }

        private bool PostExists(int id)
        {
            return _context.tblPost.Any(e => e.Id == id);
        }
    }
}
