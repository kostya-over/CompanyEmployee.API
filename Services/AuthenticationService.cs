using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticationService(ILoggerManager logger, IMapper mapper,
        UserManager<User> userManager, IConfiguration configuration)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }
    public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForCreationDto)
    {
        var user = _mapper.Map<User>(userForCreationDto);

        var result = await _userManager.CreateAsync(user, userForCreationDto.Password);

        if(result.Succeeded)
            await _userManager.AddToRolesAsync(user, userForCreationDto.Roles);
        
        return result;
    }
}