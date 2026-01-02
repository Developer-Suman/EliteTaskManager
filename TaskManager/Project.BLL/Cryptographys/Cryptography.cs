

using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Project.BLL.Cryptographys
{
    public class Cryptography : ICryptography
    {
        public Task<string> Base64UrlDecoder(string value)
        {
            var decodedBytes = WebEncoders.Base64UrlDecode(value);
            var decodedValue = Encoding.UTF8.GetString(decodedBytes);
            return Task.FromResult(decodedValue);

        }

        public Task<string> Base64UrlEncoder(string value)
        {
            var encodedValue = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(value));
            return Task.FromResult(encodedValue);
        }

        public async Task<string> Decrypt(string encrypt, byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using(var decryptor =  aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(Convert.FromBase64String(encrypt)))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cs))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
           
        }

        public async Task<string> Encrypt(string input, byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(cs))
                        {
                            writer.Write(input);
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        public async Task<byte[]> GenerateSecureIV()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] iv = new byte[16]; // AES block size
                rng.GetBytes(iv);
                return iv;
            }
        }

        public async Task<byte[]> GenerateSecureKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[32]; // AES-256
                rng.GetBytes(key);
                return key;
            }
        }
    }
}
