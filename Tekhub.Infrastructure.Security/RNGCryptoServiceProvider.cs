using Tekhub.Infrastructure.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekhub.Infrastructure.Security
{
    public class RNGCryptoServiceProvider: ICryptoServiceProvider
    {
        private System.Security.Cryptography.RNGCryptoServiceProvider CryptoServiceProvider;

        public RNGCryptoServiceProvider()
        {
            this.CryptoServiceProvider = new System.Security.Cryptography.RNGCryptoServiceProvider();
        }

        public void GetBytes(byte[] salt)
        {
            this.CryptoServiceProvider.GetBytes(salt);
        }

        public void Dispose()
        {
            if (this.CryptoServiceProvider != null)
            {
                this.CryptoServiceProvider.Dispose();
            }
        }
    }
}
