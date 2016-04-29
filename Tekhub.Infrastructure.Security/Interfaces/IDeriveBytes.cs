using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekhub.Infrastructure.Security.Interfaces
{
    public interface IDeriveBytes: IDisposable
    {
        byte[] GetBytes(string clearText, byte[] salt, int iterations, int outputBytesLength);
    }
}
