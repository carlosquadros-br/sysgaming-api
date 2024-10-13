using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SysgamingApi.Src.Application.Dtos;
using SysgamingApi.Src.Application.Users.Token;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Infrastructure.Persistence.Repositories;

public class UserRepository : AbstractRepository<User>, IUserRepository, ITokenProvider
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public UserRepository(UserManager<User> userManager, IAppDbContext appDbContext, IConfiguration configuration) : base(appDbContext)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<User> CreateAsync(User entity, string password)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        var result = await _userManager.CreateAsync(entity, password);
        if (!result.Succeeded)
        {
            throw new Exception("Error creating user", new Exception(result.Errors.ToString()));
        }
        return entity;

    }

    public async Task<User> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("User not found");

        var result = await _userManager.CheckPasswordAsync(user, password);
        if (!result)
        {
            throw new Exception("Invalid password");
        }
        return user;
    }

    public async Task<User> GetbyEmail(string email)
    {
        var result = _userManager.FindByEmailAsync(email).ConfigureAwait(false);
        return await result;
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new(ClaimTypes.Name, user.UserName.ToString()),
                    new(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _configuration["JWT:ValidAudience"],
            Issuer = _configuration["JWT:ValidIssuer"]
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}
