using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi_Demo.Authentication;
using WebApi_Demo.Dtos;

namespace WebApi_Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IConfiguration config;

    public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config)
    {
        this.userManager = userManager;
        this.config = config;
    }
    [HttpPost("register")] // api/Account/register
    public async Task<IActionResult> Register(RegisterUserDto userDto)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser();
            user.UserName = userDto.Email;
            user.Email = userDto.Email;
            IdentityResult result = await userManager.CreateAsync(user, userDto.Password!);
            if (result.Succeeded)
            {
                return Ok("Account Added Successfully");
            }
            return BadRequest(result.Errors.FirstOrDefault());
        }
        return BadRequest(ModelState);
    }
    [HttpPost("Login")]  // api/Account/Login
    public async Task<IActionResult> Login(LoginUserDto userDto)
    {
        if (ModelState.IsValid)
        {
            //Check - Create Token
            ApplicationUser? user = await userManager!.FindByEmailAsync(userDto.Email!);
            if (user != null)  //Email Found
            {
                bool found = await userManager.CheckPasswordAsync(user, userDto.Password!);
                if (found)
                {
                    //Claims Token
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.Email!));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                    // get role
                    var roles = await userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    SecurityKey securityKey =              // key in SigningCredentials
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]!));

                    SigningCredentials signingCredentials =    // need key and algorithm
                        new SigningCredentials(
                        securityKey
                       , SecurityAlgorithms.HmacSha256);

                    // Create Token
                    JwtSecurityToken token = new JwtSecurityToken(
                        issuer: config["JWT:ValidIssuer"], // url web api (provider)
                        audience: config["JWT:ValidAudience"], //url consumer (angular)
                        claims: claims,      // list of claims
                        expires: DateTime.UtcNow.AddHours(1),  // expiring at 
                        signingCredentials: signingCredentials
                        );
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                return Unauthorized();
            }
            return Unauthorized();
        }
        return Unauthorized();
    }
}
