using System;
using System.Text;
using System.Web;
using Tekhub.Infrastructure.Security.Interfaces;
using System.Web.Security;

namespace Tekhub.Infrastructure.Security
{
    public class ReversibleHashService : IReversibleHashService
    {
        private const string ReversibleHashKey = "Y*hGJ7Nd!pHe@2BCBb7aZavtSZE&&5R4w^6t6UenZuXqAMycC8kfjYPw2a5^97rG";

        public string EncryptHash(string input, string purpose)
        {
            var saltedInput = string.Format("{0}{1}{2}", input, ReversibleHashKey, purpose);
            var utfEightBytes = Encoding.UTF8.GetBytes(saltedInput);
            var machineProtectedBytes = MachineKey.Protect(utfEightBytes, purpose);
            var base64 = Convert.ToBase64String(machineProtectedBytes);
            return HttpUtility.UrlEncode(base64);
        }

        public string DecryptHash(string hashedInput, string purpose)
        {
            var base64 = HttpUtility.UrlDecode(hashedInput);
            if (base64 == null)
                throw new Exception();

            var machineProtectedBytes = Convert.FromBase64String(base64);
            var utfEightBytes = MachineKey.Unprotect(machineProtectedBytes, purpose);
            if (utfEightBytes == null)
                throw new Exception();

            var saltedInput = Encoding.UTF8.GetString(utfEightBytes);
            return saltedInput.Replace(ReversibleHashKey, string.Empty).Replace(purpose, string.Empty);
        }
    }
}