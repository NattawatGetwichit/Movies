using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Areas.Security.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly IDataProtector _protector;

        public SecurityController(IDataProtectionProvider protectionProvider)
        {
            _protector = protectionProvider.CreateProtector("value_secret_and_unique");
        }

        [HttpGet]
        public IActionResult Get()
        {
            string plainText = "Nattawat Getwichit";
            string encryptedText = _protector.Protect(plainText);
            string decryptedText = _protector.Unprotect(encryptedText);

            return Ok(new { plainText, encryptedText, decryptedText });
        }

        [HttpGet("timebound")]
        public IActionResult GetTimeBound()
        {
            var protectorTimeBound = _protector.ToTimeLimitedDataProtector();

            string plainText = "Nattawat Getwichit";
            string encryptedText = protectorTimeBound.Protect(plainText, lifetime: TimeSpan.FromSeconds(5));
            string decryptedText = protectorTimeBound.Unprotect(encryptedText);

            return Ok(new { plainText, encryptedText, decryptedText });
        }

        [HttpGet("timeboundasync")]
        public async Task<IActionResult> GetTimeBoundAsync()
        {
            var protectorTimeBound = _protector.ToTimeLimitedDataProtector();

            string plainText = "Nattawat Getwichit";
            string encryptedText = protectorTimeBound.Protect(plainText, lifetime: TimeSpan.FromSeconds(5));
            await Task.Delay(6000);

            try
            {
                string decryptedText = protectorTimeBound.Unprotect(encryptedText);
                return Ok(new { plainText, encryptedText, decryptedText });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}