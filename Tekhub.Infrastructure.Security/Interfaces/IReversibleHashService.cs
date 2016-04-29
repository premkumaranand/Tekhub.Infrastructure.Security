namespace Tekhub.Infrastructure.Security.Interfaces
{
    public interface IReversibleHashService
    {
        string EncryptHash(string input, string purpose);
        string DecryptHash(string hashedInput, string purpose);
    }
}