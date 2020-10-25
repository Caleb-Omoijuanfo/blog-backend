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
        [HttpGet("FetchPost")]
        public async Task<ActionResult<IEnumerable<Post>>> FetchPosts(int start, int length)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var id = int.Parse(User.Claims.First(u => u.Type == ClaimTypes.Name).Value);

                    var listCount = _context.tblPost.AsNoTracking().Count();
                    int value = (listCount - start < 0 ? 1 : (listCount - start));


                    var posts = await _context.tblPost.AsNoTracking().OrderByDescending(m => m.DateLastUpdated).Skip(start).Take(Math.Min(length, value)).ToListAsync();
                                     

                    return Ok(new
                    {
                        recordsTotal = listCount,
                        recordsFiltered = length > listCount ? listCount : length,
                        Data = listCount > 0 ? posts : null
                    });
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

        // GET: api/Posts/5
        [HttpGet("Fetch/One")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var post = _context.tblPost.AsNoTracking().Where(p => p.Id == id).FirstOrDefault();
                    var comments = await _context.tblComments.AsNoTracking().Where(c => c.PostId == id).ToListAsync();
                    var likes = await _context.tblLikes.AsNoTracking().Where(l => l.PostId == id).ToListAsync();

                    if (post == null)
                    {
                        return NotFound(new
                        {
                            Message = "Post does not exist",
                            responseCode = ePempoStatus.notFound
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            Message = "Post retrieved successfully!!",
                            Data = new
                            {
                                post,
                                comments,
                                likes
                            },
                            responeCode = ePempoStatus.success
                        });
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Wrong/Expired token"                        
                    });
                }               
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                    responseCode = ePempoStatus.failure
                });   
            }            
        }
        
        [HttpPut("Update")]
        public async Task<IActionResult> UpdatePost(int id, Post post)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (id != post.Id)
                    {
                        return BadRequest (new 
                        { 
                            Message = "Incorrect post id"
                        });
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
                            return NotFound(new
                            { 
                                Message = "Post not found",
                                responseCode = ePempoStatus.notFound
                            });
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return NoContent();
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Wrong/Expired token"
                    });
                }
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    Message = "Something went wrong"
                });               
            }            
        }

        // POST: api/Posts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("CreatePost")]
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
        [HttpDelete("Delete")]
        public async Task<ActionResult<Post>> DeletePost(int id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var post = await _context.tblPost.FindAsync(id);
                    var comments = await _context.tblComments.Where(c => c.PostId == id).ToListAsync();

                    if (post == null)
                    {
                        return NotFound(new
                        {
                            Message = "Post not found!",
                            responseCode = ePempoStatus.notFound
                        });
                    }
                    else
                    {
                        _context.tblPost.Remove(post);
                        foreach (var comment in comments)
                        {
                            _context.tblComments.Remove(comment);
                        }
                        await _context.SaveChangesAsync();
                    }
                   

                    return Ok(new
                    {
                        Message = "Post deleted successfully!",
                        responseCode = ePempoStatus.success
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Wrong/Expired token"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    ex.Message
                });
            }

        }

        private bool PostExists(int id)
        {
            return _context.tblPost.Any(e => e.Id == id);
        }
    }
}
