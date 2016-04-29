using Tekhub.Infrastructure.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tekhub.Infrastructure.Security
{
    public class RFC2898DeriveBytesProvider: IDeriveBytes
    {
        private Rfc2898DeriveBytes DeriveBytesProvider;

        public RFC2898DeriveBytesProvider()
        {
        }

        public void Dispose()
        {
            if (this.DeriveBytesProvider != null)
            {
                this.DeriveBytesProvider.Dispose();
            }
        }

        public byte[] GetBytes(string clearText, byte[] salt, int iterations, int outputBytesLength)
        {
            this.DeriveBytesProvider = new Rfc2898DeriveBytes(clearText, salt);
            this.DeriveBytesProvider.IterationCount = iterations;
            return this.DeriveBytesProvider.GetBytes(outputBytesLength);
        }
    }
}
