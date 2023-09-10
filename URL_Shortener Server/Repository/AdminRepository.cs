using URL_Shortener_Server.Interfaces;
using URL_Shortener_Server.Models;

public class AdminRepository : IAdminRepository
{
    private readonly DataContext _context;

    public AdminRepository(DataContext context)
    {
        _context = context;
    }

    public Admin GetAdminById(int id)
    {
        return _context.Admins.FirstOrDefault(a => a.Id == id);
    }

    public Admin GetAdminByUsername(string username)
    {
        return _context.Admins.FirstOrDefault(a => a.Username == username);
    }

    public void AddAdmin(Admin admin)
    {
        _context.Admins.Add(admin);
        _context.SaveChanges();
    }

 
}