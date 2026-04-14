using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;
using MonitoringSystemPCGWebAPI.Project.Services.Interfaces;
using Services.Interfaces;
using Utilities.Interfaces;

namespace MonitoringSystemPCGWebAPI.Project.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IEmailSenderUtility _emailSenderUtility;
        private readonly IAuthService _authService;
        private readonly IJwtUtility _jwtUtility;
        private readonly IOtpVerificationsService _otpVerificationsService;
        public AuthController(IEmailSenderUtility emailSenderUtility, IAuthService authService, IJwtUtility jwtUtility,IOtpVerificationsService otpVerificationsService)
        {
            _emailSenderUtility = emailSenderUtility;
            _authService = authService;
            _jwtUtility = jwtUtility;
            _otpVerificationsService = otpVerificationsService;
        }
        //[HttpGet]
        //public async Task<IActionResult> GetPersonnelOnGoingActivities()
        //{
        //    try
        //    {
        //        await _emailSenderUtility.SendEmailAsync("hita.jm08@gmail.com", "Test Email", "You have already exceeded the allowable passes for this quarter. For further concerns, you may reach the Chief Master-at-Arms, RTC Aurora. Thank you.");
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login parameters)
        {
            try
            {
                Usertbl? user = await _authService.Login(parameters);
                string token = _jwtUtility.GenerateToken(user?.UserId ?? 0);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,   
                    Secure = true,    
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("jwt_token", token, cookieOptions);

                return Ok(user);
                    
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOTP([FromBody] OtpDTO otp)
        {
            try
            {
                OtpVerifications? data = await _otpVerificationsService.InsertAsync(otp);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] Signup parameters)
        {
            try
            {
                var user = await _authService.Signup(parameters);
                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
