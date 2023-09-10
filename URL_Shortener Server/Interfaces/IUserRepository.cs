using URL_Shortener_Server.Models;

namespace URL_Shortener_Server.Interfaces
{
    public interface IUserRepository
    {
        User GetUserById(int id);
        User GetUserByUsername(string username);
        void AddUser(User user);

    }
}