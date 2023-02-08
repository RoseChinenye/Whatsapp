using Whatsapp.Model;

namespace Whatsapp
{
    public interface IWhatsappService : IDisposable
    {
        Task<long> CreateUser(UserViewModel user);

        Task<bool> UpdateUser(int id, UserViewModel user);

        Task<bool> DeleteUser(int id);

        Task<UserViewModel> GetUser(int id);

        Task <IEnumerable<UserViewModel>> GetUsers();

    }

    
}
