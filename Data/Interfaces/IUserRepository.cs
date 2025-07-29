using api_iso_med_pg.Models;

namespace api_iso_med_pg.Data.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User> AddAsync(User user);
}
