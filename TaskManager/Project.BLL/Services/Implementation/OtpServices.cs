using Microsoft.Extensions.Configuration;
using Project.BLL.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Implementation
{
    public class OtpServices : IOtpService
    {

        private readonly IConfiguration _configuration;
        private readonly IEmailServices _emailServices;
        private readonly Dictionary<string, int> _otpStore = new Dictionary<string, int>();

        public OtpServices(IConfiguration configuration, IEmailServices emailServices)
        {
            _configuration = configuration;
            _emailServices = emailServices;
            
        }
        public async Task<bool> SendOtpAsync(string Email, int otp)
        {
            _otpStore[Email] = otp;

            var data = _otpStore[Email];

            var subject = "OTP Verification to Register";
            var body = $"Your OTP is: {otp}";
            await _emailServices.SendEmailAsync(Email, subject, body);
            return true;
        }

        public async Task<bool> VerifyOtpAsync(string Email, int otp)
        {
            if (_otpStore.TryGetValue(Email, out var storeOTP) && storeOTP == otp) 
            {
                _otpStore.Remove(Email);
                return true;
            }
            return false;
        }
    }
}
