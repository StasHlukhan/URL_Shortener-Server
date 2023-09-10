using URL_Shortener_Server.Models;

namespace URL_Shortener_Server.Interfaces
{
    public interface IAdminRepository
    {
        Admin GetAdminById(int id);
        Admin GetAdminByUsername(string username);
        void AddAdmin(Admin admin);

    }

}
