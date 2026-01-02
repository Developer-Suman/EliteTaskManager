using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IOtpService
    {
        Task<bool> SendOtpAsync(string Email, int otp);
        Task<bool> VerifyOtpAsync(string Email, int otp);
    }
}
