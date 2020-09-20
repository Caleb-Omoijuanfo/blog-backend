﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Pempo_backend.Model;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Pempo_backend.PempoEnums.PempoEnums;

namespace Pempo_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PempoController : ControllerBase
    {
        private readonly PempoContext _context;
        private IConfiguration _config;

        public PempoController(PempoContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        private string GenerateWebToken(Admin admin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {               
                new Claim(ClaimTypes.Email, admin.Email)
               };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddSeconds(120),
                signingCredentials: credentials);

            return tokenHandler.WriteToken(token);
        }
        // POST: api/Admins
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("Register")]
        public async Task<ActionResult<Admin>> Register([FromBody] Admin admin)
        {
            try
            {               
                if (ModelState.IsValid)
                {
                    var adminExist = _context.tblAdmin.Where(m => m.Email == admin.Email).FirstOrDefault();
                    if (adminExist == null) 
                    {
                        var data = Utilities.Utilities.HashPassword(admin.Password);
                        var modifiedAdminObject = new Admin
                        {
                            FirstName = admin.FirstName,
                            LastName = admin.LastName,
                            Email = admin.Email,
                            Password = data.Hash,
                            Salt = data.Salt
                        };

                        _context.tblAdmin.Add(modifiedAdminObject);
                        await _context.SaveChangesAsync();                        

                        return Ok(new
                        {
                            Message = "Admmin created successfully!",
                            ResponseCode = ePempoStatus.success                            
                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            Message = "Record already exists",
                            ResponseCode = ePempoStatus.failure
                        });
                    }                    
                }
                else
                {
                    return BadRequest(new { message = "Something Went Wrong" });
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

        [HttpPost("Login")]
        public ActionResult Login(Admin admin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userExist = _context.tblAdmin.Where(m => m.Email == admin.Email).FirstOrDefault();

                    if (userExist != null)
                    {
                        var userSalt = userExist.Salt;
                        var userHash = userExist.Password;

                        var newHash = Utilities.Utilities.HashPassword(admin.Password, userSalt);

                        //compare hash values

                        if (newHash == userHash)
                        {
                            var token = GenerateWebToken(admin);

                            return Ok(new
                            {
                                Token = token
                            });
                        }
                        else
                        {
                            return BadRequest(new
                            {
                                Message = "Hash doesn't match"
                            });
                        }
                    }
                    else
                    {
                        return Ok(new
                        {
                            Message = "User does not exist"
                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        Message = "Something went wrong"
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

        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Admin>> DeleteAdmin(int id)
        {
            var admin = await _context.tblAdmin.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            _context.tblAdmin.Remove(admin);
            await _context.SaveChangesAsync();

            return admin;
        }

        private bool AdminExists(int id)
        {
            return _context.tblAdmin.Any(e => e.Id == id);
        }
    }
}
