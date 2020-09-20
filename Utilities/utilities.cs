using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pempo_backend.Model;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Pempo_backend.Utilities
{
    public class Utilities  
    {                
        public static PasswordHashModel HashPassword (string passwordSupplied)
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordSupplied,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            PasswordHashModel data = new PasswordHashModel
            {
                Salt = salt,
                Hash = hashed
            };

            return data;
        }

        public static string HashPassword(string passwordSupplied, byte[] salt)
        {           
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordSupplied,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
