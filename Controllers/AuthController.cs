using EcoinverCultive_api.Models.Dto;
using EcoinverCultive_api.Services;
using EcoinverGMAO_api.Models.Dto;
using EcoinverGMAO_api.Services;
using EcoinverGMAO_api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly ITokenUserService _tokenUserService;
    public AuthController(
    IAuthService authService,
    ILogger<AuthController> logger,
    ITokenUserService tokenUserService)
    {
        _authService = authService;
        _logger = logger;
        _tokenUserService = tokenUserService;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var response = await _authService.AuthenticateAsync(loginDto.Username, loginDto.Password);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            var msg = "Credenciales Inválidas";
            _logger.LogError(ex, msg);
            return BadRequest(new { message = msg });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in api/auth/login");
            return StatusCode(500, new { message = ex.Message });
        }
    }
    [HttpGet("me")]
    
    public IActionResult GetCurrentUser()
    {
        Console.WriteLine($"¿Autenticado?: {User.Identity.IsAuthenticated}");

        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"CLAIM: {claim.Type} = {claim.Value}");
        }

        
        var userInfo = _tokenUserService.GetUserFromToken(User);
        return Ok(userInfo);
    }

    //autologin para pasar del hub al cultive el usuario iniciado sesion:
    //[HttpPost("auto-login")]
    //public async Task<IActionResult> AutoLogin([FromBody] AutoLoginDto dto)
    //{
    //    try
    //    {
    //        var response = await _authService.AutoLoginAsync(dto.Username, dto.Email);
    //        return Ok(response);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error en auto-login");
    //        return StatusCode(500, new { message = ex.Message });
    //    }
    //}




}
