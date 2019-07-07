namespace GoshoSecurity.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IAccountService
    {
        Task<bool> Delete(string userId);
        Task<string> GetTokenForUser(string username, string password);

    }
}
