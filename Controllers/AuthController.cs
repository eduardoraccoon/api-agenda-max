using Microsoft.AspNetCore.Mvc;
using api_iso_med_pg.DTOs;
using api_iso_med_pg.Models;
using api_iso_med_pg.Data.Interfaces;
using api_iso_med_pg.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
namespace api_iso_med_pg.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    public AuthController(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<BaseResponse<string>>> Register(UserRegisterDto dto)
    {
        var existing = await _userRepository.GetByUsernameAsync(dto.Username);
        if (existing != null)
        {
            return BadRequest(new BaseResponse<string>
            {
                IsSuccess = false,
                Data = null,
                Message = ReplyMessage.MESSAGE_EXISTS
            });
        }
        var user = new User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        await _userRepository.AddAsync(user);
        return Ok(new BaseResponse<string>
        {
            IsSuccess = true,
            Data = user.Username,
            Message = ReplyMessage.MESSAGE_SAVE
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<BaseResponse<string>>> Login(UserLoginDto dto)
    {
        var user = await _userRepository.GetByUsernameAsync(dto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return Unauthorized(new BaseResponse<string>
            {
                IsSuccess = false,
                Data = null,
                Message = ReplyMessage.MESSAGE_TOKEN_ERROR
            });
        }
        var token = GenerateJwtToken(user);
        return Ok(new BaseResponse<string>
        {
            IsSuccess = true,
            Data = token,
            Message = ReplyMessage.MESSAGE_TOKEN
        });
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("id", user.Id.ToString())
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
