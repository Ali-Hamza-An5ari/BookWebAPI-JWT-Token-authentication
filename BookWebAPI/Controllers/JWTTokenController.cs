using BookWebAPI.Data;
using BookWebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTTokenController : ControllerBase
    {
        public IConfiguration _configuration;
        public readonly BooksDbContext _context;

        public JWTTokenController(IConfiguration configuration, BooksDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            if(user != null && user.AuthorPseudoName != null && user.Password != null)
            {
                var userData = await GetUser(user.AuthorPseudoName, user.Password);
                var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
                
                if(user !=null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,jwt.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("Id",user.UserId.ToString()),
                        new Claim("AuthorPseudoName",user.AuthorPseudoName),
                        new Claim("Password", user.Password)

                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                    var SignIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                            jwt.Issuer,
                            jwt.Audience,
                            claims,
                            expires: DateTime.Now.AddMinutes(20),
                            signingCredentials: SignIn
                         );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                }
                else
                {
                    return BadRequest("Invalid Credentials");

                }
            }
            else
            {
                return BadRequest("Invalid Credentials");
            }
        }

        private async Task<User> GetUser(string authorPseudoName, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.AuthorPseudoName == authorPseudoName && u.Password == password);
        }
    }
}
