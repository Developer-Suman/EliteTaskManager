using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Cryptographys
{
    public interface ICryptography
    {

        Task<string> Base64UrlEncoder(string value);
        Task<string> Base64UrlDecoder(string value);
        Task<string> Encrypt(string password, byte[] key, byte[] iv);
        Task<string> Decrypt(string encrypt, byte[] key, byte[] iv);

        Task<byte[]> GenerateSecureKey();
        Task<byte[]> GenerateSecureIV();
    }
}
