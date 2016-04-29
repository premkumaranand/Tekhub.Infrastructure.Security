using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekhub.Framework;
using Tekhub.Infrastructure.Security.Interfaces;

namespace Tekhub.Infrastructure.Security
{
    public class RngHashService : IHashService
    {
        private readonly ICryptoServiceProvider _cryptoServiceProvider;
        private readonly IDeriveBytes _deriveBytesProvider;

        public RngHashService(ICryptoServiceProvider cryptoServiceProvider, IDeriveBytes deriveBytesProvider)
        {
            _cryptoServiceProvider = cryptoServiceProvider;
            _deriveBytesProvider = deriveBytesProvider;
        }

        public const int SaltByteSize = 16;
        public const int HashByteSize = 16;
        public const int Pbkdf2Iterations = 1000;

        public const int IterationIndex = 0;
        public const int SaltIndex = 1;
        public const int Pbkdf2Index = 2;

        public string CreateHash(string clearText)
        {
            if (string.IsNullOrEmpty(clearText))
            {
                throw new ArgumentException("clearText");
            }

            var salt = new byte[SaltByteSize];

            // Generate a random salt
            using (var csprng = _cryptoServiceProvider)
            {
                csprng.GetBytes(salt);
            }

            // Hash the password and encode the parameters
            var hash = Pbkdf2(clearText, salt, Pbkdf2Iterations, HashByteSize);
            return Pbkdf2Iterations + ":" + Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        public bool ValidateClearText(string clearText, string correctHash)
        {
            if (clearText == null)
            {
                throw new ArgumentNullException("clearText");
            }

            if (correctHash == null)
            {
                throw new ArgumentNullException("correctHash");
            }

            // Extract the parameters from the hash
            var delimiter = new[] { ':' };
            var split = correctHash.Split(delimiter);
            var iterations = int.Parse(split[IterationIndex]);
            var salt = Convert.FromBase64String(split[SaltIndex]);
            var hash = Convert.FromBase64String(split[Pbkdf2Index]);
            var testHash = Pbkdf2(clearText, salt, iterations, hash.Length);

            return Equals(hash, testHash);
        }

        protected byte[] Pbkdf2(string clearText, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = _deriveBytesProvider)
            {
                return pbkdf2.GetBytes(clearText, salt, iterations, outputBytes);
            }
        }

        protected bool Equals(IList<byte> a, IList<byte> b)
        {
            var diff = (uint)a.Count ^ (uint)b.Count;

            for (var i = 0; (i < a.Count) && (i < b.Count); i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }

            return diff == 0;
        }
    }
}
